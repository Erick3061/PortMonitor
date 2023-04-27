using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPSER
{
    public class Parametros
    {
        public string ServerIP { set; get; }

        public int ServerPuerto { set; get; }

        public bool CodigoAbonadoRepetido { set; get; }

        public string CodEventoAReemp { set; get; }

        public string CodZona1 { set; get; }

        public string CodEventoReemp1 { set; get; }

        public string CodZona2 { set; get; }

        public string CodEventoReemp2 { set; get; }

        public string CodEventoError { set; get; }

        public string CodEventoAReemp2 { set; get; }

        public string CodZona3 { set; get; }

        public string CodEventoReemp3 { set; get; }

        public string CodZona4 { set; get; }

        public string CodEventoReemp4 { set; get; }

        public Parametros(string ServerIP, int ServerPuerto, bool CodigoAbonadoRepetido)
        {
            this.ServerIP = ServerIP;
            this.ServerPuerto = ServerPuerto;
            this.CodigoAbonadoRepetido = CodigoAbonadoRepetido;
        }

        public Parametros()
        {
        }
    }

}
