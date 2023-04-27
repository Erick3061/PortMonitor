using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSER
{
    public class EventoVideo
    {
        public DateTime FechaHora { set; get; }

        public string CodigoAbonado { set; get; }

        public string Video { set; get; }

        public EventoVideo(DateTime FechaHora, string CodigoAbonado, string Video)
        {
            this.FechaHora = FechaHora;
            this.CodigoAbonado = CodigoAbonado;
            this.Video = Video;
        }

        public EventoVideo()
        {
        }
    }
}
