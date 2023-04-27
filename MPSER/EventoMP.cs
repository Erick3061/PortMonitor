using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSER
{
    public class EventoMP
    {
        public string EventoOriginador { set; get; }

        public DateTime FechaHoraEvento { set; get; }

        public DateTime FechaHoraRespuesta { set; get; }

        public string NomBD { set; get; }
    }
}
