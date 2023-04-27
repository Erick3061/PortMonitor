using Comun;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace MPSER
{
    public class _MPCAccesoADatos
    {
        private AccesoADatos.AccesoADatos cadAccesoDatos = new AccesoADatos.AccesoADatos();
        private string aviso = "";
        public DataSet ds = new DataSet();

        //public string[] GetParams()
        //{
        //    StreamReader strMaquina = null;
        //    string nameBD = "", server = "";
        //    string NameRuta = @"C:\STMONITOR";
        //    string NameFill = @"C:\STMONITOR\Config.txt";
        //    if (!Directory.Exists(NameRuta))
        //    {
        //        RegistroBitacora2(Sistema.strLocalAssemblyName, "cnStringStec Erick hizo", "30", 000, "La ruta a la cual quiere acceder no se encuentra");
        //    }
        //    else
        //    {
        //        strMaquina = new StreamReader(NameFill);
        //        server = strMaquina.ReadLine();
        //        nameBD = strMaquina.ReadLine();
        //    }
        //    strMaquina.Close();
        //    string[] ret = { server, nameBD };
        //    return ret;
        //}

        //public string strCadena(string strDBName)
        //{
        //    if (strDBName.ToUpper() == "SISTEMA") return this.cnStringStec(strDBName);
        //    return this.cnStringStec(strDBName);
        //}

        //public string cnStringStec(string strDBName)
        //{
        //    string str = Sistema.strPathEmpresa;
        //    string[] pa = this.GetParams();

        //    if (strDBName == "SISTEMA")
        //    {
        //        str = Sistema.strPathSistema;
        //        return string.Format("Data Source={0}; Initial Catalog={1}{2}; MultipleActiveResultSets=True; User ID=sa; Password=siatecMexico2014=)(", (object)pa[0], (object)str, (object)strDBName);
        //    }
        //    return string.Format("Data Source={0}; Initial Catalog={1}; MultipleActiveResultSets=True; User ID=sa; Password=siatecMexico2014=)(", (object)pa[0], (object)pa[1]);
        //}

        public string strCadena(string strDBName)
        {
            if (Sistema.sysTipoBD == "2005")
                return this.cnStringStec(strDBName);
            string str2 = Sistema.strPathEmpresa;
            if (strDBName.ToUpper() == "SISTEMA")
                str2 = Sistema.strPathSistema;
            return string.Format("Data Source={0}; Initial Catalog={1}{2}; MultipleActiveResultSets=True; User ID=sa; Password=siatecMexico2014=)(", (object)Sistema.sysServer, (object)str2, (object)strDBName);
        }

        public string cnStringStec(string strDBName)
        {
            string str = Sistema.strPathEmpresa;
            if (strDBName == "SISTEMA")
                str = Sistema.strPathSistema;
            string format = "Data Source={0}\\SQLEXPRESS;Database={1}{2}.mdf;" + Methods.Rot39("|L>K pkdL:bw:LLPHK=dLB:M><YWW\\d)(b") + "MultipleActiveResultsets=yes";
            if (Sistema.sysNombreEmpresa.IndexOf("CRP DE MEXICO") >= 0)
                format = "Data Source={0}\\SQLEXPRESS;Database={1}{2}.mdf;User ID=sa;Password=siatec2005;MultipleActiveResultsets=yes";
            return string.Format(format, (object)Sistema.sysServer, (object)str, (object)strDBName);
        }

        public void RegistroBitacora2(
          string Programa,
          string Metodo,
          string Linea,
          int NoError,
          string DescripcionError)
        {
            if (!File.Exists("C:\\STMONITOR\\Bitacora.err"))
                File.CreateText("C:\\STMONITOR\\Bitacora.err").Close();
            StreamWriter streamWriter = File.AppendText("C:\\STMONITOR\\Bitacora.err");
            string str1 = string.Format("{0:yyyy/MM/dd}", (object)DateTime.Now);
            string str2 = string.Format("{0:HH:mm:ss}", (object)DateTime.Now);
            string str3 = Programa;
            string str4 = Metodo;
            string str5 = Linea;
            int num = NoError;
            string str6 = DescripcionError;
            streamWriter.WriteLine(str1.ToString() + " " + str2.ToString() + " " + str3.ToString() + " " + str4.ToString() + " " + str5.ToString() + " " + num.ToString() + " " + str6.ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }

        public void UseDS(
          string strSQL,
          ref SqlDataAdapter da,
          ref SqlCommandBuilder cbl,
          ref DataSet ds,
          string strNombreTabla,
          bool siModifica,
          ref int ErrNumero,
          ref string ErrDescr,
          ref string ErrLinea)
        {
            SqlConnection selectConnection = new SqlConnection();
            if (selectConnection.State != ConnectionState.Open)
                selectConnection.ConnectionString = this.strCadena("STMonitor");
            try
            {
                ds.Tables[strNombreTabla].Clear();
            }
            catch
            {
            }
            try
            {
                da = new SqlDataAdapter(strSQL, selectConnection);
                if (siModifica)
                {
                    cbl = new SqlCommandBuilder(da);
                    da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                }
                da.Fill(ds, strNombreTabla);
            }
            catch (SqlException ex)
            {
                ErrNumero = ex.Number;
                ErrDescr = ex.Message;
                ErrLinea = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en "));
            }
            selectConnection.Close();
        }

        public void UseDS1(
          string strSQL,
          ref SqlDataAdapter da,
          ref SqlCommandBuilder cbl,
          DataSet ds,
          string strNombreTabla,
          bool siModifica,
          ref int ErrNumero,
          ref string ErrDescr,
          ref string ErrLinea,
          SqlConnection cone)
        {
            try
            {
                cone.ConnectionString = this.strCadena("STMonitor");
                try
                {
                    //ds.Tables[strNombreTabla].Clear();
                }
                catch
                {
                }
                try
                {
                    da = new SqlDataAdapter(strSQL, cone);
                    if (siModifica)
                    {
                        cbl = new SqlCommandBuilder(da);
                        da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    }
                    da.Fill(ds, strNombreTabla);
                }
                catch (SqlException ex)
                {
                    ErrNumero = ex.Number;
                    ErrDescr = ex.Message;
                    ErrLinea = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en "));
                    this.RegistroBitacora2("MPC", nameof(UseDS1), "0", ex.HResult, ex.Message + "  " + ex.StackTrace + "  " + (object)ex.TargetSite);
                }
                cone.Close();
            }
            catch (Exception ex)
            {
                this.RegistroBitacora2("MPC", nameof(UseDS1), "0", ex.HResult, ex.Message + "  " + ex.StackTrace + "  " + (object)ex.TargetSite);
            }
        }

        public SqlDataReader SelectDR1(
          string DBName,
          string strSQL,
          string[] arrParam,
          CommandBehavior cmdBhvr)
        {
            SqlConnection sqlConnection = new SqlConnection();
            SqlDataReader sqlDataReader = (SqlDataReader)null;
            Sistema.sysError = 0L;
            Sistema.sysErrorDescr = "";
            Sistema.sysErrorDescr = "";
            try
            {
                if (sqlConnection.State != ConnectionState.Open)
                {
                    sqlConnection.ConnectionString = this.cadAccesoDatos.cnString(DBName);
                    sqlConnection.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(strSQL);
                for (int index = 1; index <= Convert.ToInt32(arrParam[0]); ++index)
                {
                    string parameterName = "@P" + index.ToString().PadLeft(2, '0');
                    sqlCommand.Parameters.AddWithValue(parameterName, (object)arrParam[index]);
                }
                sqlCommand.Connection = sqlConnection;
                sqlDataReader = sqlCommand.ExecuteReader(cmdBhvr | CommandBehavior.CloseConnection);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 0)
                {
                    Sistema.sysError = -99999L;
                }
                else
                {
                    Sistema.sysError = (long)ex.Number;
                    Sistema.sysErrorDescr = ex.Message + " " + sqlConnection.ConnectionString;
                }
                if (ex.Number == -1)
                {
                    Sistema.sysError = -1L;
                    Sistema.sysErrorDescr = "Error en la conexión a la Base de datos. Avise a su Administrador";
                    this.RegistroBitacora2(ex.Source, ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en ")), ex.Number, ex.Message + " " + sqlConnection.ConnectionString);
                }
                else
                {
                    Sistema.sysError = (long)ex.Number;
                    Sistema.sysErrorDescr = "Ha ocurrido un error. Avise a su Administrador (" + ex.Message + ")";
                    this.RegistroBitacora2(ex.Source, ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en ")), ex.Number, ex.Message + " " + sqlConnection.ConnectionString);
                }
            }
            catch (Exception ex)
            {
                this.RegistroBitacora2(ex.Source, ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en ")), ex.HResult, ex.Message + " " + sqlConnection.ConnectionString);
                Sistema.sysError = (long)ex.HResult;
                Sistema.sysErrorDescr = "Ha ocurrido un error. Avise a su Administrador (" + ex.Message + ")";
            }
            return sqlDataReader;
        }

        public SqlDataReader SelectDR(
          string strSQL,
          string[] arrParam,
          SqlConnection cone,
          CommandBehavior cmbhv)
        {
            SqlDataReader sqlDataReader = (SqlDataReader)null;
            try
            {
                cone.ConnectionString = this.strCadena("STMonitor");
                Console.WriteLine(".............................................................................");
                Console.WriteLine(this.strCadena("STMonitor"));
                Console.WriteLine(strSQL);
                Console.WriteLine(".............................................................................");
                cone.Open();
                SqlCommand sqlCommand = new SqlCommand(strSQL);
                for (int index = 1; index <= Convert.ToInt32(arrParam[0]); ++index)
                {
                    string parameterName = "@P" + index.ToString().PadLeft(2, '0');
                    sqlCommand.Parameters.AddWithValue(parameterName, (object)arrParam[index]);
                }
                sqlCommand.Connection = cone;
                sqlCommand.CommandTimeout = 100;
                sqlDataReader = sqlCommand.ExecuteReader(cmbhv);
            }
            catch (SqlException ex)
            {
                Sistema.sysError = ex.ErrorCode != 0 ? (long)ex.ErrorCode : -99999L;
                if (ex.ErrorCode != -2146232060)
                    this.RegistroBitacora2("MPC - SelectDR  " + ex.StackTrace, ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en ")), ex.Number, ex.Message + " " + (object)cone.State + " " + strSQL + " " + arrParam[0] + " " + arrParam[1]);
            }
            catch (Exception ex)
            {
                this.RegistroBitacora2("MPC - SelectDR  ", ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en ")), ex.HResult, ex.Message + " " + (object)cone.State + " " + strSQL + " " + arrParam[0] + " " + arrParam[1]);
                Sistema.sysError = (long)ex.HResult;
            }
            return sqlDataReader;
        }

        public bool IsLock(string strNombreTable, string strLlave)
        {
            bool flag = false;
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT * FROM Lock WHERE NombreTabla= '{0}' and Llave='{1}'", (object)strNombreTable, (object)strLlave.ToString()), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError != 0L || !sqlDataReader.HasRows)
                        return flag;
                    sqlDataReader.Read();
                    flag = true;
                }
            }
            return flag;
        }

        public string Lock(string strNombreTable, string strLlave)
        {
            string str = "";
            string strSQL = string.Format("INSERT INTO Lock ({0}) VALUES ({1})", (object)"NombreTabla, Llave", (object)string.Format("'{0}', '{1}'", (object)strNombreTable, (object)strLlave.ToString()));
            int ErrNumber = 0;
            string ErrDescr = "";
            string ErrLinea = "";
            this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
            switch (ErrNumber)
            {
                case 0:
                    return str;
                case 2601:
                    str = "Nombre ya registrado";
                    goto case 0;
                default:
                    str = "Ha ocurrido un error, avise a su administrador";
                    this.RegistroBitacora("MPC", nameof(Lock), ErrLinea, ErrNumber, ErrDescr);
                    goto case 0;
            }
        }

        public string UnLock(string strNombreTable, string strLlave)
        {
            string str = "";
            string strSQL = string.Format("DELETE FROM Lock WHERE NombreTabla='{0}' and Llave='{1}' ", (object)strNombreTable, (object)strLlave.ToString());
            int ErrNumber = 0;
            string ErrDescr = "";
            string ErrLinea = "";
            this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
            switch (ErrNumber)
            {
                case 0:
                    return str;
                case 2601:
                    str = "Nombre ya registrado";
                    goto case 0;
                default:
                    str = "Ha ocurrido un error, avise a su administrador";
                    this.RegistroBitacora("MPC", nameof(UnLock), ErrLinea, ErrNumber, ErrDescr);
                    goto case 0;
            }
        }

        public void RegistroBitacora(
          string Programa,
          string Metodo,
          string Linea,
          int NoError,
          string DescripcionError)
        {
            if (!File.Exists("C:\\STMONITOR\\Bitacora.err"))
                File.CreateText("C:\\STMONITOR\\Bitacora.err").Close();
            StreamWriter streamWriter = File.AppendText("C:\\STMONITOR\\Bitacora.err");
            string str1 = string.Format("{0:yyyy/MM/dd}", (object)DateTime.Now);
            string str2 = string.Format("{0:HH:mm:ss}", (object)DateTime.Now);
            string str3 = Programa;
            string str4 = Metodo;
            string str5 = Linea;
            int num = NoError;
            string str6 = DescripcionError;
            streamWriter.WriteLine(str1.ToString() + " " + str2.ToString() + " " + str3.ToString() + " " + str4.ToString() + " " + str5.ToString() + " " + num.ToString() + " " + str6.ToString());
            streamWriter.Flush();
            streamWriter.Close();
        }

        public void InUpDeSQL(
          string strSQL,
          bool CierraBD,
          ref int ErrNumber,
          ref string ErrDescr,
          ref string ErrLinea)
        {
            this.aviso = "";
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        sqlConnection.ConnectionString = this.strCadena("STMonitor");
                        sqlCommand.Connection = sqlConnection;
                        sqlConnection.Open();
                    }
                    catch (SqlException ex)
                    {
                        ErrNumber = ex.Number;
                        ErrDescr = ex.Message;
                        ErrLinea = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en "));
                    }
                    try
                    {
                        sqlCommand.CommandText = strSQL;
                        int num = sqlCommand.ExecuteNonQuery();
                        if (CierraBD)
                            sqlConnection.Close();
                        if (num != 0)
                            return;
                        this.aviso = "No se actualizo ninguna fila";
                    }
                    catch (SqlException ex)
                    {
                        ErrNumber = ex.Number;
                        ErrDescr = ex.Message;
                        ErrLinea = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en "));
                    }
                }
            }
        }

        public void InUpDeSQL1(
          string strSQL,
          bool CierraBD,
          ref int ErrNumber,
          ref string ErrDescr,
          ref string ErrLinea)
        {
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                this.aviso = "";
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    try
                    {
                        sqlConnection.ConnectionString = this.strCadena("STMonitor");
                        sqlCommand.Connection = sqlConnection;
                        sqlConnection.Open();
                    }
                    catch (SqlException ex)
                    {
                        ErrNumber = ex.Number;
                        ErrDescr = ex.Message;
                        ErrLinea = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en "));
                    }
                    try
                    {
                        sqlCommand.CommandText = strSQL;
                        if (sqlCommand.ExecuteNonQuery() != 0)
                            return;
                        this.aviso = "No se actualizo ninguna fila";
                    }
                    catch (SqlException ex)
                    {
                        ErrNumber = ex.Number;
                        ErrDescr = ex.Message;
                        ErrLinea = ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en "));
                    }
                }
            }
        }

        public string SelectCodigoReceptora(string NombrePuertoCOMM)
        {
            string str = "";
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                string strSQL = "SELECT Receptora.NombreR FROM PuertoSerial, Receptora WHERE " + "PuertoSerial.CodigoReceptora = Receptora.CodigoReceptora and PuertoSerial.NombrePuerto = @P01";
                arrParam[0] = "1";
                arrParam[1] = NombrePuertoCOMM;
                using (SqlDataReader sqlDataReader = this.SelectDR(strSQL, arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError != 0L || !sqlDataReader.HasRows)
                        return str;
                    sqlDataReader.Read();
                    str = sqlDataReader.GetString(0);
                }
            }
            return str;
        }

        public List<string> strReceptora()
        {
            List<string> stringList = new List<string>();
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT * FROM Receptora WHERE EstadoRecep = 'A'", new string[99], cone, CommandBehavior.SingleResult))
                {
                    if (ComData.SysError != 0L)
                        return stringList;
                    if (sqlDataReader != null && sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            string str = sqlDataReader["CodigoReceptora"].ToString().Trim() + "¬" + sqlDataReader["NombreR"].ToString().Trim();
                            stringList.Add(str);
                        }
                    }
                    ComData.blnCierraDB = true;
                }
            }
            return stringList;
        }

        public List<string> strPuertoReceptora()
        {
            List<string> stringList = new List<string>();
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT CodigoPuerto, CodigoReceptora FROM PuertoSerial WHERE TipoPuerto = 1", new string[99], cone, CommandBehavior.SingleResult))
                {
                    if (ComData.SysError != 0L || sqlDataReader == null || !sqlDataReader.HasRows)
                        return stringList;
                    while (sqlDataReader.Read())
                    {
                        string str = sqlDataReader["CodigoPuerto"].ToString().Substring(3).Trim() + "¬" + sqlDataReader["CodigoReceptora"].ToString().Trim();
                        stringList.Add(str);
                    }
                }
            }
            return stringList;
        }

        public string InsertEventos(Evento ccoEvento)
        {
            string str1 = "Evento" + (object)ComData.strAñoServidor;
            string str2 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str3 = str1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1("SELECT * FROM " + str3 + " WHERE FechaHora = '2099-01-01 00:00:00'", ref da, ref cbl, dataSet, str3, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    if (dataSet.Tables[str3].Rows.Count <= 0)
                    {
                        DataRow row = dataSet.Tables[str3].NewRow();
                        row["FechaHora"] = (object)ccoEvento.FechaHora;
                        row["ReceptorNum"] = (object)ccoEvento.ReceptorNum;
                        row["LineaNum"] = ccoEvento.LineaNum == null ? (object)"" : (object)ccoEvento.LineaNum;
                        row["CodigoAbonado"] = (object)ccoEvento.CodigoAbonado;
                        row["CodigoEvento"] = (object)ccoEvento.CodigoEvento;
                        row["CodigoZona"] = ccoEvento.CodigoZona == null ? (object)"" : (object)ccoEvento.CodigoZona;
                        row["UsuarioNum"] = ccoEvento.UsuarioNum == null ? (object)"" : (object)ccoEvento.UsuarioNum;
                        row["Calificador"] = ccoEvento.Calificador == null ? (object)"" : (object)ccoEvento.Calificador;
                        row["GrupoNum"] = ccoEvento.GrupoNum == null ? (object)"" : (object)ccoEvento.GrupoNum;
                        row["CodigoReceptora"] = (object)ccoEvento.CodigoReceptora;
                        row["PuertoCommNum"] = (object)ccoEvento.PuertoCommNum;
                        row["CodigoProtocolo"] = (object)ccoEvento.CodigoProtocolo;
                        row["EventoOriginador"] = (object)ccoEvento.EventoOriginador;
                        row["Estado"] = (object)ccoEvento.Estado;
                        row["ClaveMonitorista"] = (object)"";
                        row["SeProcesaCon"] = (object)DBNull.Value;
                        row["ContEnEspera"] = (object)0;
                        row["CalificacionEvento"] = (object)0;
                        row["CallerID"] = (object)ccoEvento.CallerID;
                        row["CodigoReceptoraReal"] = (object)ccoEvento.CodigoReceptoraReal;
                        row["CodigoAlarma"] = (object)"";
                        row["Particion"] = (object)ccoEvento.Particion;
                        row["NombreUsuario"] = (object)"";
                        row["NombreZona"] = (object)"";
                        try
                        {
                            dataSet.Tables[str3].Rows.Add(row);
                            da.Update(dataSet, str3);
                            dataSet.Tables[str3].Clear();
                        }
                        catch (Exception ex)
                        {
                            Evento ccEvent = ccoEvento;
                            switch (ErrNumero)
                            {
                                case 0:
                                    if (ex.HResult == -2146232060)
                                    {
                                        if (this.ConsultaEvento(ccEvent))
                                        {
                                            str2 = ex.Message;
                                            break;
                                        }
                                        Evento evento = ccoEvento;
                                        DateTime fechaHora = ccoEvento.FechaHora;
                                        DateTime dateTime = fechaHora.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime;
                                        this.InsertEventos(ccoEvento);
                                        fechaHora = ccoEvento.FechaHora;
                                        str2 = "@@$$¿¿" + fechaHora.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                                        break;
                                    }
                                    break;
                                case 2601:
                                    str2 = "Nombre ya registrado";
                                    break;
                                default:
                                    str2 = "Ha ocurrido un error, avise a su administrador";
                                    this.RegistroBitacora2("MPC", nameof(InsertEventos), "0", ex.HResult, ex.Message);
                                    break;
                            }
                        }
                    }
                }
            }
            return str2;
        }

        public string InsertEventosVideo(EventoVideo ccoEventoVideo)
        {
            string str1 = "EventoVideo" + (object)ComData.strAñoServidor;
            string str2 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str3 = str1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1("SELECT * FROM " + str3 + " WHERE FechaHora = '2099-01-01 00:00:00'", ref da, ref cbl, dataSet, str3, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    if (dataSet.Tables[str3].Rows.Count <= 0)
                    {
                        DataRow row = dataSet.Tables[str3].NewRow();
                        row["FechaHora"] = (object)ccoEventoVideo.FechaHora;
                        row["CodigoAbonado"] = (object)ccoEventoVideo.CodigoAbonado;
                        row["CodigoVideo"] = (object)ccoEventoVideo.Video;
                        try
                        {
                            dataSet.Tables[str3].Rows.Add(row);
                            da.Update(dataSet, str3);
                            dataSet.Tables[str3].Clear();
                        }
                        catch (Exception ex)
                        {
                            EventoVideo ccEventVideo = ccoEventoVideo;
                            switch (ErrNumero)
                            {
                                case 0:
                                    if (ex.HResult == -2146232060)
                                    {
                                        if (this.ConsultaEventoVideo(ccEventVideo))
                                        {
                                            str2 = ex.Message;
                                            break;
                                        }
                                        EventoVideo eventoVideo = ccoEventoVideo;
                                        DateTime fechaHora = ccoEventoVideo.FechaHora;
                                        DateTime dateTime = fechaHora.AddMilliseconds(1.0);
                                        eventoVideo.FechaHora = dateTime;
                                        this.InsertEventosVideo(ccoEventoVideo);
                                        fechaHora = ccoEventoVideo.FechaHora;
                                        str2 = "@@$$¿¿" + fechaHora.ToString("yyyy-MM-dd HH:mm:ss.ffff");
                                        break;
                                    }
                                    break;
                                case 2601:
                                    str2 = "Nombre ya registrado";
                                    break;
                                default:
                                    str2 = "Ha ocurrido un error, avise a su administrador";
                                    this.RegistroBitacora2("MPC", "InsertEventos", "0", ex.HResult, ex.Message);
                                    break;
                            }
                        }
                    }
                }
            }
            return str2;
        }

        public bool ConsultaEvento(Evento ccEvent)
        {
            bool flag = false;
            using (SqlConnection cone = new SqlConnection())
            {
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT * FROM {0} WHERE FechaHora = '{1}' AND LineaNum = {2} AND CodigoAbonado = '{3}' " + "AND CodigoEvento = '{4}' AND CodigoZona = '{5}' AND CodigoReceptora = '{6}'", (object)("Evento" + (object)ComData.strAñoServidor), (object)ccEvent.FechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff"), (object)ccEvent.LineaNum, (object)ccEvent.CodigoAbonado, (object)ccEvent.CodigoEvento, (object)ccEvent.CodigoZona, (object)ccEvent.CodigoReceptora), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader.HasRows)
                        flag = true;
                }
            }
            return flag;
        }

        public bool ConsultaEventoVideo(EventoVideo ccEventVideo)
        {
            bool flag = false;
            using (SqlConnection cone = new SqlConnection())
            {
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT * FROM {0} WHERE FechaHora = '{1}' AND CodigoAbonado = '{2}' " + "AND CodigoEvento = '{3}'", (object)("Evento" + (object)ComData.strAñoServidor), (object)ccEventVideo.FechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff"), (object)ccEventVideo.CodigoAbonado, (object)ccEventVideo.Video), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader.HasRows)
                        flag = true;
                }
            }
            return flag;
        }

        public string updateEventos(Evento ccoEvento)
        {
            string str1 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str2 = "Eventos";
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1(string.Format("SELECT * FROM Eventos WHERE FechaHora = '{0}'", (object)ccoEvento.FechaHora), ref da, ref cbl, dataSet, str2, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    DataRow row = dataSet.Tables[str2].Rows[0];
                    row["FechaHora"] = (object)ccoEvento.FechaHora;
                    row["ReceptorNum"] = (object)ccoEvento.ReceptorNum;
                    row["LineaNum"] = (object)ccoEvento.LineaNum;
                    row["CodigoAbonado"] = (object)ccoEvento.CodigoAbonado;
                    row["CodigoEvento"] = (object)ccoEvento.CodigoEvento;
                    row["CodigoZona"] = (object)ccoEvento.CodigoZona;
                    row["UsuarioNum"] = (object)ccoEvento.UsuarioNum;
                    row["Calificador"] = (object)ccoEvento.Calificador;
                    row["GrupoNum"] = (object)ccoEvento.GrupoNum;
                    row["CodigoReceptora"] = (object)ccoEvento.CodigoReceptora;
                    row["PuertoCommNum"] = (object)ccoEvento.PuertoCommNum;
                    row["CodigoProtocolo"] = (object)ccoEvento.CodigoProtocolo;
                    row["EventoOriginador"] = (object)ccoEvento.EventoOriginador;
                    row["Estado"] = (object)ccoEvento.Estado;
                    row["CallerID"] = (object)ccoEvento.CallerID;
                    try
                    {
                        da.Update(dataSet, str2);
                    }
                    catch
                    {
                        switch (ErrNumero)
                        {
                            case 0:
                                break;
                            case 2601:
                                str1 = "Nombre ya registrado";
                                break;
                            default:
                                str1 = "Ha ocurrido un error, avise a su administrador";
                                this.RegistroBitacora("MPC", nameof(updateEventos), ErrLinea, ErrNumero, ErrDescr);
                                break;
                        }
                    }
                }
            }
            return str1;
        }

        public string SelectNameReceptora(byte CodigoReceptora)
        {
            string str = "";
            using (SqlConnection cone = new SqlConnection())
            {
                string[] arrParam = new string[99];
                string strSQL = "SELECT NombreR FROM Receptora WHERE CodigoReceptora = @P01";
                arrParam[0] = "1";
                arrParam[1] = CodigoReceptora.ToString();
                using (SqlDataReader sqlDataReader = this.SelectDR(strSQL, arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError != 0L || sqlDataReader == null || !sqlDataReader.HasRows)
                        return str;
                    sqlDataReader.Read();
                    str = sqlDataReader.GetString(0);
                }
            }
            return str;
        }

        public List<Parametros> ConsultaParam()
        {
            List<Parametros> parametrosList = new List<Parametros>();
            using (SqlConnection cone = new SqlConnection())
            {
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT * FROM Parametros", new string[99], cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError != 0L || sqlDataReader == null || !sqlDataReader.HasRows)
                        return parametrosList;
                    sqlDataReader.Read();
                    parametrosList.Add(new Parametros()
                    {
                        ServerIP = sqlDataReader["ServerIP"].ToString(),
                        ServerPuerto = (int)sqlDataReader["ServerPuerto"],
                        CodigoAbonadoRepetido = (bool)sqlDataReader["CodigoAbonadoRepetido"],
                        CodEventoAReemp = sqlDataReader["CodEventoAReemp"].ToString(),
                        CodZona1 = sqlDataReader["CodZona1"].ToString(),
                        CodEventoReemp1 = sqlDataReader["CodEventoReemp1"].ToString(),
                        CodZona2 = sqlDataReader["CodZona2"].ToString(),
                        CodEventoReemp2 = sqlDataReader["CodEventoReemp2"].ToString(),
                        CodEventoError = sqlDataReader["CodEventoError"].ToString(),
                        CodEventoAReemp2 = sqlDataReader["CodEventoAReemp2"].ToString(),
                        CodZona3 = sqlDataReader["CodZona3"].ToString(),
                        CodEventoReemp3 = sqlDataReader["CodEventoReemp3"].ToString(),
                        CodZona4 = sqlDataReader["CodZona4"].ToString(),
                        CodEventoReemp4 = sqlDataReader["CodEventoReemp4"].ToString()
                    });
                }
            }
            return parametrosList;
        }

        public DateTime GetServerDateTime()
        {
            DateTime serverDateTime = DateTime.MinValue;
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT convert(varchar,SYSDATETIME(),121)", arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        serverDateTime = Convert.ToDateTime(sqlDataReader[0].ToString());
                    }
                }
            }
            Console.WriteLine(serverDateTime);
            return serverDateTime;
        }

        public bool ChecaTabla(string NuevaTabla)
        {
            bool flag = false;
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + NuevaTabla + "'", arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader.HasRows)
                        flag = true;
                }
            }
            return flag;
        }

        public void InsertaTabla(string strNuevaTabla)
        {
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                using (this.SelectDR("SELECT Top 0 * INTO " + strNuevaTabla + " From strEvento", new string[2], cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError == 0L)
                        ;
                }
            }
        }

        public string CreaIndices(string strNuevaTabla)
        {
            string str = "";
            try
            {
                string strSQL = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (FechaHora DESC);" + "CREATE  INDEX IX_Evento ON {0} (CodigoAbonado ASC);" + "CREATE  INDEX IX_Evento1 ON {0} (CodigoAbonado ASC, CodigoReceptora ASC)" + "CREATE  INDEX IX_{0} ON {0} (Estado ASC)" + "CREATE  INDEX IX_{0}_1 ON {0} (CodigoAbonado ASC, CodigoReceptora ASC, Particion ASC)" + "CREATE  INDEX IX_{0}_2 ON {0} (CodigoAbonado ASC, CodigoReceptora ASC, Particion ASC, CodigoAlarma ASC)" + "CREATE  INDEX IX_{0}_3 ON {0} (SeProcesaCon ASC)" + "CREATE  INDEX IX_{0}_4 ON {0} (Estado ASC, RegEnDG ASC)" + "CREATE  INDEX IX_{0}_5 ON {0} (CodigoAbonado ASC, CodigoReceptora ASC,DeSistemaNoMuestra ASC,RegEnDg ASC)" + "CREATE  INDEX IX_{0}_6 ON {0} (CodigoAbonado ASC, CodigoReceptora ASC,DeSistemaNoMuestra ASC,Particion ASC)", (object)strNuevaTabla);
                int ErrNumber = 0;
                string ErrDescr = "";
                string ErrLinea = "";
                this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
                if (this.aviso != "")
                    str = this.aviso;
                switch (ErrNumber)
                {
                    case 0:
                        break;
                    case 2601:
                        str = "Nombre ya registrado";
                        break;
                    default:
                        str = "Ha ocurrido un error, avise a su administrador";
                        this.RegistroBitacora("MPC", nameof(CreaIndices), ErrLinea, ErrNumber, ErrDescr);
                        break;
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string ModificCallerID(Evento ccEvento)
        {
            string str = "";
            try
            {
                string strSQL = string.Format("UPDATE EventoCallerID SET CallerID = '{0}' WHERE CodigoAbonado = '{1}' AND CodigoReceptora = {2}", (object)ccEvento.CallerID, (object)ccEvento.CodigoAbonado, (object)ccEvento.CodigoReceptora);
                int ErrNumber = 0;
                string ErrDescr = "";
                string ErrLinea = "";
                this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
                if (this.aviso != "")
                    str = this.aviso;
                switch (ErrNumber)
                {
                    case 0:
                        break;
                    case 2601:
                        str = "Nombre ya registrado";
                        break;
                    default:
                        str = "Ha ocurrido un error, avise a su administrador";
                        this.RegistroBitacora("MPC", nameof(ModificCallerID), ErrLinea, ErrNumber, ErrDescr);
                        break;
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string InsertCallerID(Evento ccEvento)
        {
            string str = "";
            try
            {
                string strSQL = string.Format("INSERT INTO EventoCallerID (CodigoAbonado, CodigoReceptora, CallerID) VALUES ('{0}', {1}, '{2}')", (object)ccEvento.CodigoAbonado, (object)ccEvento.CodigoReceptora, (object)ccEvento.CallerID);
                int ErrNumber = 0;
                string ErrDescr = "";
                string ErrLinea = "";
                this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
                if (this.aviso != "")
                    str = this.aviso;
                switch (ErrNumber)
                {
                    case 0:
                        break;
                    case 2601:
                        str = "Nombre ya registrado";
                        break;
                    default:
                        str = "Ha ocurrido un error, avise a su administrador";
                        this.RegistroBitacora("MPC", nameof(InsertCallerID), ErrLinea, ErrNumber, ErrDescr);
                        break;
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string ConsultaCallerID(Evento ccEvento)
        {
            string str = "";
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT CallerID FROM EventoCallerID WHERE CodigoAbonado = '{0}' AND CodigoReceptora = '{1}'", (object)ccEvento.CodigoAbonado, (object)ccEvento.CodigoReceptora), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader != null)
                    {
                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();
                            str = sqlDataReader["CallerID"].ToString();
                        }
                    }
                }
            }
            return str;
        }

        public string InsertParticion(AbonadoPart ccAbonPart)
        {
            string str1 = "AbonadoPart";
            string str2 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str3 = str1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1("SELECT * FROM " + str3 + " WHERE CodigoAbonado = '999999' AND CodigoReceptora = 999", ref da, ref cbl, dataSet, str3, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    if (dataSet.Tables[str3].Rows.Count <= 0)
                    {
                        DataRow row = dataSet.Tables[str3].NewRow();
                        row["CodigoAbonado"] = (object)ccAbonPart.CodigoAbonado;
                        row["CodigoReceptora"] = (object)ccAbonPart.CodigoReceptora;
                        row["NumParticion"] = (object)ccAbonPart.NumParticion;
                        try
                        {
                            dataSet.Tables[str3].Rows.Add(row);
                            da.Update(dataSet, str3);
                            dataSet.Tables[str3].Clear();
                        }
                        catch (SqlException ex)
                        {
                            if (ex.HResult == -2146232060 && ex.Number == 2627)
                                str2 = "Llave duplicada";
                        }
                        catch (Exception ex)
                        {
                            switch (ErrNumero)
                            {
                                case 0:
                                    break;
                                case 2601:
                                    str2 = "Particion ya registrada";
                                    break;
                                default:
                                    str2 = "Ha ocurrido un error, avise a su administrador";
                                    break;
                            }
                        }
                    }
                }
            }
            return str2;
        }

        public string UpdateParticion(AbonadoPart ccAbonPart)
        {
            string str1 = "AbonadoPart";
            string str2 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str3 = str1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1(string.Format("SELECT * FROM " + str3 + " WHERE CodigoAbonado = '{0}' AND CodigoReceptora = {1}", (object)ccAbonPart.CodigoAbonado, (object)ccAbonPart.CodigoReceptora), ref da, ref cbl, dataSet, str3, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    DataRow row = dataSet.Tables[str3].Rows[0];
                    row["CodigoAbonado"] = (object)ccAbonPart.CodigoAbonado;
                    row["CodigoReceptora"] = (object)ccAbonPart.CodigoReceptora;
                    row["NumParticion"] = (object)ccAbonPart.NumParticion;
                    try
                    {
                        da.Update(dataSet, str3);
                    }
                    catch (SqlException ex)
                    {
                        if (ex.HResult == -2146232060 && ex.Number == 2627)
                            str2 = "Llave duplicada";
                    }
                    catch (Exception ex)
                    {
                        switch (ErrNumero)
                        {
                            case 0:
                                break;
                            case 2601:
                                str2 = "Particion ya registrada";
                                break;
                            default:
                                str2 = "Ha ocurrido un error, avise a su administrador";
                                break;
                        }
                    }
                }
            }
            return str2;
        }

        public byte BuscaParticion(AbonadoPart ccAbonPart)
        {
            byte num = 0;
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT NumParticion FROM AbonadoPart WHERE CodigoAbonado = '{0}' AND CodigoReceptora = {1}", (object)ccAbonPart.CodigoAbonado, (object)ccAbonPart.CodigoReceptora), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader != null)
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                                num = (byte)sqlDataReader["NumParticion"];
                        }
                    }
                }
            }
            return num;
        }

        public List<Receptora> ListaReceptora()
        {
            List<Receptora> receptoraList = new List<Receptora>();
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                using (SqlDataReader sqlDataReader = this.SelectDR("select * from Receptora ORDER BY NombreR", new string[99], cone, CommandBehavior.SingleResult))
                {
                    if (ComData.SysError != 0L || sqlDataReader == null || !sqlDataReader.HasRows)
                        return receptoraList;
                    while (sqlDataReader.Read())
                        receptoraList.Add(new Receptora()
                        {
                            CodigoReceptora = (byte)sqlDataReader["CodigoReceptora"],
                            NombreR = sqlDataReader["NombreR"].ToString(),
                            EmulaA = (byte)sqlDataReader["EmulaA"]
                        });
                }
            }
            return receptoraList;
        }

        public List<RecepcionIP> ConsultaRecIP(int Equipo)
        {
            List<RecepcionIP> recepcionIpList = new List<RecepcionIP>();
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[2];
                string strSQL = "select * from RecepcionIP WHERE CodigoEquipo = @P01";
                arrParam[0] = "1";
                arrParam[1] = Equipo.ToString();
                using (SqlDataReader sqlDataReader = this.SelectDR(strSQL, arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError != 0L || sqlDataReader == null || !sqlDataReader.HasRows)
                        return recepcionIpList;
                    sqlDataReader.Read();
                    recepcionIpList.Add(new RecepcionIP()
                    {
                        CodigoEquipo = (byte)sqlDataReader["CodigoEquipo"],
                        CodigoReceptora = (byte)sqlDataReader["CodigoReceptora"],
                        IP = sqlDataReader["IP"].ToString(),
                        Puerto = (int)sqlDataReader["Puerto"]
                    });
                }
            }
            return recepcionIpList;
        }

        public string RegRecepcionIP(RecepcionIP ccIP)
        {
            string str1 = "RecepcionIP";
            string str2 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str3 = str1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1("SELECT * FROM " + str3 + " WHERE CodigoEquipo = 99999", ref da, ref cbl, dataSet, str3, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    if (dataSet.Tables[str3].Rows.Count <= 0)
                    {
                        DataRow row = dataSet.Tables[str3].NewRow();
                        row["CodigoEquipo"] = (object)ccIP.CodigoEquipo;
                        row["CodigoReceptora"] = (object)ccIP.CodigoReceptora;
                        row["IP"] = (object)ccIP.IP;
                        row["Puerto"] = (object)ccIP.Puerto;
                        try
                        {
                            dataSet.Tables[str3].Rows.Add(row);
                            da.Update(dataSet, str3);
                            dataSet.Tables[str3].Clear();
                        }
                        catch (Exception ex)
                        {
                            str2 = ex.Message;
                        }
                    }
                }
            }
            return str2;
        }

        public string ModRecepcionIP(RecepcionIP ccIP)
        {
            string str1 = "RecepcionIP";
            string str2 = "";
            using (SqlConnection cone = new SqlConnection())
            {
                using (DataSet dataSet = new DataSet())
                {
                    string str3 = str1;
                    SqlDataAdapter da = new SqlDataAdapter();
                    SqlCommandBuilder cbl = new SqlCommandBuilder();
                    int ErrNumero = 0;
                    string ErrDescr = "";
                    string ErrLinea = "";
                    this.UseDS1(string.Format("SELECT * FROM " + str3 + " WHERE CodigoEquipo = {0}", (object)ccIP.CodigoEquipo), ref da, ref cbl, dataSet, str3, true, ref ErrNumero, ref ErrDescr, ref ErrLinea, cone);
                    DataRow row = dataSet.Tables[str3].Rows[0];
                    row["CodigoEquipo"] = (object)ccIP.CodigoEquipo;
                    row["CodigoReceptora"] = (object)ccIP.CodigoReceptora;
                    row["IP"] = (object)ccIP.IP;
                    row["Puerto"] = (object)ccIP.Puerto;
                    try
                    {
                        da.Update(dataSet, str3);
                    }
                    catch (Exception ex)
                    {
                        str2 = ex.Message;
                    }
                }
            }
            return str2;
        }

        public List<RecepcionIP> ListaParamIP()
        {
            List<RecepcionIP> recepcionIpList = new List<RecepcionIP>();
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                using (SqlDataReader sqlDataReader = this.SelectDR("select * from RecepcionIP", new string[2], cone, CommandBehavior.SingleResult))
                {
                    if (ComData.SysError != 0L || sqlDataReader == null || !sqlDataReader.HasRows)
                        return recepcionIpList;
                    while (sqlDataReader.Read())
                        recepcionIpList.Add(new RecepcionIP()
                        {
                            CodigoEquipo = (byte)sqlDataReader["CodigoEquipo"],
                            CodigoReceptora = (byte)sqlDataReader["CodigoReceptora"],
                            IP = sqlDataReader["IP"].ToString(),
                            Puerto = (int)sqlDataReader["Puerto"]
                        });
                }
            }
            return recepcionIpList;
        }

        public string EliminarParamIP(int CodigoEquipo)
        {
            string str = "";
            try
            {
                string strSQL = string.Format("DELETE FROM RecepcionIP WHERE CodigoEquipo = {0}", (object)CodigoEquipo);
                int ErrNumber = 0;
                string ErrDescr = "";
                string ErrLinea = "";
                this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
                if (this.aviso != "")
                    str = this.aviso;
                switch (ErrNumber)
                {
                    case 0:
                        break;
                    case 2601:
                        str = "Nombre ya registrado";
                        break;
                    default:
                        str = "Ha ocurrido un error, avise a su administrador";
                        this.RegistroBitacora("MPC", nameof(EliminarParamIP), ErrLinea, ErrNumber, ErrDescr);
                        break;
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string ConsultaCodOper()
        {
            string str = "";
            using (SqlDataReader sqlDataReader = this.SelectDR1("SISTEMA", "SELECT CodOperacion FROM Parametro", new string[2], CommandBehavior.SingleResult))
            {
                if (sqlDataReader != null)
                {
                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        str = sqlDataReader["CodOperacion"].ToString();
                    }
                }
            }
            return str;
        }

        public int selectContador(short Id)
        {
            string[] arrParam = new string[2];
            int num = 0;
            string strSQL = "SELECT Contador FROM ContNum WHERE CodigoContNum = @P01";
            arrParam[0] = "1";
            arrParam[1] = Id.ToString();
            using (SqlDataReader sqlDataReader = this.SelectDR1("SISTEMA", strSQL, arrParam, CommandBehavior.SingleRow))
            {
                if (sqlDataReader != null)
                {
                    if (sqlDataReader.HasRows)
                    {
                        sqlDataReader.Read();
                        num = sqlDataReader.GetInt32(0);
                    }
                }
            }
            return num;
        }

        public bool ExisteEquipo(string strClaveEquipo)
        {
            bool flag = false;
            string[] arrParam = new string[2];
            using (SqlDataReader sqlDataReader = this.SelectDR1("SISTEMA", string.Format("SELECT * FROM EquiposAut WHERE ID_PC = '{0}'", (object)strClaveEquipo), arrParam, CommandBehavior.SingleRow))
            {
                if (sqlDataReader != null)
                {
                    if (sqlDataReader.HasRows)
                        flag = true;
                }
            }
            return flag;
        }

        public bool AbonadoUnico(string CodAbonado)
        {
            bool flag = false;
            using (SqlConnection cone = new SqlConnection())
            {
                string[] arrParam = new string[2];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT * FROM AbonadoUnico WHERE CodigoAbonado = '{0}'", (object)CodAbonado), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader != null)
                    {
                        if (sqlDataReader.HasRows)
                            flag = true;
                    }
                }
            }
            return flag;
        }

        public bool ExisteTabla(string strNuevaTabla)
        {
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string[] arrParam = new string[99];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", (object)strNuevaTabla), arrParam, cone, CommandBehavior.SingleRow))
                {
                   return sqlDataReader.HasRows ? true : false;
                }
            }
        }

        public void CreaTabla(string strNuevaTabla)
        {
            using (SqlConnection cone = new SqlConnection())
            {
                ComData.SysError = 0L;
                string strSQL = string.Format("SELECT Top 0 * INTO  {0}  From strTXT", (object)strNuevaTabla);
                string[] arrParam = new string[99];
                List<string> stringList = new List<string>();
                using (this.SelectDR(strSQL, arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (ComData.SysError == 0L)
                        ;
                }
            }
        }

        public string CreaIndice(string strNuevaTabla)
        {
            string str = "";
            try
            {
                string strSQL = string.Format("ALTER TABLE {0} ADD PRIMARY KEY (Id ASC);", (object)strNuevaTabla);
                int ErrNumber = 0;
                string ErrDescr = "";
                string ErrLinea = "";
                this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
                if (this.aviso != "")
                    str = this.aviso;
                switch (ErrNumber)
                {
                    case 0:
                        break;
                    case 2601:
                        str = "Nombre ya registrado";
                        break;
                    default:
                        str = "Ha ocurrido un error, avise a su administrador";
                        this.RegistroBitacora("MPC", nameof(CreaIndice), ErrLinea, ErrNumber, ErrDescr);
                        break;
                }
            }
            catch (Exception ex)
            {
                str = ex.Message;
            }
            return str;
        }

        public string RegEvento(string strNuevaTabla, string strEvento)
        {
            string str = "";
            string strSQL = string.Format("INSERT INTO {0} (Evento) VALUES ('{1}')", (object)strNuevaTabla, (object)strEvento);
            int ErrNumber = 0;
            string ErrDescr = "";
            string ErrLinea = "";
            this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
            switch (ErrNumber)
            {
                case 0:
                    return str;
                case 208:
                    str = "No existe la tabla";
                    goto case 0;
                case 2601:
                    str = "Nombre ya registrado";
                    goto case 0;
                default:
                    str = "Ha ocurrido un error, avise a su administrador";
                    this.RegistroBitacora("MPC", nameof(RegEvento), ErrLinea, ErrNumber, ErrDescr);
                    goto case 0;
            }
        }

        public List<strTXT> ConsultaLog(string strNombreTabla)
        {
            List<strTXT> strTxtList = new List<strTXT>();
            using (SqlConnection cone = new SqlConnection())
            {
                string[] arrParam = new string[2];
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT * FROM " + strNombreTabla, arrParam, cone, CommandBehavior.SingleResult))
                {
                    if (sqlDataReader != null)
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                                strTxtList.Add(new strTXT()
                                {
                                    Id = (int)sqlDataReader["Id"],
                                    Evento = sqlDataReader["Evento"].ToString()
                                });
                        }
                    }
                }
            }
            return strTxtList;
        }

        public string BuscaEvento(RecepEvento ccCRE)
        {
            string str = "";
            using (SqlConnection cone = new SqlConnection())
            {
                string[] arrParam = new string[2];
                using (SqlDataReader sqlDataReader = this.SelectDR(string.Format("SELECT CodigoEvento FROM RecepEvento WHERE CodigoReceptora = {0} AND CodigoProtocolo = {1} AND EdoCanal = {2} AND Valor = '{3}'", (object)ccCRE.CodigoReceptora, (object)ccCRE.CodigoProtocolo, (object)ccCRE.EdoCanal, (object)ccCRE.Valor), arrParam, cone, CommandBehavior.SingleRow))
                {
                    if (sqlDataReader != null)
                    {
                        if (sqlDataReader.HasRows)
                        {
                            sqlDataReader.Read();
                            str = sqlDataReader["CodigoEvento"].ToString();
                        }
                    }
                }
            }
            return str;
        }

        public List<EventoZona> ConsultaEventoZona()
        {
            List<EventoZona> eventoZonaList = new List<EventoZona>();
            using (SqlConnection cone = new SqlConnection())
            {
                using (SqlDataReader sqlDataReader = this.SelectDR("SELECT EventoZona.*, EmulaA, NombreR, DescripcionEvent FROM EventoZona LEFT JOIN Receptora ON EventoZona.CodigoReceptora = Receptora.CodigoReceptora LEFT JOIN EventosCat ON EventoZona.CodigoEvento = EventosCat.CodigoEvento", new string[2], cone, CommandBehavior.SingleResult))
                {
                    if (!this.cadAccesoDatos.ExisteError())
                    {
                        if (sqlDataReader != null)
                        {
                            while (sqlDataReader.Read())
                                eventoZonaList.Add(new EventoZona()
                                {
                                    CodigoReceptora = (byte)sqlDataReader["CodigoReceptora"],
                                    NombreR = sqlDataReader["NombreR"].ToString(),
                                    CodigoProtocolo = sqlDataReader["CodigoProtocolo"].ToString(),
                                    CodigoEvento = sqlDataReader["CodigoEvento"].ToString(),
                                    DescripcionEvent = sqlDataReader["DescripcionEvent"].ToString(),
                                    CodigoZona = sqlDataReader["CodigoZona"].ToString(),
                                    EmulaA = (byte)sqlDataReader["EmulaA"]
                                });
                        }
                    }
                }
            }
            return eventoZonaList;
        }

        public string BorrarCallerID(string strCodAbon, byte bytCodRecep)
        {
            string str = "";
            string strSQL = string.Format("DELETE FROM EventoCallerID WHERE CodigoAbonado ='{0}' AND CodigoReceptora = {1}", (object)strCodAbon, (object)bytCodRecep);
            int ErrNumber = 0;
            string ErrDescr = "";
            string ErrLinea = "";
            this.InUpDeSQL1(strSQL, ComData.blnCierraDB, ref ErrNumber, ref ErrDescr, ref ErrLinea);
            switch (ErrNumber)
            {
                case 0:
                    return str;
                case 2601:
                    str = "Nombre ya registrado";
                    goto case 0;
                default:
                    str = "Ha ocurrido un error, avise a su administrador";
                    this.RegistroBitacora("MPC", nameof(BorrarCallerID), ErrLinea, ErrNumber, ErrDescr);
                    goto case 0;
            }
        }

    }

}
