//using Comun;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Security.Principal;

//namespace MPSER
//{
//    public class _MPCLogica
//    {
//        public _MPCAccesoADatos cadPuertos = new _MPCAccesoADatos();

//        public bool Main()
//        {
//            Sistema.strLocalAssemblyName = "MPSER";
//            if (!this.GetParameters())
//            {
//                if (!this.ExisteError()) ;
//                return false;
//            }
//            Sistema.bytCodigoEmpresaReal = Convert.ToByte(Sistema.sysCodigoEmpresa);
//            Sistema.strPathSistema = "C:\\Siatec\\SiatecInfo\\";
//            Sistema.strPathEmpresa = "C:\\Siatec\\SiatecInfo\\G" + Sistema.sysCodigoGrupo + "\\E" + Sistema.sysCodigoEmpresa + "\\";
//            Sistema.strPathGrupo = "C:\\Siatec\\SiatecInfo\\G" + Sistema.sysCodigoGrupo + "\\E0";
//            return true;
//        }

//        public bool GetParameters()
//        {
//            bool flag = false;
//            StreamReader @null = StreamReader.Null;
//            string[] array = new string[16];
//            int num = 0;
//            int num2 = 0;
//            string text = null;
//            string text2 = null;
//            flag = false;
//            try
//            {
//                Stream stream = File.OpenRead("c:\\Windows\\Stec" + GetWinUserCodif() + ".dll");
//                @null = new StreamReader(stream);
//            }
//            catch (Exception ex)
//            {
//                if (!Sistema.sysPrueba)
//                {
//                    cadPuertos.RegistroBitacora2(ex.Source, "GetParameters", "0", ex.HResult, ex.Message);
//                    Sistema.sysError = ex.HResult;
//                    Sistema.sysErrorDescr = ex.Message;
//                    return flag;
//                }

//                Sistema.sysError = ex.HResult;
//                Sistema.sysErrorDescr = ex.Message;
//                Environment.Exit(0);
//                return flag;
//            }

//            text = @null.ReadToEnd();
//            @null.Close();
//            text = Methods.Rot39(text);
//            text2 = text;
//            while (true)
//            {
//                bool flag2 = true;
//                num = text.IndexOf('\t');
//                if (num < 0)
//                {
//                    break;
//                }

//                switch (num2)
//                {
//                    case 0:
//                        Sistema.sysOpCode = text.Substring(0, num);
//                        break;
//                    case 1:
//                        Sistema.sysIP = text.Substring(0, num);
//                        break;
//                    case 2:
//                        Sistema.sysServer = text.Substring(0, num);
//                        break;
//                    case 3:
//                        Sistema.sysCodigoGrupo = text.Substring(0, num);
//                        break;
//                    case 4:
//                        Sistema.sysCodigoEmpresa = text.Substring(0, num);
//                        break;
//                    case 5:
//                        Sistema.sysNombreEmpresa = text.Substring(0, num);
//                        break;
//                    case 6:
//                        Sistema.sysUser = text.Substring(0, num);
//                        break;
//                    case 7:
//                        Sistema.sysAssembliesPath = text.Substring(0, num);
//                        break;
//                    case 8:
//                        Sistema.sysCodigoFuncion = Convert.ToInt16(text.Substring(0, num));
//                        break;
//                    case 9:
//                        Sistema.sysIVA = Convert.ToDouble(text.Substring(0, num));
//                        break;
//                    case 11:
//                        Sistema.sysNombreEnsamblado = text.Substring(0, num);
//                        break;
//                    case 12:
//                        Sistema.sysUsaLocalidad = Convert.ToBoolean(text.Substring(0, num));
//                        break;
//                }

//                text = text.Substring(num + 1);
//                num2++;
//            }

//            Sistema.sysTipoBD = "2012";
//            Sistema.sysDBBase = "STMONITOR";

//            return true;
//        }

