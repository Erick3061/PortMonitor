using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSER
{
    public class RecepcionIP
    {
        public byte CodigoEquipo { set; get; }

        public byte CodigoReceptora { set; get; }

        public string IP { set; get; }

        public int Puerto { set; get; }
    }
}
