using Microsoft.AspNetCore.SignalR;
using SignalRWithGroup.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace SignalRWithGroup.Hubs
{
    public class ChatHub : Hub
    {
        static HashSet<User> listUser = new HashSet<User>();
        static HashSet<ClientMessage> listPesan = new HashSet<ClientMessage>();

        public async Task<User> CekUser(string idUser)
        {
            var cekDataUser = listUser.Where(i => i.IdUser == idUser).FirstOrDefault();
            return cekDataUser;
        }
        public async Task MulaiKoneksi(string divisi, string idUser)
        {
            //Manage User
            var cekDataUser = await CekUser(idUser);
            if (cekDataUser == null)
            {
                listUser.Add(new User
                {
                    IdUser = idUser,
                    IdKoneksi = Context.ConnectionId,
                    Divisi = divisi,
                });
            }
            else
            {
                cekDataUser.IdKoneksi = Context.ConnectionId;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, divisi);

            //Kirim Pesan yang belum terkirim ke user penerima kembali online
            var dataChat = listPesan.Where(i => i.IdUserPenerima == idUser || i.IdUserPengirim == idUser).ToList();
            foreach (var chat in dataChat)
            {
                await Clients.Clients(Context.ConnectionId).SendAsync("KirimPesanChat", chat);
            }
        }

        public async Task KirimPesan(ClientMessage clientMessage)
        {
           await Clients.OthersInGroup(clientMessage.Divisi).SendAsync("KirimPesan", clientMessage);
        }

        public async Task KirimPesanBroadcast(ClientMessage clientMessage)
        {
            await Clients.Others.SendAsync("KirimPesanBroadcast", clientMessage);
        }

        public async Task KirimPesanChat(ClientMessage clientMessage)
        {
            listPesan.Add(clientMessage);
            var cekUserPenerima = await CekUser(clientMessage.IdUserPenerima);
            if (cekUserPenerima == null)
            {
                await Clients.Clients(Context.ConnectionId).SendAsync("KirimPesanChat", clientMessage);
            }
            else
            {
                await Clients.Clients(Context.ConnectionId, cekUserPenerima.IdKoneksi).SendAsync("KirimPesanChat", clientMessage);

            }
        }

        public async Task StopKoneksi(string divisi, string idUser)
        {
            var cekDataUser = await CekUser(idUser);
            listUser.Remove(cekDataUser);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, divisi);
        }
    }
}