//        public string GetWinUserCodif()
//        {
//            string text = "";
//            string text2 = Sistema.sysDBBase;
//            string text3 = "STMonitor";
//            if (!File.Exists("C:\\STMONITOR\\OperNComp.oper"))
//            {
//                return text;
//            }

//            if (text2.Trim() == "")
//            {
//                text2 = Environment.CommandLine;
//                text3 = "Exes";
//            }

//            if (text2.ToUpper().IndexOf(text3.ToUpper()) >= 0)
//            {
//                string text4 = WindowsIdentity.GetCurrent().Name;
//                if (text4.IndexOf("\\") >= 0)
//                {
//                    text4 = text4.Substring(text4.IndexOf("\\") + 1);
//                }

//                for (byte b = 0; b < text4.Length; b = (byte)(b + 1))
//                {
//                    text += Convert.ToChar(Convert.ToChar(text4.Substring(b, 1)) + 1);
//                }
//            }

//            return text;
//        }

//        public bool ExisteError()
//        {
//            bool flag = false;
//            if (Sistema.sysError != 0L || Sistema.sysMensaje != "")
//                flag = true;
//            return flag;
//        }

//        public string MensajeError()
//        {
//            string str = "";
//            if (ComData.SysError == -2146232060L)
//            {
//                switch (ComData.SysErrorNumber)
//                {
//                    case 53:
//                        str = "FNo se pudo conectar a la Base de Datos. Avise a su administrador";
//                        break;
//                    case 208:
//                        str = "FNo se pudo acceder a la tabla solicitada. Avise a su administrador";
//                        break;
//                }
//            }
//            else
//                str = "FHa ocurrido un error. Avise a su administrador";
//            return str;
//        }

//        public bool IsLock(string strNombreTable, string strLlave) => this.cadPuertos.IsLock(strNombreTable, strLlave);

//        public string Lock(string strNombreTable, string strLlave) => this.cadPuertos.Lock(strNombreTable, strLlave);

//        public string UnLock(string strNombreTable, string strLlave) => this.cadPuertos.UnLock(strNombreTable, strLlave);

//        public string SelectCodReceptora(string NombrePuertoCOMM) => this.cadPuertos.SelectCodigoReceptora(NombrePuertoCOMM);

//        public List<string> strReceptora() => this.cadPuertos.strReceptora();

//        public List<string> strPuertoReceptora() => this.cadPuertos.strPuertoReceptora();

//        public string InsertEventos(Evento ccoEvento) => this.cadPuertos.InsertEventos(ccoEvento);

//        public string InsertEventosVideo(EventoVideo ccoEventoVideo) => this.cadPuertos.InsertEventosVideo(ccoEventoVideo);

//        public string SelectNameReceptora(byte CodigoReceptora) => this.cadPuertos.SelectNameReceptora(CodigoReceptora);

//        public List<Parametros> consultaParam() => this.cadPuertos.ConsultaParam();

//        public DateTime GetServerDateTime() => this.cadPuertos.GetServerDateTime();

//        public void ChecaTabla(string strNuevaTabla)
//        {
//            if (this.cadPuertos.ChecaTabla(strNuevaTabla))
//                return;
//            this.cadPuertos.InsertaTabla(strNuevaTabla);
//            this.cadPuertos.CreaIndices(strNuevaTabla);
//        }

//        public string ModificCallerID(Evento ccEvento) => this.cadPuertos.ModificCallerID(ccEvento);

//        public string InsertCallerID(Evento ccEvento) => this.cadPuertos.InsertCallerID(ccEvento);

//        public string ConsultaCallerID(Evento ccEvento) => this.cadPuertos.ConsultaCallerID(ccEvento);

//        public void RegistroBitacora2(
//          string Programa,
//          string Metodo,
//          string Linea,
//          int NoError,
//          string DescripcionError)
//        {
//            this.cadPuertos.RegistroBitacora2(Programa, Metodo, Linea, NoError, DescripcionError);
//        }

//        public string InsertParticion(AbonadoPart ccAbonPart) => this.cadPuertos.InsertParticion(ccAbonPart);

