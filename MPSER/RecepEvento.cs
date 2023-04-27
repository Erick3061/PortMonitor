using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSER
{
    public class RecepEvento
    {
        public byte CodigoReceptora { set; get; }

        public byte CodigoProtocolo { set; get; }

        public byte EdoCanal { set; get; }

        public string Valor { set; get; }

        public string CodigoEvento { set; get; }
    }
}
