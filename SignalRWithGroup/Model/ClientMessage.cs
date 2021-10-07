using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWithGroup.Model
{
    public class ClientMessage
    {
        public string IdUserPengirim { get; set; }
        public string IdUserPenerima { get; set; }
        public long IdPesanChat { get; set; }
        public string Divisi { get; set; }
        public string IsiPesan { get; set; }
        public byte[] PesanGambar { get; set; }
        public string PesanChat { get; set; }
        public string JenisPesan { get; set; }
        public object Id_PrimaryKey { get; set; }
        public string NamaHalaman { get; set; }
    }
}