//        public string UpdateParticion(AbonadoPart ccAbonPart) => this.cadPuertos.UpdateParticion(ccAbonPart);

//        public byte BuscaParticion(AbonadoPart ccAbonPart) => this.cadPuertos.BuscaParticion(ccAbonPart);

//        public List<Receptora> ListaReceptora() => this.cadPuertos.ListaReceptora();

//        public List<RecepcionIP> ConsultaRecIP(int Equipo) => this.cadPuertos.ConsultaRecIP(Equipo);

//        public string RegRecepcionIP(RecepcionIP ccIP) => this.cadPuertos.RegRecepcionIP(ccIP);

//        public string ModRecepcionIP(RecepcionIP ccIP) => this.cadPuertos.ModRecepcionIP(ccIP);

//        public List<RecepcionIP> ListaParamIP() => this.cadPuertos.ListaParamIP();

//        public string EliminarParamIP(int CodigoEquipo) => this.cadPuertos.EliminarParamIP(CodigoEquipo);

//        public string ConsultaCodOper() => this.cadPuertos.ConsultaCodOper();

//        public int selectContador(short Id) => this.cadPuertos.selectContador(Id);

//        public bool ExisteEquipo(string strClaveEquipo) => this.cadPuertos.ExisteEquipo(strClaveEquipo);

//        public bool AbonadoUnico(string CodAbonado) => this.cadPuertos.AbonadoUnico(CodAbonado);

//        public void ExisteTabla(string strNuevaTabla)
//        {
//            if (this.cadPuertos.ExisteTabla(strNuevaTabla))
//                return;
//            this.cadPuertos.CreaTabla(strNuevaTabla);
//            this.cadPuertos.CreaIndice(strNuevaTabla);
//        }

//        public string RegEvento(string strNuevaTabla, string strEvento) => this.cadPuertos.RegEvento(strNuevaTabla, strEvento);

//        public List<strTXT> ConsultaLog(string strNombreTabla) => this.cadPuertos.ConsultaLog(strNombreTabla);

//        public string BuscaEvento(RecepEvento ccCRE) => this.cadPuertos.BuscaEvento(ccCRE);

//        public List<EventoZona> ConsultaEventoZona() => this.cadPuertos.ConsultaEventoZona();

//        public string BorrarCallerID(string strCodAbon, byte bytCodRecep) => this.cadPuertos.BorrarCallerID(strCodAbon, bytCodRecep);
//    }

//}

using Comun;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

namespace MPSER
{
    public class _MPCLogica
    {
        public _MPCAccesoADatos cadPuertos = new _MPCAccesoADatos();
        public Logica.Logica cloLogica = new Logica.Logica();
        public AccesoADatos.AccesoADatos cadCAD = new AccesoADatos.AccesoADatos();

        public bool Main()
        {
            Sistema.strLocalAssemblyName = "MPC";
            if (!this.GetParameters())
            {
                if (!this.ExisteError())
                    ;
                return false;
            }
            Sistema.bytCodigoEmpresaReal = Convert.ToByte(Sistema.sysCodigoEmpresa);
            Sistema.strPathSistema = "C:\\Siatec\\SiatecInfo\\";
            Sistema.strPathEmpresa = "C:\\Siatec\\SiatecInfo\\G" + Sistema.sysCodigoGrupo + "\\E" + Sistema.sysCodigoEmpresa + "\\";
            Sistema.strPathGrupo = "C:\\Siatec\\SiatecInfo\\G" + Sistema.sysCodigoGrupo + "\\E0";
            return true;
        }

