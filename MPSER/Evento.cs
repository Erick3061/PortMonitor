using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSER
{
    public class Evento
    {
        public DateTime FechaHora { set; get; }

        public string ReceptorNum { set; get; }

        public string LineaNum { set; get; }

        public string CodigoAbonado { set; get; }

        public string CodigoEvento { set; get; }

        public string CodigoZona { set; get; }

        public string UsuarioNum { set; get; }

        public string Calificador { set; get; }

        public string GrupoNum { set; get; }

        public byte CodigoReceptora { set; get; }

        public byte CodigoReceptoraReal { set; get; }

        public byte PuertoCommNum { set; get; }

        public byte CodigoProtocolo { set; get; }

        public string EventoOriginador { set; get; }

        public string Estado { set; get; }

        public string ContEnEspera { set; get; }

        public byte CalificacionEvento { set; get; }

        public string CallerID { set; get; }

        public byte Particion { set; get; }

        public string longitudCad { set; get; }

        public Evento(
          DateTime FechaHora,
          string ReceptorNum,
          string LineaNum,
          string CodigoAbonado,
          string CodigoEvento,
          string CodigoZona,
          string UsuarioNum,
          string Calificador,
          string GrupoNum,
          byte CodigoReceptora,
          byte PuertoCommNum,
          byte CodigoProtocolo,
          string EventoOriginador,
          string Estado,
          string longitudC)
        {
            this.FechaHora = FechaHora;
            this.ReceptorNum = ReceptorNum;
            this.LineaNum = LineaNum;
            this.CodigoAbonado = CodigoAbonado;
            this.CodigoEvento = CodigoEvento;
            this.CodigoZona = CodigoZona;
            this.UsuarioNum = UsuarioNum;
            this.Calificador = Calificador;
            this.GrupoNum = GrupoNum;
            this.longitudCad = longitudC;
            this.CodigoReceptora = CodigoReceptora;
            this.PuertoCommNum = PuertoCommNum;
            this.CodigoProtocolo = CodigoProtocolo;
            this.EventoOriginador = EventoOriginador;
            this.Estado = Estado;
        }

        public Evento()
        {
        }

        public void LimpiaProps()
        {
            this.ReceptorNum = (string)null;
            this.LineaNum = (string)null;
            this.CodigoAbonado = (string)null;
            this.CodigoEvento = (string)null;
            this.CodigoZona = (string)null;
            this.UsuarioNum = (string)null;
            this.Calificador = (string)null;
            this.GrupoNum = (string)null;
            this.CodigoReceptora = (byte)0;
            this.PuertoCommNum = (byte)0;
            this.CodigoProtocolo = (byte)0;
            this.EventoOriginador = (string)null;
            this.Particion = (byte)0;
        }
    }

}