        public bool GetParameters()
        {
            bool flag = false;
            StreamReader @null = StreamReader.Null;
            string[] array = new string[16];
            int num = 0;
            int num2 = 0;
            string text = null;
            string text2 = null;
            flag = false;
            try
            {
                Stream stream = File.OpenRead("c:\\Windows\\Stec" + GetWinUserCodif() + ".dll");
                @null = new StreamReader(stream);
            }
            catch (Exception ex)
            {
                if (!Sistema.sysPrueba)
                {
                    cadPuertos.RegistroBitacora2(ex.Source, "GetParameters", "0", ex.HResult, ex.Message);
                    Sistema.sysError = ex.HResult;
                    Sistema.sysErrorDescr = ex.Message;
                    return flag;
                }

                Sistema.sysError = ex.HResult;
                Sistema.sysErrorDescr = ex.Message;
                Environment.Exit(0);
                return flag;
            }

            text = @null.ReadToEnd();
            @null.Close();
            text = Methods.Rot39(text);
            text2 = text;
            while (true)
            {
                bool flag2 = true;
                num = text.IndexOf('\t');
                if (num < 0)
                {
                    break;
                }

                switch (num2)
                {
                    case 0:
                        Sistema.sysOpCode = text.Substring(0, num);
                        break;
                    case 1:
                        Sistema.sysIP = text.Substring(0, num);
                        break;
                    case 2:
                        Sistema.sysServer = text.Substring(0, num);
                        break;
                    case 3:
                        Sistema.sysCodigoGrupo = text.Substring(0, num);
                        break;
                    case 4:
                        Sistema.sysCodigoEmpresa = text.Substring(0, num);
                        break;
                    case 5:
                        Sistema.sysNombreEmpresa = text.Substring(0, num);
                        break;
                    case 6:
                        Sistema.sysUser = text.Substring(0, num);
                        break;
                    case 7:
                        Sistema.sysAssembliesPath = text.Substring(0, num);
                        break;
                    case 8:
                        Sistema.sysCodigoFuncion = Convert.ToInt16(text.Substring(0, num));
                        break;
                    case 9:
                        Sistema.sysIVA = Convert.ToDouble(text.Substring(0, num));
                        break;
                    case 11:
                        Sistema.sysNombreEnsamblado = text.Substring(0, num);
                        break;
                    case 12:
                        Sistema.sysUsaLocalidad = Convert.ToBoolean(text.Substring(0, num));
                        break;
                }

                text = text.Substring(num + 1);
                num2++;
            }

            Sistema.sysTipoBD = "2012";
            Sistema.sysDBBase = "STMONITOR";

            return true;
        }

        public string GetWinUserCodif()
        {
            string text = "";
            string text2 = Sistema.sysDBBase;
            string text3 = "STMonitor";
            if (!File.Exists("C:\\STMONITOR\\OperNComp.oper"))
            {
                return text;
            }

            if (text2.Trim() == "")
            {
                text2 = Environment.CommandLine;
                text3 = "Exes";
            }

            if (text2.ToUpper().IndexOf(text3.ToUpper()) >= 0)
            {
                string text4 = WindowsIdentity.GetCurrent().Name;
                if (text4.IndexOf("\\") >= 0)
                {
                    text4 = text4.Substring(text4.IndexOf("\\") + 1);
                }

                for (byte b = 0; b < text4.Length; b = (byte)(b + 1))
                {
                    text += Convert.ToChar(Convert.ToChar(text4.Substring(b, 1)) + 1);
                }
            }

            return text;
        }

        public bool ExisteError()
        {
            bool flag = false;
            if (Sistema.sysError != 0L || Sistema.sysMensaje != "")
                flag = true;
            return flag;
        }

        public bool IsLock(string strNombreTable, string strLlave) => this.cadPuertos.IsLock(strNombreTable, strLlave);

        public string Lock(string strNombreTable, string strLlave) => this.cadPuertos.Lock(strNombreTable, strLlave);

        public string UnLock(string strNombreTable, string strLlave) => this.cadPuertos.UnLock(strNombreTable, strLlave);

        public string SelectCodReceptora(string NombrePuertoCOMM) => this.cadPuertos.SelectCodigoReceptora(NombrePuertoCOMM);

        public List<string> strReceptora() => this.cadPuertos.strReceptora();

        public List<string> strPuertoReceptora() => this.cadPuertos.strPuertoReceptora();

        public string InsertEventos(Evento ccoEvento) => this.cadPuertos.InsertEventos(ccoEvento);

        public string InsertEventosVideo(EventoVideo ccoEventoVideo) => this.cadPuertos.InsertEventosVideo(ccoEventoVideo);

        public string SelectNameReceptora(byte CodigoReceptora) => this.cadPuertos.SelectNameReceptora(CodigoReceptora);

        public List<Parametros> consultaParam() => this.cadPuertos.ConsultaParam();

        public DateTime GetServerDateTime() => this.cadPuertos.GetServerDateTime();

        public void ChecaTabla(string strNuevaTabla)
        {
            if (this.cadPuertos.ChecaTabla(strNuevaTabla))
                return;
            this.cadPuertos.InsertaTabla(strNuevaTabla);
            this.cadPuertos.CreaIndices(strNuevaTabla);
        }

        public string ModificCallerID(Evento ccEvento) => this.cadPuertos.ModificCallerID(ccEvento);

        public string InsertCallerID(Evento ccEvento) => this.cadPuertos.InsertCallerID(ccEvento);

        public string ConsultaCallerID(Evento ccEvento) => this.cadPuertos.ConsultaCallerID(ccEvento);

        public void RegistroBitacora2(
          string Programa,
          string Metodo,
          string Linea,
          int NoError,
          string DescripcionError)
        {
            this.cadPuertos.RegistroBitacora2(Programa, Metodo, Linea, NoError, DescripcionError);
        }

        public string InsertParticion(AbonadoPart ccAbonPart) => this.cadPuertos.InsertParticion(ccAbonPart);

        public string UpdateParticion(AbonadoPart ccAbonPart) => this.cadPuertos.UpdateParticion(ccAbonPart);

        public byte BuscaParticion(AbonadoPart ccAbonPart) => this.cadPuertos.BuscaParticion(ccAbonPart);

        public List<Receptora> ListaReceptora() => this.cadPuertos.ListaReceptora();

        public List<RecepcionIP> ConsultaRecIP(int Equipo) => this.cadPuertos.ConsultaRecIP(Equipo);

        public string RegRecepcionIP(RecepcionIP ccIP) => this.cadPuertos.RegRecepcionIP(ccIP);

        public string ModRecepcionIP(RecepcionIP ccIP) => this.cadPuertos.ModRecepcionIP(ccIP);

        public List<RecepcionIP> ListaParamIP() => this.cadPuertos.ListaParamIP();

        public string EliminarParamIP(int CodigoEquipo) => this.cadPuertos.EliminarParamIP(CodigoEquipo);

        public string ConsultaCodOper() => this.cadPuertos.ConsultaCodOper();

        public int selectContador(short Id) => this.cadPuertos.selectContador(Id);

        public bool ExisteEquipo(string strClaveEquipo) => this.cadPuertos.ExisteEquipo(strClaveEquipo);

        public bool AbonadoUnico(string CodAbonado) => this.cadPuertos.AbonadoUnico(CodAbonado);

        public void ExisteTabla(string strNuevaTabla)
        {
            if (this.cadPuertos.ExisteTabla(strNuevaTabla))  return;
            this.cadPuertos.CreaTabla(strNuevaTabla);
            this.cadPuertos.CreaIndice(strNuevaTabla);
        }

        public string RegEvento(string strNuevaTabla, string strEvento) => this.cadPuertos.RegEvento(strNuevaTabla, strEvento);

        public List<strTXT> ConsultaLog(string strNombreTabla) => this.cadPuertos.ConsultaLog(strNombreTabla);

        public string BuscaEvento(RecepEvento ccCRE) => this.cadPuertos.BuscaEvento(ccCRE);

        public List<EventoZona> ConsultaEventoZona() => this.cadPuertos.ConsultaEventoZona();

        public string BorrarCallerID(string strCodAbon, byte bytCodRecep) => this.cadPuertos.BorrarCallerID(strCodAbon, bytCodRecep);
    }
}
