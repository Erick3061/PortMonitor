using Comun;
using CustomControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace MPSER
{
    partial class MPTCP
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private bool blDesconectar1 = false;
        private bool blDesconectar2 = false;
        private bool blDesconectar3 = false;
        private bool blDesconectar4 = false;
        private bool blDesconectar5 = false;
        private bool blDesconectar6 = false;
        private bool blDesconectar7 = false;
        private bool blDesconectar8 = false;
        public _MPCLogica cadPuertos = new _MPCLogica();
        //public _MPCAccesoADatos cadPuertos2 = new _MPCAccesoADatos();
        public TcpClients tcpCli = new TcpClients();
        private TcpListener _listenerDT42;
        private TcpListener _listenerServer1;
        private TcpClient _tcpClient1;
        private TcpListener _listenerServer2;
        private TcpListener _listenerServer3;
        private TcpListener _listenerServer4;
        private TcpListener _listenerServer5;
        private TcpListener _listenerServer6;
        private TcpListener _listenerServer7;
        private TcpListener _listenerServer8;
        public Parametros ccParam = new Parametros();
        public int Emula1 = 0;
        public int Emula2 = 0;
        public int Emula3 = 0;
        public int Emula4 = 0;
        public int Emula5 = 0;
        public int Emula6 = 0;
        public int Emula7 = 0;
        public int Emula8 = 0;
        public int Recep1 = 0;
        public int Recep2 = 0;
        public int Recep3 = 0;
        public int Recep4 = 0;
        public int Recep5 = 0;
        public int Recep6 = 0;
        public int Recep7 = 0;
        public int Recep8 = 0;
        public List<Receptora> ListRec = new List<Receptora>();
        public List<EventoZona> ltEZ = new List<EventoZona>();
        private bool CodigoAbonadoRepetido = false;
        private byte longCodAbonadoSIA = 4;
        public int numEquipos = 0;
        public string strNuEq = "";
        public bool blError = false;
        private Button btnSalir;
        private ImageList imageList1;
        private Button btnBorrar;
        private ToolTip toolTip1;
        private ToolTip toolTip2;
        private ToolTip toolTip3;
        private ToolTip toolTip4;
        private ToolTip toolTip5;
        private ToolTip toolTip6;
        private Button btnSendAck;
        private ToolTip toolTip7;
        private ToolTip toolTip8;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool VerificaEquipo()
        {
            bool flag1 = true;
            string str1 = this.cadPuertos.ConsultaCodOper();
            bool flag2;
            if (str1.Length == 0)
                return flag2 = false;
            string str2 = Methods.Rot39(str1);
            if (this.ObtieneDigVer(str2.Substring(0, str2.Length - 1).Substring(1)).ToString() != str2.Substring(str2.Length - 1, 1))
                return flag2 = false;
            this.numEquipos = this.Asc(str2.Substring(0, 1)) - 64;
            int num1 = 0;
            int num2 = this.cadPuertos.selectContador((short)12);
            string str3 = num2.ToString();
            string str4 = "";
            if (str3.Length <= 3)
            {
                num2 = this.ObtieneDigVer(str3.Substring(0, 2));
                if (num2.ToString() != str3.Substring(str3.Length - 1, 1))
                    return flag2 = false;
                str3 = Convert.ToChar(Convert.ToInt32(str3.Substring(0, 2))).ToString();
                num1 = Convert.ToInt32(str3);
            }
            if (str3.Length > 3 && str3.Length <= 5)
            {
                num2 = this.ObtieneDigVer(str3.Substring(0, 4));
                if (num2.ToString() != str3.Substring(str3.Length - 1, 1))
                    return flag2 = false;
                string str5 = str3.Substring(0, 4);
                int startIndex = 0;
                for (int index = 0; index < 2; ++index)
                {
                    int int32 = Convert.ToInt32(str5.Substring(startIndex, 2));
                    str4 += Convert.ToChar(int32).ToString();
                    num1 = Convert.ToInt32(str4);
                    startIndex = 2;
                }
            }
            if (this.numEquipos != num1)
                return flag2 = false;
            this.strNuEq = Convert.ToChar(num1 + 64).ToString();
            return this.ObtenerCodOper() != str2 ? (flag2 = false) : flag1;
        }

        public string ObtenerCodOper()
        {
            string str1 = "";
            ManagementObject managementObject1 = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}\"", (object)new DirectoryInfo(Environment.CurrentDirectory).Root.Name.Replace("\\", "")));
            managementObject1.Get();
            managementObject1["VolumeSerialNumber"].ToString();
            string str2 = managementObject1.GetPropertyValue("VolumeSerialNumber").ToString();
            ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            List<string> stringList = new List<string>();
            foreach (ManagementObject managementObject2 in instances)
            {
                if ((bool)(managementObject2["IPEnabled"] ?? (object)false))
                    stringList.Add(managementObject2["MacAddress"] as string);
                managementObject2.Dispose();
            }
            foreach (string str3 in stringList)
                str1 = str3;
            string str4 = "";
            string dato = str2.PadLeft(12, '0') + str1;
            str4 = dato;
            string strClave = this.DesCifraInfo(dato);
            int num = this.ObtieneDigVer(strClave);
            return this.strNuEq + strClave + (object)num;
        }

        public string ObtenerClave()
        {
            string str1 = "";
            ManagementObject managementObject1 = new ManagementObject(string.Format("win32_logicaldisk.deviceid=\"{0}\"", (object)new DirectoryInfo(Environment.CurrentDirectory).Root.Name.Replace("\\", "")));
            managementObject1.Get();
            managementObject1["VolumeSerialNumber"].ToString();
            string str2 = managementObject1.GetPropertyValue("VolumeSerialNumber").ToString();
            ManagementObjectCollection instances = new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances();
            List<string> stringList = new List<string>();
            foreach (ManagementObject managementObject2 in instances)
            {
                if ((bool)(managementObject2["IPEnabled"] ?? (object)false))
                    stringList.Add(managementObject2["MacAddress"] as string);
                managementObject2.Dispose();
            }
            foreach (string str3 in stringList)
                str1 = str3;
            string str4 = "";
            string dato = str2.PadLeft(12, '0') + str1;
            string str5 = dato;
            int num = this.ObtieneDigVer(this.DesCifraInfo(dato));
            return str4 = str5 + (object)num;
        }

        public int ObtieneDigVer(string strClave)
        {
            Regex regex = new Regex("a-zA-Z+");
            int num = 0;
            int result = 0;
            char[] charArray1 = strClave.ToCharArray();
            for (int index = 0; index < charArray1.Length; ++index)
            {
                if (int.TryParse(charArray1[index].ToString(), out result))
                    num += Convert.ToInt32(charArray1[index].ToString()) * (index + 1);
            }
            bool flag = false;
            while (!flag)
            {
                if (num.ToString().Length == 1)
                {
                    flag = true;
                }
                else
                {
                    char[] charArray2 = num.ToString().ToCharArray();
                    num = 0;
                    for (int index = 0; index < charArray2.Length; ++index)
                    {
                        if (int.TryParse(charArray1[index].ToString(), out result))
                            num += Convert.ToInt32(charArray2[index].ToString());
                    }
                }
            }
            return num;
        }

        private string DesCifraInfo(string dato)
        {
            string str1 = "";
            string str2 = "";
            for (int startIndex = 0; dato.Length > startIndex; ++startIndex)
            {
                if (dato.Substring(startIndex, 1) == ":")
                    dato = dato.Remove(startIndex, 1);
            }
            for (int index = dato.Length - 1; index >= 0; --index)
                str2 += (string)(object)dato[index];
            char ch;
            for (int index = 0; index < dato.Length; index += 2)
            {
                if (index + 1 < dato.Length)
                {
                    string str3 = str1;
                    ch = str2[index + 1];
                    string str4 = ch.ToString();
                    ch = str2[index];
                    string str5 = ch.ToString();
                    str1 = str3 + str4 + str5;
                }
                else
                {
                    string str6 = str1;
                    ch = str2[dato.Length - 1];
                    string str7 = ch.ToString();
                    str1 = str6 + str7;
                }
            }
            return str1;
        }

        private int Asc(string s) => (int)Encoding.ASCII.GetBytes(s)[0];


        private void MPTCP_Load(object sender, EventArgs e)
        {
            this.cadPuertos.RegistroBitacora2("MPSER", "Abierto ", "0", 1, "Usuario : " + Sistema.sysUser);
            Receptora receptora1 = new Receptora();
            ComData.strAñoServidor = this.cadPuertos.GetServerDateTime().Year;
            string str = "";
            List<Receptora> receptoraList = this.cadPuertos.ListaReceptora();
            if (receptoraList.Count != 0)
            {
                foreach (Receptora receptora2 in receptoraList)
                {
                    if (str == "")
                        str = receptora2.CodigoReceptora.ToString() + "¬" + receptora2.NombreR;
                    else
                        str = str + "," + (object)receptora2.CodigoReceptora + "¬" + receptora2.NombreR;
                    this.ListRec.Add(receptora2);
                }
                this.cmbReceptora1.LoadWithList(str);
                this.cmbReceptora2.LoadWithList(str);
                this.cmbReceptora3.LoadWithList(str);
                this.cmbReceptora4.LoadWithList(str);
                this.cmbReceptora5.LoadWithList(str);
                this.cmbReceptora6.LoadWithList(str);
                this.cmbReceptora7.LoadWithList(str);
                this.cmbReceptora8.LoadWithList(str);
                ((Control)this.cmbReceptora1).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora2).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora3).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora4).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora5).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora6).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora7).Text = this.cmbReceptora1.GetNameFromCode("0");
                ((Control)this.cmbReceptora8).Text = this.cmbReceptora1.GetNameFromCode("0");
            }
            else
            {
                int num = (int)MessageBox.Show("No tiene registrado al menos una receptora");
                this.Close();
            }

            List<Parametros> parametrosList = this.cadPuertos.consultaParam();
            if (parametrosList.Count != 0)
            {
                this.ccParam = parametrosList[0];
            }
            else
            {
                int num = (int)MessageBox.Show("No tiene registrado parametros");
                this.Close();
            }
            this.CodigoAbonadoRepetido = this.ccParam.CodigoAbonadoRepetido;
            List<RecepcionIP> recepcionIpList = this.cadPuertos.ListaParamIP();
            if (recepcionIpList.Count != 0)
            {
                GroupBox groupBox = new GroupBox();
                ComboBoxNew comboBoxNew = new ComboBoxNew();
                textBoxText textBoxText = new textBoxText();
                textBoxNum textBoxNum = new textBoxNum();
                for (int index = 1; index < 9; ++index) // 9 NUMERO DE SERVIDORES
                {
                    int serv = index + 8;
                    RecepcionIP exist = recepcionIpList.Find(data => data.CodigoEquipo == serv);
                    if (exist != null)
                    {
                        GroupBox control1 = (GroupBox)this.groupBox1.Controls["gb" + (object)index];
                        ComboBoxNew control2 = (ComboBoxNew)control1.Controls["cmbReceptora" + (object)index];
                        textBoxText control3 = (textBoxText)control1.Controls["txtIP" + (object)index];
                        textBoxNum control4 = (textBoxNum)control1.Controls["txtPuerto" + (object)index];
                        ((Control)control2).Text = control2.GetNameFromCode(exist.CodigoReceptora.ToString());
                        ((Control)control3).Text = exist.IP;
                        ((Control)control4).Text = exist.Puerto.ToString();
                    }
                }
            }
            this.Conectar(this.btnConectar1, this.ckbTCP1, this.cmbReceptora1, this.txtIP1, this.txtPuerto1, this.blDesconectar1, 9);
            this.Conectar(this.btnConectar2, this.ckbTCP2, this.cmbReceptora2, this.txtIP2, this.txtPuerto2, this.blDesconectar2, 10);
            this.Conectar(this.btnConectar3, this.ckbTCP3, this.cmbReceptora3, this.txtIP3, this.txtPuerto3, this.blDesconectar3, 11);
            this.Conectar(this.btnConectar4, this.ckbTCP4, this.cmbReceptora4, this.txtIP4, this.txtPuerto4, this.blDesconectar4, 12);
            this.Conectar(this.btnConectar5, this.ckbTCP5, this.cmbReceptora5, this.txtIP5, this.txtPuerto5, this.blDesconectar5, 13);
            this.Conectar(this.btnConectar6, this.ckbTCP6, this.cmbReceptora6, this.txtIP6, this.txtPuerto6, this.blDesconectar6, 14);
            this.Conectar(this.btnConectar7, this.ckbTCP7, this.cmbReceptora7, this.txtIP7, this.txtPuerto7, this.blDesconectar7, 15);
            this.Conectar(this.btnConectar8, this.ckbTCP8, this.cmbReceptora8, this.txtIP8, this.txtPuerto8, this.blDesconectar8, 16);
            this.ltEZ = this.cadPuertos.ConsultaEventoZona();
        }

        public void Conectar(Button btn, checkBoxNew ckb, ComboBoxNew cmbReceptora, textBoxText ip, textBoxNum puerto, bool desconectar, int numServer)
        {
            RecepcionIPSERVER server = new RecepcionIPSERVER();
            if (cmbReceptora.FindInCombo() == "" || cmbReceptora.GetCodeFromName(((Control)cmbReceptora).Text) == "0" || string.IsNullOrEmpty(((Control)ip).Text)) return;
            if (!this.ValidaIP(((Control)ip).Text))
            {
                ((Control)ip).Focus();
            }
            else
            {
                if (((Control)puerto).Text.Trim() == "") return;
                for (int index = 0; index < this.ListRec.Count; ++index)
                {
                    if (this.ListRec[index].NombreR == ((Control)cmbReceptora).Text)
                    {
                        SetER(numServer, (int)this.ListRec[index].EmulaA, (int)this.ListRec[index].CodigoReceptora);
                        break;
                    }
                }
                desconectar = false;
                ((Control)cmbReceptora).Enabled = false;
                ((Control)ip).Enabled = false;
                ((Control)puerto).Enabled = false;
                server.ip = ((Control)ip).Text;
                server.Puerto = Convert.ToInt32(((Control)puerto).Text);
                btn.Text = "Desconectar";
                btn.BackColor = Color.Green;
                ((CheckBox)ckb).Checked = true;
                this.StarServer(server, numServer);
            }
        }

        public void SetER(int numServer, int emula, int recept)
        {
            switch (numServer)
            {
                case 9:
                    this.Emula1 = emula;
                    this.Recep1 = recept;
                    break;
                case 10:
                    this.Emula2 = emula;
                    this.Recep2 = recept;
                    break;
                case 11:
                    this.Emula3 = emula;
                    this.Recep3 = recept;
                    break;
                case 12:
                    this.Emula4 = emula;
                    this.Recep4 = recept;
                    break;
                case 13:
                    this.Emula5 = emula;
                    this.Recep5 = recept;
                    break;
                case 14:
                    this.Emula6 = emula;
                    this.Recep6 = recept;
                    break;
                case 15:
                    this.Emula7 = emula;
                    this.Recep7 = recept;
                    break;

                    break;
            }
        }
        
        public void Desconectar(Button btn, checkBoxNew ckb, ComboBoxNew cmbReceptora, textBoxText ip, textBoxNum puerto, bool desconectar, ToolTip tooltip, int numServer)
        {
            desconectar = true;
            btn.Text = "Conectar";
            btn.BackColor = Color.Red;
            ((Control)cmbReceptora).Enabled = true;
            ((Control)ip).Enabled = true;
            ((Control)puerto).Enabled = true;
            tooltip.SetToolTip((Control)btn, "");
            ((CheckBox)ckb).Checked = false;
            this.DoBeginConnect(numServer);
        }

        public void CrearServidor(Button btn, checkBoxNew ckb, ComboBoxNew cmbReceptora, textBoxText ip, textBoxNum puerto, bool desconectar, int numServer)
        {
            int emula = 0, recept=0;
            if (cmbReceptora.FindInCombo() == "" || cmbReceptora.GetCodeFromName(((Control)cmbReceptora).Text) == "0")
            {
                int num = (int)MessageBox.Show(String.Format("{0} {1}", "Falta seleccionar la Receptora", numServer.ToString()), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ((Control)cmbReceptora).Focus();
            }
            else if (!string.IsNullOrEmpty(((Control)ip).Text))
            {
                if (!this.ValidaIP(((Control)ip).Text)) ((Control)ip).Focus();
                else if (((Control)puerto).Text.Trim() == "")
                {
                    int num = (int)MessageBox.Show(String.Format("{0} {1}", "Necesita ingresar un Puerto del servidor", numServer.ToString()), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    ((Control)puerto).Focus();
                }
                else
                {
                    for (int index = 0; index < this.ListRec.Count; ++index)
                    {
                        if (this.ListRec[index].NombreR == ((Control)cmbReceptora).Text)
                        {
                            emula = (int)this.ListRec[index].EmulaA;
                            recept = (int)this.ListRec[index].CodigoReceptora;
                            Console.WriteLine(String.Format("{0} {1} {2}", this.ListRec[index].NombreR, emula, recept));
                            SetER(numServer, emula,recept);
                            break;
                        }
                    }
                    List<RecepcionIP> registered = this.cadPuertos.ListaParamIP();

                    RecepcionIP exist = registered.Find(data => (data.CodigoReceptora == recept && data.CodigoEquipo != numServer));
                    if (exist == null) //registrar receptora
                    {
                        exist = registered.Find(data => (data.IP.Trim() == ((Control)ip).Text.Trim() && data.Puerto.ToString().Trim() == ((Control)puerto).Text.Trim()) && data.CodigoEquipo != numServer);
                        if (exist == null)//aceptar puertos
                        {
                            RecepcionIP ccIP = new RecepcionIP();
                            ccIP.CodigoEquipo = (byte)numServer;
                            ccIP.CodigoReceptora = (byte)recept;
                            ccIP.IP = ((Control)ip).Text.Trim();
                            ccIP.Puerto = Convert.ToInt32(((Control)puerto).Text.Trim());
                            if (this.cadPuertos.ConsultaRecIP(numServer).Count == 0)
                            {
                                string text = this.cadPuertos.RegRecepcionIP(ccIP);
                                if (text != "")
                                {
                                    int num = (int)MessageBox.Show(text, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    ((Control)cmbReceptora).Focus();
                                    return;
                                }
                            }
                            else
                            {
                                string text = this.cadPuertos.ModRecepcionIP(ccIP);
                                if (text != "")
                                {
                                    int num = (int)MessageBox.Show(text, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    ((Control)cmbReceptora).Focus();
                                    return;
                                }
                            }
                            this.Conectar(btn, ckb, cmbReceptora, ip, puerto, desconectar, numServer);
                        }
                        else
                        {
                            int num = (int)MessageBox.Show(
                                String.Format("{0} {1}", "La dirección IP y el Puerto ya estan siendo utilizados. por el Equipo", exist.CodigoEquipo)
                                , "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            ((Control)cmbReceptora).Focus();
                        }
                    }
                    else
                    {
                        int num = (int)MessageBox.Show(
                            String.Format("{0} {1}", "El nombre de receptora que usted selecciono ya esta siendo utilizado por el Equipo", exist.CodigoEquipo)
                            , "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        ((Control)cmbReceptora).Focus();
                    }
                }
            }
            else
            {
                int num = (int)MessageBox.Show(String.Format("{0} {1}", "Necesita ingresar una dirección IP", numServer.ToString()), "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                ((Control)ip).Focus();
            }
        }

        public void DoBeginConnect(int servidor)
        {
            switch (servidor)
            {
                case 9:
                    if (this._tcpClient1 == null)
                    {
                        if (this._listenerServer1 == null) return;
                        this._listenerServer1.Stop();
                    }
                    else
                    {
                        this._tcpClient1.Close();
                        this._tcpClient1.Dispose();
                        if (this._listenerServer1 == null) return;
                        this._listenerServer1.Stop();
                    }
                    return;
                case 10:
                    if (this._listenerServer2 == null) return;
                    this._listenerServer2.Stop();
                    break;
                case 11:
                    if (this._listenerServer3 == null) return;
                    this._listenerServer3.Stop();
                    break;
                case 12:
                    if (this._listenerServer4 == null) return;
                    this._listenerServer4.Stop();
                    break;
                case 13:
                    if (this._listenerServer5 == null) return;
                    this._listenerServer5.Stop();
                    break;
                case 14:
                    if (this._listenerServer6 == null) return;
                    this._listenerServer6.Stop();
                    break;
                case 15:
                    if (this._listenerServer7 == null) return;
                    this._listenerServer7.Stop();
                    break;
                case 16:
                    if (this._listenerServer8 == null) return;
                    this._listenerServer8.Stop();
                    break;
            }
        }

        public void AddEvent(
        Evento ccoEvento,
        string evento,
        CheckedListBox ckblb,
        bool blnValidaEvento)
        {
            if (!(evento.Trim() != ""))
                return;
            if (blnValidaEvento)
            {
                if (evento.IndexOf("           @    ") < 0)
                {
                    evento = evento.Substring(0, 19) + " " + evento.Substring(25);
                    if (ckblb.Items.Count >= 2100)
                    {
                        for (int count = ckblb.Items.Count; count >= 2000; --count)
                            ckblb.Items.RemoveAt(count - 1);
                    }
                    ckblb.Items.Insert(0, (object)evento);
                }
            }
            else if (ccoEvento.CodigoProtocolo != (byte)83 && ccoEvento.EventoOriginador.Length != 20)
            {
                evento = evento.Substring(0, 19) + " " + evento.Substring(25);
                if (ckblb.Items.Count >= 2100)
                {
                    for (int count = ckblb.Items.Count; count >= 2000; --count)
                        ckblb.Items.RemoveAt(count - 1);
                }
                ckblb.Items.Insert(0, (object)evento);
                ckblb.SetItemCheckState(0, CheckState.Checked);
            }
        }

        private void btnConectar1_Click(object sender, EventArgs e)
        {
            if (this.btnConectar1.Text == "Conectar")
                this.CrearServidor(this.btnConectar1, this.ckbTCP1, this.cmbReceptora1, this.txtIP1, this.txtPuerto1, this.blDesconectar1, 9);
            else
                this.Desconectar(this.btnConectar1, this.ckbTCP1, this.cmbReceptora1, this.txtIP1, this.txtPuerto1, this.blDesconectar1, this.toolTip1, 9);
        }

        private void btnConectar2_Click(object sender, EventArgs e)
        {
            if (this.btnConectar2.Text == "Conectar")
                this.CrearServidor(this.btnConectar2, this.ckbTCP2, this.cmbReceptora2, this.txtIP2, this.txtPuerto2, this.blDesconectar2, 10);
            else
                this.Desconectar(this.btnConectar2, this.ckbTCP2, this.cmbReceptora2, this.txtIP2, this.txtPuerto2, this.blDesconectar2, this.toolTip2, 10);
        }

        private void btnConectar3_Click(object sender, EventArgs e)
        {
            if (this.btnConectar3.Text == "Conectar")
                this.CrearServidor(this.btnConectar3, this.ckbTCP3, this.cmbReceptora3, this.txtIP3, this.txtPuerto3, this.blDesconectar3, 11);
            else
                this.Desconectar(this.btnConectar3, this.ckbTCP3, this.cmbReceptora3, this.txtIP3, this.txtPuerto3, this.blDesconectar3, this.toolTip3, 11);
        }

        private void btnConectar4_Click(object sender, EventArgs e)
        {
            if (this.btnConectar4.Text == "Conectar")
                this.CrearServidor(this.btnConectar4, this.ckbTCP4, this.cmbReceptora4, this.txtIP4, this.txtPuerto4, this.blDesconectar4, 12);
            else
                this.Desconectar(this.btnConectar4, this.ckbTCP4, this.cmbReceptora4, this.txtIP4, this.txtPuerto4, this.blDesconectar4, this.toolTip4, 12);
        }

        private void btnConectar5_Click(object sender, EventArgs e)
        {
            if (this.btnConectar5.Text == "Conectar")
                this.CrearServidor(this.btnConectar5, this.ckbTCP5, this.cmbReceptora5, this.txtIP5, this.txtPuerto5, this.blDesconectar5, 13);
            else
                this.Desconectar(this.btnConectar5, this.ckbTCP5, this.cmbReceptora5, this.txtIP5, this.txtPuerto5, this.blDesconectar5, this.toolTip5, 13);
        }

        private void btnConectar6_Click(object sender, EventArgs e)
        {
            if (this.btnConectar6.Text == "Conectar")
                this.CrearServidor(this.btnConectar6, this.ckbTCP6, this.cmbReceptora6, this.txtIP6, this.txtPuerto6, this.blDesconectar6, 14);
            else
                this.Desconectar(this.btnConectar6, this.ckbTCP6, this.cmbReceptora6, this.txtIP6, this.txtPuerto6, this.blDesconectar6, this.toolTip6, 14);
        }

        private void btnConectar7_Click(object sender, EventArgs e)
        {
            if (this.btnConectar7.Text == "Conectar")
                this.CrearServidor(this.btnConectar7, this.ckbTCP7, this.cmbReceptora7, this.txtIP7, this.txtPuerto7, this.blDesconectar7, 15);
            else
                this.Desconectar(this.btnConectar7, this.ckbTCP7, this.cmbReceptora7, this.txtIP7, this.txtPuerto7, this.blDesconectar7, this.toolTip7, 15);
        }

        private void btnConectar8_Click(object sender, EventArgs e)
        {
            if (this.btnConectar8.Text == "Conectar")
                this.CrearServidor(this.btnConectar8, this.ckbTCP8, this.cmbReceptora8, this.txtIP8, this.txtPuerto8, this.blDesconectar8, 16);
            else
                this.Desconectar(this.btnConectar8, this.ckbTCP8, this.cmbReceptora8, this.txtIP8, this.txtPuerto8, this.blDesconectar8, this.toolTip8, 16);
        }

        private void cmbReceptora1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora1.GetCodeFromName(((Control)this.cmbReceptora1).Text) != "0")
                this.gb1.Text = ((Control)this.cmbReceptora1).Text;
            else
                this.gb1.Text = "";
        }

        private void cmbReceptora2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora2.GetCodeFromName(((Control)this.cmbReceptora2).Text) != "0")
                this.gb2.Text = ((Control)this.cmbReceptora2).Text;
            else
                this.gb2.Text = "";
        }

        private void cmbReceptora3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora3.GetCodeFromName(((Control)this.cmbReceptora3).Text) != "0")
                this.gb3.Text = ((Control)this.cmbReceptora3).Text;
            else
                this.gb3.Text = "";
        }

        private void cmbReceptora4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora4.GetCodeFromName(((Control)this.cmbReceptora4).Text) != "0")
                this.gb4.Text = ((Control)this.cmbReceptora4).Text;
            else
                this.gb4.Text = "";
        }

        private void cmbReceptora5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora5.GetCodeFromName(((Control)this.cmbReceptora5).Text) != "0")
                this.gb5.Text = ((Control)this.cmbReceptora5).Text;
            else
                this.gb5.Text = "";
        }

        private void cmbReceptora6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora6.GetCodeFromName(((Control)this.cmbReceptora6).Text) != "0")
                this.gb6.Text = ((Control)this.cmbReceptora6).Text;
            else
                this.gb6.Text = "";
        }

        private void cmbReceptora8_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora8.GetCodeFromName(((Control)this.cmbReceptora8).Text) != "0")
                this.gb8.Text = ((Control)this.cmbReceptora8).Text;
            else
                this.gb8.Text = "";
        }

        private void cmbReceptora7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbReceptora7.GetCodeFromName(((Control)this.cmbReceptora7).Text) != "0")
                this.gb7.Text = ((Control)this.cmbReceptora7).Text;
            else
                this.gb7.Text = "";
        }

        private void ckbTCP1_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP1).Checked)
            {
                this.checkedListBox1.Visible = true;
            }
            else
            {
                this.checkedListBox1.Visible = false;
            }
        }

        private void ckbTCP2_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP2).Checked)
            {
                this.checkedListBox2.Visible = true;
            }
            else
            {
                this.checkedListBox2.Visible = false;
            }
        }

        private void ckbTCP3_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP3).Checked)
            {
                this.checkedListBox3.Visible = true;
            }
            else
            {
                this.checkedListBox3.Visible = false;
            }
        }

        private void ckbTCP4_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP4).Checked)
            {
                this.checkedListBox4.Visible = true;
            }
            else
            {
                this.checkedListBox4.Visible = false;
            }
        }

        private void ckbTCP5_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP5).Checked)
            {
                this.checkedListBox5.Visible = true;
            }
            else
            {
                this.checkedListBox5.Visible = false;
            }
        }

        private void ckbTCP6_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP6).Checked)
            {
                this.checkedListBox6.Visible = true;
            }
            else
            {
                this.checkedListBox6.Visible = false;
            }
        }

        private void ckbTCP7_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP7).Checked)
            {
                this.checkedListBox7.Visible = true;
            }
            else
            {
                this.checkedListBox7.Visible = false;
            }
        }

        private void ckbTCP8_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)this.ckbTCP8).Checked)
            {
                this.checkedListBox8.Visible = true;
            }
            else
            {
                this.checkedListBox8.Visible = false;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e) => this.Close();

        private bool ValidaIP(string strFindin)
        {
            bool flag;
            if (new Regex("^(([01]?\\d\\d?|2[0-4]\\d|25[0-5])\\.){3}([01]?\\d\\d?|25[0-5]|2[0-4]\\d)$").IsMatch(strFindin))
            {
                flag = true;
            }
            else
            {
                int num = (int)MessageBox.Show("Por favor, introduzca una dirección IP válida", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                flag = false;
            }
            return flag;
        }

        private void GuardaDatos(string strDate, string IP, string Puerto)
        {
            try
            {
                DateTime serverDateTime = this.cadPuertos.GetServerDateTime();
                for (int index = 0; index < IP.Length; ++index)
                {
                    if (IP.Substring(index, 1) == ".")
                        IP = IP.Substring(0, index) + "_" + IP.Substring(index + 1);
                }
                string strNuevaTabla = "X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)serverDateTime, (object)IP, (object)Puerto);
                string str = this.cadPuertos.RegEvento(strNuevaTabla, strDate);
                if (!(str != "") || !(str == "No existe la tabla"))
                    return;
                this.cadPuertos.ExisteTabla(strNuevaTabla);
                this.cadPuertos.RegEvento(strNuevaTabla, strDate);
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2(nameof(MPTCP), "GuardaDatos2", "0", ex.HResult, ex.Message);
            }
        }

        private void GuardaDatos2(string strDate)
        {
            try
            {
                this.cadPuertos.GetServerDateTime();
                string str = "RevisaCadenasPorIP";
                if (!System.IO.File.Exists("C:\\STMONITOR\\Txtfiles\\" + str + ".txt"))
                {
                    Directory.CreateDirectory("C:\\STMONITOR\\Txtfiles");
                    System.IO.File.CreateText("C:\\STMONITOR\\Txtfiles\\" + str + ".txt").Close();
                }
                StreamWriter streamWriter = System.IO.File.AppendText("C:\\STMONITOR\\Txtfiles\\" + str + ".txt");
                streamWriter.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " " + strDate.ToString());
                streamWriter.Flush();
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2(nameof(MPTCP), nameof(GuardaDatos2), "0", ex.HResult, ex.Message);
            }
        }

        private Evento ValidaEventoYGuardaBD(
          string Evento,
          int EmulaAReceptora,
          int CodigoReceptora,
          string PuertoCOMM)
        {
            Evento evento = new Evento();
            Console.WriteLine(String.Format("Evento: {0} Emula: {1} CodR: {2} pc: {3}", Evento, EmulaAReceptora, CodigoReceptora, PuertoCOMM));
            switch (EmulaAReceptora)
            {
                case 1:
                    evento = this.ProceSurGard(Evento, PuertoCOMM, CodigoReceptora);
                    break;
                case 6:
                    evento = this.ProceSYSTEMIII(Evento, PuertoCOMM, CodigoReceptora);
                    break;
                case 7:
                    evento = this.ProceBosch(Evento, PuertoCOMM, CodigoReceptora);
                    break;
                case 10:
                    evento = this.ProceRisco(Evento, PuertoCOMM, CodigoReceptora);
                    break;
            }
            Console.WriteLine(evento.FechaHora);
            return evento;
        }

        public static double Val(string value)
        {
            try
            {
                string empty = string.Empty;
                foreach (char c in value)
                {
                    if (char.IsNumber(c) || c.Equals('.') && empty.Count<char>((Func<char, bool>)(x => x.Equals('.'))) == 0)
                        empty += String.Format("{0}", c);
                    else if (!c.Equals(' '))
                        return string.IsNullOrEmpty(empty) ? 0.0 : Convert.ToDouble(empty);
                }
                return string.IsNullOrEmpty(empty) ? 0.0 : Convert.ToDouble(empty);
            }
            catch (Exception er)
            {
                Console.WriteLine("Valor: "+value+" .."+er.Message);
                return (0.0);
            }
        }

        private Evento ProceSurGard(string Evento, string PuertoCOMM, int CodigoReceptora)
        {
            string str1 = "";
            Evento ccoEvento = new Evento();
            bool flag1 = false;
            bool flag2 = true;
            try
            {
                ccoEvento.LimpiaProps();
                str1 = Evento.Substring(0, 24);
                ccoEvento.Estado = "A";
                ccoEvento.FechaHora = DateTime.Parse(Evento.Substring(0, 25));
                ccoEvento.EventoOriginador = Evento.Substring(25);
                string str2 = ccoEvento.EventoOriginador.Substring(0, 1);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                string str3;
                switch (str2)
                {
                    case "1":
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1).Trim();
                        if (ccoEvento.EventoOriginador.IndexOf("           @    ") >= 0)
                        {
                            flag1 = true;
                            break;
                        }
                        if (ccoEvento.EventoOriginador.Length != 20)
                        {
                            flag1 = false;
                            break;
                        }
                        if (ccoEvento.EventoOriginador.Substring(10, 4) != "0000")
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                            if (ccoEvento.EventoOriginador.Substring(16, 1) != " ")
                                ccoEvento.Particion = Convert.ToByte(ccoEvento.EventoOriginador.Substring(16, 1));
                            ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(17, 3).Trim();
                            for (int startIndex = 0; startIndex < ccoEvento.CodigoZona.Length; ++startIndex)
                            {
                                if (!char.IsNumber(Convert.ToChar(ccoEvento.CodigoZona.Substring(startIndex, 1))))
                                {
                                    flag2 = false;
                                    break;
                                }
                            }
                            if (flag2)
                                ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(18, 2)).ToString().Trim();
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            string str4 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str4 != "")
                            {
                                if (str4.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str4.Substring(6);
                                else
                                    break;
                            }
                            flag1 = true;
                            break;
                        }
                        switch (ccoEvento.CodigoEvento)
                        {
                            case "P":
                                ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                if (this.CodigoAbonadoRepetido)
                                    ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                string str5 = this.cadPuertos.InsertEventos(ccoEvento);
                                if (str5 != "")
                                {
                                    if (str5.Substring(0, 6) == "@@$$¿¿")
                                        str1 = str5.Substring(6);
                                    else
                                        break;
                                }
                                flag1 = true;
                                break;
                            case "A":
                            case "R":
                            case "T":
                                ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                                ccoEvento.CodigoEvento = MPTCP.Val(ccoEvento.EventoOriginador.Substring(18, 2)).ToString();
                                ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                if (this.CodigoAbonadoRepetido)
                                    ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                string str6 = this.cadPuertos.InsertEventos(ccoEvento);
                                if (str6 != "")
                                {
                                    if (str6.Substring(0, 6) == "@@$$¿¿")
                                        str1 = str6.Substring(6);
                                    else
                                        break;
                                }
                                flag1 = true;
                                break;
                            case "O":
                            case "C":
                                ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).PadLeft(6, ' ');
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                ccoEvento.GrupoNum = ccoEvento.EventoOriginador.Substring(16, 1);
                                ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(17, 3);
                                ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                if (this.CodigoAbonadoRepetido)
                                    ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                string str7 = this.cadPuertos.InsertEventos(ccoEvento);
                                if (str7 != "")
                                {
                                    if (str7.Substring(0, 6) == "@@$$¿¿")
                                        str1 = str7.Substring(6);
                                    else
                                        break;
                                }
                                flag1 = true;
                                break;
                            default:
                                ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).PadLeft(6, ' ');
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(17, 3).PadLeft(4, ' ');
                                ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                if (this.CodigoAbonadoRepetido)
                                    ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                string str8 = this.cadPuertos.InsertEventos(ccoEvento);
                                if (str8 != "")
                                {
                                    if (str8.Substring(0, 6) == "@@$$¿¿")
                                        str1 = str8.Substring(6);
                                    else
                                        break;
                                }
                                flag1 = true;
                                break;
                        }
                        break;
                    case "3":
                        if (ccoEvento.EventoOriginador.Length == 20)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).Trim().PadLeft(6, ' ');
                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(14, 2);
                            ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(16, 4)).ToString().Trim();
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                            {
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                            }
                            AbonadoPart ccAbonPart = new AbonadoPart();
                            ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                            ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                            if (ccoEvento.CodigoEvento == "ri")
                            {
                                ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                str3 = "";
                                if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                    this.cadPuertos.UpdateParticion(ccAbonPart);
                                flag1 = true;
                                break;
                            }
                            byte num = this.cadPuertos.BuscaParticion(ccAbonPart);
                            ccoEvento.Particion = num;
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)1 && p.CodigoProtocolo == "3" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list.Count != 0)
                                ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                            string str9 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str9 != "")
                            {
                                if (str9.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str9.Substring(6);
                                else
                                    break;
                            }
                            flag1 = true;
                            break;
                        }
                        flag1 = false;
                        break;
                    case "S":
                    case "s":
                        bool flag3 = true;
                        bool flag4 = true;
                        if (str2 == "s" && ccoEvento.EventoOriginador.Length == 35)
                        {
                            for (int startIndex = 14; startIndex < 35; ++startIndex)
                            {
                                if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(startIndex, 1))))
                                {
                                    flag4 = false;
                                    break;
                                }
                            }
                            flag1 = flag4;
                            break;
                        }
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(83);
                        DateTime fechaHora;
                        for (int length1 = 0; length1 <= 6; ++length1)
                        {
                            int startIndex1 = 6 + length1;
                            if (ccoEvento.EventoOriginador.Substring(startIndex1, 1) == "|")
                            {
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(6, length1).PadLeft(6, ' ');
                                if (ccoEvento.CodigoAbonado.Trim().Length > (int)this.longCodAbonadoSIA)
                                    ccoEvento.CodigoAbonado = ccoEvento.CodigoAbonado.Substring(ccoEvento.CodigoAbonado.Length - (int)this.longCodAbonadoSIA, 4).PadLeft(6, ' ');
                                int startIndex2 = startIndex1 + 2;
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex2, 2);
                                int startIndex3 = startIndex2 + 2;
                                if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                {
                                    flag3 = false;
                                    for (int index = startIndex3; index < ccoEvento.EventoOriginador.Length; ++index)
                                    {
                                        if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))))
                                        {
                                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index) + "/" + ccoEvento.EventoOriginador.Substring(index);
                                            break;
                                        }
                                    }
                                }
                                for (int length2 = 0; length2 < 4; ++length2)
                                {
                                    if (ccoEvento.EventoOriginador.Substring(startIndex3, 1) == "/")
                                    {
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex3 - length2, length2)).ToString().Trim();
                                        ccoEvento.FechaHora = ccoEvento.FechaHora.AddMilliseconds(1.0);
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str3 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                        }
                                        else
                                        {
                                            byte num = this.cadPuertos.BuscaParticion(ccAbonPart);
                                            ccoEvento.Particion = num;
                                        }
                                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                                        {
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                                        }
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)1 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                        if (list.Count != 0)
                                            ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                                        string str10 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str10 != "")
                                        {
                                            if (str10.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str10.Substring(6);
                                            else
                                                break;
                                        }
                                        str1 = ccoEvento.FechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                        flag1 = true;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex3 + 1, 2);
                                        startIndex3 = startIndex3 + 2 + 1;
                                        length2 = 0;
                                    }
                                    if (ccoEvento.EventoOriginador.Substring(startIndex3, 1) == "]")
                                    {
                                        Evento evento = ccoEvento;
                                        fechaHora = ccoEvento.FechaHora;
                                        DateTime dateTime = fechaHora.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime;
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex3 - length2, length2)).ToString().Trim();
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str3 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                            break;
                                        }
                                        byte num = this.cadPuertos.BuscaParticion(ccAbonPart);
                                        ccoEvento.Particion = num;
                                        break;
                                    }
                                    ++startIndex3;
                                }
                                break;
                            }
                        }
                        if (!flag3)
                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Replace("/", "");
                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                        {
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                        }
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        List<EventoZona> list1 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)1 && p.CodigoProtocolo == "3" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                        if (list1.Count != 0)
                            ccoEvento.CodigoEvento = list1[0].CodigoEvento.Trim() + list1[0].CodigoZona.Trim();
                        string str11 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str11 != "")
                        {
                            if (str11.Substring(0, 6) == "@@$$¿¿")
                                str1 = str11.Substring(6);
                            else
                                break;
                        }
                        fechaHora = ccoEvento.FechaHora;
                        str1 = fechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                        flag1 = true;
                        break;
                    case "5":
                        if (ccoEvento.EventoOriginador.Length == 20)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(ccoEvento.EventoOriginador.Substring(5, 2));
                            ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(7, 4).PadLeft(6, ' ');
                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(11, 4);
                            ccoEvento.Particion = (byte)MPTCP.Val(ccoEvento.EventoOriginador.Substring(15, 2));
                            ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(17, 3)).ToString().Trim();
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list2 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)1 && p.CodigoProtocolo == "5" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list2.Count != 0)
                                ccoEvento.CodigoEvento = list2[0].CodigoEvento.Trim() + list2[0].CodigoZona.Trim();
                            string str12 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str12 != "")
                            {
                                if (str12.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str12.Substring(6);
                                else
                                    break;
                            }
                            flag1 = true;
                            break;
                        }
                        flag1 = false;
                        break;
                    default:
                        flag1 = false;
                        break;
                }
                if (!flag1)
                    ccoEvento.FechaHora = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2("MPC", "Puerto " + (object)ccoEvento.PuertoCommNum, "0", ex.HResult, ex.Message + " " + ex.StackTrace + " " + (object)ex.TargetSite);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                ccoEvento.EventoOriginador = "";
                ccoEvento.ReceptorNum = "";
                ccoEvento.LineaNum = "0";
                ccoEvento.CodigoAbonado = "  0000";
                ccoEvento.CodigoEvento = this.ccParam.CodEventoError;
                ccoEvento.CodigoZona = "0";
                ccoEvento.PuertoCommNum = (byte)0;
                ccoEvento.CodigoProtocolo = (byte)0;
                string s = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff }", (object)this.cadPuertos.GetServerDateTime());
                ccoEvento.FechaHora = DateTime.Parse(s);
                this.cadPuertos.InsertEventos(ccoEvento);
                ccoEvento.FechaHora = DateTime.MinValue;
            }
            return ccoEvento;
        }

        private Evento ProceSYSTEMIII(string Evento, string PuertoCOMM, int CodigoReceptora)
        {
            string str1 = "";
            Evento ccoEvento = new Evento();
            bool flag1 = false;
            bool flag2 = true;
            try
            {
                ccoEvento.LimpiaProps();
                str1 = Evento.Substring(0, 24);
                ccoEvento.Estado = "A";
                ccoEvento.FechaHora = DateTime.Parse(Evento.Substring(0, 25));
                ccoEvento.EventoOriginador = Evento.Substring(25);
                string str2 = ccoEvento.EventoOriginador.Substring(0, 1);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                string str3;

                Console.WriteLine("*********************************************************");
                Console.WriteLine("Estado "+ccoEvento.Estado);
                Console.WriteLine("FechaHora "+ccoEvento.FechaHora);
                Console.WriteLine("EventoOriginador " + ccoEvento.EventoOriginador);
                Console.WriteLine("str1 " + str1);
                Console.WriteLine("str2 " + str2);
                Console.WriteLine("CodigoReceptoraReal " + ccoEvento.CodigoReceptoraReal);
                Console.WriteLine("CodigoReceptora " + ccoEvento.CodigoReceptora);
                Console.WriteLine("*********************************************************");
                switch (str2)
                {
                    case "1":
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1).Trim();
                        if (ccoEvento.EventoOriginador.IndexOf("           @    ") >= 0)
                        {
                            flag1 = true;
                            break;
                        }
                        if (ccoEvento.EventoOriginador.Length == 20)
                        {
                            if (ccoEvento.EventoOriginador.Substring(10, 4) != "0000")
                            {
                                ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                if (ccoEvento.EventoOriginador.Substring(16, 1) != " ")
                                    ccoEvento.Particion = Convert.ToByte(ccoEvento.EventoOriginador.Substring(16, 1));
                                ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(17, 3).Trim();
                                for (int startIndex = 0; startIndex < ccoEvento.CodigoZona.Length; ++startIndex)
                                {
                                    if (!char.IsNumber(Convert.ToChar(ccoEvento.CodigoZona.Substring(startIndex, 1))))
                                    {
                                        flag2 = false;
                                        break;
                                    }
                                }
                                if (flag2)
                                    ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(18, 2)).ToString().Trim();
                                ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                if (this.CodigoAbonadoRepetido)
                                    ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                string str4 = this.cadPuertos.InsertEventos(ccoEvento);
                                if (str4 != "")
                                {
                                    if (str4.Substring(0, 6) == "@@$$¿¿")
                                        str1 = str4.Substring(6);
                                    else
                                        break;
                                }
                                flag1 = true;
                                break;
                            }
                            switch (ccoEvento.CodigoEvento)
                            {
                                case "P":
                                    ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                    ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                    ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                                    ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                    ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                    ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                    if (this.CodigoAbonadoRepetido)
                                        ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                    string str5 = this.cadPuertos.InsertEventos(ccoEvento);
                                    if (str5 != "")
                                    {
                                        if (str5.Substring(0, 6) == "@@$$¿¿")
                                            str1 = str5.Substring(6);
                                        else
                                            break;
                                    }
                                    flag1 = true;
                                    break;
                                case "A":
                                case "R":
                                case "T":
                                    ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                    ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                    ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                                    ccoEvento.CodigoEvento = MPTCP.Val(ccoEvento.EventoOriginador.Substring(18, 2)).ToString();
                                    ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                    ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                    ccoEvento.EventoOriginador += "123";
                                    if (this.CodigoAbonadoRepetido)
                                        ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                    string str6 = this.cadPuertos.InsertEventos(ccoEvento);
                                    if (str6 != "")
                                    {
                                        if (str6.Substring(0, 6) == "@@$$¿¿")
                                            str1 = str6.Substring(6);
                                        else
                                            break;
                                    }
                                    flag1 = true;
                                    break;
                                case "O":
                                case "C":
                                    ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                    ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                    ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).PadLeft(6, ' ');
                                    ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                    ccoEvento.GrupoNum = ccoEvento.EventoOriginador.Substring(16, 1);
                                    ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(17, 3);
                                    ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                    ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                    if (this.CodigoAbonadoRepetido)
                                        ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                    string str7 = this.cadPuertos.InsertEventos(ccoEvento);
                                    if (str7 != "")
                                    {
                                        if (str7.Substring(0, 6) == "@@$$¿¿")
                                            str1 = str7.Substring(6);
                                        else
                                            break;
                                    }
                                    flag1 = true;
                                    break;
                                default:
                                    ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                                    ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                                    ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).PadLeft(6, ' ');
                                    ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1);
                                    ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(17, 3).PadLeft(4, ' ');
                                    ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                    ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                    if (this.CodigoAbonadoRepetido)
                                        ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                    string str8 = this.cadPuertos.InsertEventos(ccoEvento);
                                    if (str8 != "")
                                    {
                                        if (str8.Substring(0, 6) == "@@$$¿¿")
                                            str1 = str8.Substring(6);
                                        else
                                            break;
                                    }
                                    flag1 = true;
                                    break;
                            }
                        }
                        else
                        {
                            if (ccoEvento.EventoOriginador.Length == 22)
                            {
                                if (ccoEvento.EventoOriginador.Substring(12, 4) != "0000")
                                {
                                    ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 3).PadLeft(4, ' ');
                                    ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(5, 1);
                                    ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(12, 4).PadLeft(6, ' ');
                                    ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(17, 1);
                                    if (ccoEvento.EventoOriginador.Substring(16, 1) != " ")
                                        ccoEvento.Particion = Convert.ToByte(ccoEvento.EventoOriginador.Substring(18, 1));
                                    ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(19, 3).Trim();
                                    for (int startIndex = 0; startIndex < ccoEvento.CodigoZona.Length; ++startIndex)
                                    {
                                        if (!char.IsNumber(Convert.ToChar(ccoEvento.CodigoZona.Substring(startIndex, 1))))
                                        {
                                            flag2 = false;
                                            break;
                                        }
                                    }
                                    if (flag2)
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(20, 2)).ToString().Trim();
                                    ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                    ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                    ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                    if (this.CodigoAbonadoRepetido)
                                        ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                    string str9 = this.cadPuertos.InsertEventos(ccoEvento);
                                    if (str9 != "")
                                    {
                                        if (str9.Substring(0, 6) == "@@$$¿¿")
                                            str1 = str9.Substring(6);
                                        else
                                            break;
                                    }
                                    flag1 = true;
                                    break;
                                }
                                switch (ccoEvento.CodigoEvento)
                                {
                                    case "P":
                                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 3).PadLeft(4, ' ');
                                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(5, 1);
                                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(12, 4).PadLeft(6, ' ');
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(17, 1);
                                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                        ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        string str10 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str10 != "")
                                        {
                                            if (str10.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str10.Substring(6);
                                            else
                                                break;
                                        }
                                        flag1 = true;
                                        break;
                                    case "A":
                                    case "R":
                                    case "T":
                                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 3).PadLeft(4, ' ');
                                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(5, 1);
                                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(12, 4).PadLeft(6, ' ');
                                        ccoEvento.CodigoEvento = MPTCP.Val(ccoEvento.EventoOriginador.Substring(20, 2)).ToString();
                                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                        ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                        ccoEvento.EventoOriginador += "123";
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        string str11 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str11 != "")
                                        {
                                            if (str11.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str11.Substring(6);
                                            else
                                                break;
                                        }
                                        flag1 = true;
                                        break;
                                    case "O":
                                    case "C":
                                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 3).PadLeft(4, ' ');
                                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(5, 1);
                                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 6).PadLeft(6, ' ');
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(17, 1);
                                        ccoEvento.GrupoNum = ccoEvento.EventoOriginador.Substring(18, 1);
                                        ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(19, 3);
                                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                        ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        string str12 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str12 != "")
                                        {
                                            if (str12.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str12.Substring(6);
                                            else
                                                break;
                                        }
                                        flag1 = true;
                                        break;
                                    default:
                                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 3).PadLeft(4, ' ');
                                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(5, 1);
                                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 6).PadLeft(6, ' ');
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(17, 1);
                                        ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(19, 3).PadLeft(4, ' ');
                                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                        ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        string str13 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str13 != "")
                                        {
                                            if (str13.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str13.Substring(6);
                                            else
                                                break;
                                        }
                                        flag1 = true;
                                        break;
                                }
                            }
                            else
                                flag1 = false;
                            break;
                        }
                        break;
                    case "3":
                        if (ccoEvento.EventoOriginador.Length == 20)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).Trim().PadLeft(6, ' ');
                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(14, 2);
                            ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(16, 4)).ToString().Trim();
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(str2);
                            ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                            {
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                            }
                            AbonadoPart ccAbonPart = new AbonadoPart();
                            ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                            ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                            if (ccoEvento.CodigoEvento == "ri")
                            {
                                ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                str3 = "";
                                if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                    this.cadPuertos.UpdateParticion(ccAbonPart);
                                flag1 = true;
                                break;
                            }
                            byte num = this.cadPuertos.BuscaParticion(ccAbonPart);
                            ccoEvento.Particion = num;
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "3" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list.Count != 0)
                                ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                            string str14 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str14 != "")
                            {
                                if (str14.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str14.Substring(6);
                                else
                                    break;
                            }
                            flag1 = true;
                            break;
                        }
                        flag1 = false;
                        break;
                    case "4":
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(ccoEvento.EventoOriginador.IndexOf("  ") + 2, 4).PadLeft(6, ' ');
                        ccoEvento.CallerID = ccoEvento.EventoOriginador.Substring(ccoEvento.EventoOriginador.IndexOf("  ") + 6);
                        if (this.cadPuertos.ModificCallerID(ccoEvento) != "")
                            this.cadPuertos.InsertCallerID(ccoEvento);
                        flag1 = true;
                        break;
                    case "S":
                    case "0":
                    case "s":
                        bool flag3 = true;
                        bool flag4 = true;
                        if (str2 == "s" && ccoEvento.EventoOriginador.Length == 35)
                        {
                            for (int startIndex = 16; startIndex < 35; ++startIndex)
                            {
                                if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(startIndex, 1))))
                                {
                                    flag4 = false;
                                    break;
                                }
                            }
                            if (flag4)
                            {
                                flag1 = true;
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                                ccoEvento.CallerID = ccoEvento.EventoOriginador.Substring(16);
                                if (this.cadPuertos.ModificCallerID(ccoEvento) != "")
                                {
                                    this.cadPuertos.InsertCallerID(ccoEvento);
                                    break;
                                }
                                break;
                            }
                            flag1 = false;
                            break;
                        }
                        int num1;
                        double num2;
                        DateTime fechaHora;
                        int num3;
                        if (ccoEvento.EventoOriginador.Substring(0, ccoEvento.EventoOriginador.IndexOf('[')).Length == 4)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(83);
                            if (ccoEvento.EventoOriginador.IndexOf('[') >= 0 && ccoEvento.EventoOriginador.IndexOf('|') >= 0 && ccoEvento.EventoOriginador.IndexOf('*') >= 0 && ccoEvento.EventoOriginador.IndexOf(']') >= 0)
                            {
                                for (int length = 0; length < 12; ++length)
                                {
                                    num1 = 0;
                                    int startIndex1 = 6 + length;
                                    if (ccoEvento.EventoOriginador.Substring(startIndex1, 1) == "|")
                                    {
                                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(ccoEvento.EventoOriginador.IndexOf('[') + 2, length).PadLeft(6, ' ');
                                        if (ccoEvento.CodigoAbonado.Trim().Length > 4)
                                            ccoEvento.CodigoAbonado = ccoEvento.CodigoAbonado.Substring(6).PadLeft(6, ' ');
                                        int startIndex2 = startIndex1 + 2;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex2, 2);
                                    }
                                }
                                string str15 = ccoEvento.EventoOriginador.Substring(ccoEvento.EventoOriginador.IndexOf('*') + 1);
                                ccoEvento.CallerID = str15.Substring(0, str15.IndexOf(']') - 1);
                            }
                            else
                            {
                                for (int length1 = 0; length1 < 11; ++length1)
                                {
                                    int startIndex3 = 6 + length1;
                                    if (ccoEvento.EventoOriginador.Substring(startIndex3, 1) == "|")
                                    {
                                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(6, length1).PadLeft(6, ' ');
                                        if (ccoEvento.CodigoAbonado.Trim().Length > (int)this.longCodAbonadoSIA)
                                            ccoEvento.CodigoAbonado = ccoEvento.CodigoAbonado.Substring(ccoEvento.CodigoAbonado.Length - (int)this.longCodAbonadoSIA, 4).PadLeft(6, ' ');
                                        int startIndex4 = startIndex3 + 2;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex4, 2);
                                        int startIndex5 = startIndex4 + 2;
                                        if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                        {
                                            flag3 = false;
                                            for (int index = startIndex5; index < ccoEvento.EventoOriginador.Length; ++index)
                                            {
                                                if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))))
                                                {
                                                    ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index) + "/" + ccoEvento.EventoOriginador.Substring(index);
                                                    break;
                                                }
                                            }
                                        }
                                        for (int length2 = 0; length2 <= 4; ++length2)
                                        {
                                            if (ccoEvento.EventoOriginador.Substring(startIndex5, 1) == "/")
                                            {
                                                Evento evento = ccoEvento;
                                                num2 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex5 - length2, length2));
                                                string str16 = num2.ToString().Trim();
                                                evento.CodigoZona = str16;
                                                ccoEvento.FechaHora = ccoEvento.FechaHora.AddMilliseconds(1.0);
                                                ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                                AbonadoPart ccAbonPart = new AbonadoPart();
                                                ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                                ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                                if (ccoEvento.CodigoEvento == "ri")
                                                {
                                                    ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                                    str3 = "";
                                                    if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                        this.cadPuertos.UpdateParticion(ccAbonPart);
                                                    flag1 = true;
                                                }
                                                else
                                                {
                                                    byte num4 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                                    ccoEvento.Particion = num4;
                                                }
                                                if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                                                {
                                                    if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                                        ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                                    if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                                        ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                                                }
                                                if (this.CodigoAbonadoRepetido)
                                                    ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                                List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                                if (list.Count != 0)
                                                    ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                                                string str17 = this.cadPuertos.InsertEventos(ccoEvento);
                                                if (str17 != "")
                                                {
                                                    if (str17.Substring(0, 6) == "@@$$¿¿")
                                                        str1 = str17.Substring(6);
                                                    else
                                                        break;
                                                }
                                                str1 = ccoEvento.FechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                                Thread.Sleep(200);
                                                flag1 = true;
                                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex5 + 1, 2);
                                                startIndex5 = startIndex5 + 2 + 1;
                                                length2 = 0;
                                            }
                                            if (ccoEvento.EventoOriginador.Substring(startIndex5, 1) == "]")
                                            {
                                                Evento evento1 = ccoEvento;
                                                fechaHora = ccoEvento.FechaHora;
                                                DateTime dateTime = fechaHora.AddMilliseconds(1.0);
                                                evento1.FechaHora = dateTime;
                                                Evento evento2 = ccoEvento;
                                                num2 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex5 - length2, length2));
                                                string str18 = num2.ToString().Trim();
                                                evento2.CodigoZona = str18;
                                                AbonadoPart ccAbonPart = new AbonadoPart();
                                                ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                                ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                                if (ccoEvento.CodigoEvento == "ri")
                                                {
                                                    ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                                    str3 = "";
                                                    if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                        this.cadPuertos.UpdateParticion(ccAbonPart);
                                                    flag1 = true;
                                                    break;
                                                }
                                                byte num5 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                                ccoEvento.Particion = num5;
                                                break;
                                            }
                                            ++startIndex5;
                                            num3 = 0;
                                        }
                                        break;
                                    }
                                }
                                ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                            }
                            if (!flag3)
                                ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Replace("/", "");
                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                            {
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                            }
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list1 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list1.Count != 0)
                                ccoEvento.CodigoEvento = list1[0].CodigoEvento.Trim() + list1[0].CodigoZona.Trim();
                            string str19 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str19 != "")
                            {
                                if (str19.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str19.Substring(6);
                                else
                                    break;
                            }
                            fechaHora = ccoEvento.FechaHora;
                            str1 = fechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                            flag1 = true;
                        }
                        if (ccoEvento.EventoOriginador.Substring(0, ccoEvento.EventoOriginador.IndexOf('[')).Length == 6)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(4, 2);
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(83);
                            for (int length3 = 0; length3 < 8; ++length3)
                            {
                                num1 = 0;
                                int startIndex6 = 8 + length3;
                                if (ccoEvento.EventoOriginador.Substring(startIndex6, 1) == "|")
                                {
                                    ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, length3).PadLeft(6, ' ');
                                    int startIndex7 = startIndex6 + 2;
                                    ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex7, 2);
                                    int startIndex8 = startIndex7 + 2;
                                    if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                    {
                                        flag3 = false;
                                        for (int index = startIndex8; index < ccoEvento.EventoOriginador.Length; ++index)
                                        {
                                            if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))))
                                            {
                                                ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index) + "/" + ccoEvento.EventoOriginador.Substring(index);
                                                break;
                                            }
                                        }
                                    }
                                    for (int length4 = 0; length4 <= 4; ++length4)
                                    {
                                        if (ccoEvento.EventoOriginador.Substring(startIndex8, 1) == "/")
                                        {
                                            Evento evento3 = ccoEvento;
                                            num2 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex8 - length4, length4));
                                            string str20 = num2.ToString().Trim();
                                            evento3.CodigoZona = str20;
                                            Evento evento4 = ccoEvento;
                                            fechaHora = ccoEvento.FechaHora;
                                            DateTime dateTime = fechaHora.AddMilliseconds(1.0);
                                            evento4.FechaHora = dateTime;
                                            ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                            AbonadoPart ccAbonPart = new AbonadoPart();
                                            ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                            ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                            if (ccoEvento.CodigoEvento == "ri")
                                            {
                                                ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                                str3 = "";
                                                if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                    this.cadPuertos.UpdateParticion(ccAbonPart);
                                                flag1 = true;
                                            }
                                            else
                                            {
                                                byte num6 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                                ccoEvento.Particion = num6;
                                            }
                                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                                            {
                                                if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                                if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                                            }
                                            if (this.CodigoAbonadoRepetido)
                                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                            List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                            if (list.Count != 0)
                                                ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                                            string str21 = this.cadPuertos.InsertEventos(ccoEvento);
                                            if (str21 != "")
                                            {
                                                if (str21.Substring(0, 6) == "@@$$¿¿")
                                                    str1 = str21.Substring(6);
                                                else
                                                    break;
                                            }
                                            fechaHora = ccoEvento.FechaHora;
                                            str1 = fechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                            Thread.Sleep(200);
                                            flag1 = true;
                                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex8 + 1, 2);
                                            startIndex8 = startIndex8 + 2 + 1;
                                            length4 = 0;
                                        }
                                        if (ccoEvento.EventoOriginador.Substring(startIndex8, 1) == "]")
                                        {
                                            Evento evento5 = ccoEvento;
                                            fechaHora = ccoEvento.FechaHora;
                                            DateTime dateTime = fechaHora.AddMilliseconds(1.0);
                                            evento5.FechaHora = dateTime;
                                            Evento evento6 = ccoEvento;
                                            num2 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex8 - length4, length4));
                                            string str22 = num2.ToString().Trim();
                                            evento6.CodigoZona = str22;
                                            AbonadoPart ccAbonPart = new AbonadoPart();
                                            ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                            ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                            if (ccoEvento.CodigoEvento == "ri")
                                            {
                                                ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                                str3 = "";
                                                if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                    this.cadPuertos.UpdateParticion(ccAbonPart);
                                                flag1 = true;
                                                break;
                                            }
                                            byte num7 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                            ccoEvento.Particion = num7;
                                            break;
                                        }
                                        ++startIndex8;
                                        num3 = 0;
                                    }
                                    break;
                                }
                            }
                            ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                            if (!flag3)
                                ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Replace("/", "");
                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                            {
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                            }
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list2 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list2.Count != 0)
                                ccoEvento.CodigoEvento = list2[0].CodigoEvento.Trim() + list2[0].CodigoZona.Trim();
                            string str23 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str23 != "")
                            {
                                if (str23.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str23.Substring(6);
                                else
                                    break;
                            }
                            fechaHora = ccoEvento.FechaHora;
                            str1 = fechaHora.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                            flag1 = true;
                            break;
                        }
                        break;
                    case "5":
                        if (ccoEvento.EventoOriginador.Length == 20)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(ccoEvento.EventoOriginador.Substring(5, 2));
                            ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(7, 4).PadLeft(6, ' ');
                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(11, 4);
                            ccoEvento.Particion = (byte)MPTCP.Val(ccoEvento.EventoOriginador.Substring(15, 2));
                            ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(17, 3)).ToString().Trim();
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp2.Trim())
                            {
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona3.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp3.Trim();
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona4.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp4.Trim();
                            }
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "5" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list.Count != 0)
                                ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                            string str24 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str24 != "")
                            {
                                if (str24.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str24.Substring(6);
                                else
                                    break;
                            }
                            flag1 = true;
                            break;
                        }
                        if (ccoEvento.EventoOriginador.Length == 22)
                        {
                            ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 3).PadLeft(4, ' ');
                            ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(5, 1);
                            ccoEvento.CodigoProtocolo = Convert.ToByte(ccoEvento.EventoOriginador.Substring(7, 2));
                            ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(9, 4).PadLeft(6, ' ');
                            ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(13, 4);
                            ccoEvento.Particion = (byte)MPTCP.Val(ccoEvento.EventoOriginador.Substring(17, 2));
                            ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(19, 3)).ToString().Trim();
                            ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                            ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                            if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp2.Trim())
                            {
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona3.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp3.Trim();
                                if (ccoEvento.CodigoZona == this.ccParam.CodZona4.Trim())
                                    ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp4.Trim();
                            }
                            if (this.CodigoAbonadoRepetido)
                                ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                            List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "5" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                            if (list.Count != 0)
                                ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                            string str25 = this.cadPuertos.InsertEventos(ccoEvento);
                            if (str25 != "")
                            {
                                if (str25.Substring(0, 6) == "@@$$¿¿")
                                    str1 = str25.Substring(6);
                                else
                                    break;
                            }
                            flag1 = true;
                        }
                        else
                            flag1 = false;
                        break;
                    default:
                        flag1 = false;
                        break;
                }
                if (!flag1)
                    ccoEvento.FechaHora = DateTime.MinValue;
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2("MPSER", "Puerto " + (object)ccoEvento.PuertoCommNum, "0", ex.HResult, ex.Message + " " + ex.StackTrace + " " + (object)ex.TargetSite);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                ccoEvento.EventoOriginador = "";
                ccoEvento.ReceptorNum = "";
                ccoEvento.LineaNum = "0";
                ccoEvento.CodigoAbonado = "  0000";
                ccoEvento.CodigoEvento = this.ccParam.CodEventoError;
                ccoEvento.CodigoZona = "0";
                ccoEvento.PuertoCommNum = (byte)0;
                ccoEvento.CodigoProtocolo = (byte)0;
                string s = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff }", (object)this.cadPuertos.GetServerDateTime());
                ccoEvento.FechaHora = DateTime.Parse(s);
                this.cadPuertos.InsertEventos(ccoEvento);
                ccoEvento.FechaHora = DateTime.MinValue;
            }
            return ccoEvento;
        }

        private Evento ProceBosch(string Evento, string PuertoCOMM, int CodigoReceptora)
        {
            string str1 = "";
            Evento ccoEvento = new Evento();
            bool flag1 = false;
            bool flag2 = true;
            try
            {
                ccoEvento.LimpiaProps();
                str1 = Evento.Substring(0, 24);
                ccoEvento.Estado = "A";
                ccoEvento.FechaHora = DateTime.Parse(Evento.Substring(0, 25));
                ccoEvento.EventoOriginador = Evento.Substring(25);
                string str2 = ccoEvento.EventoOriginador.Substring(0, 1);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                int num1;
                switch (str2)
                {
                    case "1":
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 1).Trim();
                        if (ccoEvento.EventoOriginador.IndexOf("           @    ") >= 0)
                        {
                            flag1 = true;
                            break;
                        }
                        if (ccoEvento.EventoOriginador.Length != 20)
                        {
                            flag1 = false;
                            break;
                        }
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(50);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(4, 10).Trim().PadLeft(6, ' ');
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(14, 2).Trim();
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        ccoEvento.CodigoZona = ccoEvento.EventoOriginador.Substring(16, 4).Trim();
                        for (int startIndex = 0; startIndex < ccoEvento.CodigoZona.Length; ++startIndex)
                        {
                            if (!char.IsNumber(Convert.ToChar(ccoEvento.CodigoZona.Substring(startIndex, 1))))
                            {
                                flag2 = false;
                                break;
                            }
                        }
                        if (flag2)
                            ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(16, 4)).ToString().Trim();
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        List<EventoZona> list1 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)7 && p.CodigoProtocolo == "1" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                        if (list1.Count != 0)
                            ccoEvento.CodigoEvento = list1[0].CodigoEvento.Trim() + list1[0].CodigoZona.Trim();
                        string str3 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str3 != "")
                        {
                            if (str3.Substring(0, 6) == "@@$$¿¿")
                                str1 = str3.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "i":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(50);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).PadLeft(6, ' ');
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(14, 2);
                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(16, 2)).ToString().Trim();
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        string str4 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str4 != "")
                        {
                            if (str4.Substring(0, 6) == "@@$$¿¿")
                                str1 = str4.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "S":
                        bool flag3 = true;
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(83);
                        DateTime fechaHora1;
                        for (int length1 = 0; length1 <= 6; ++length1)
                        {
                            int startIndex1 = 6 + length1;
                            if (ccoEvento.EventoOriginador.Substring(startIndex1, 1) == "|")
                            {
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(6, length1).PadLeft(6, ' ');
                                if (ccoEvento.CodigoAbonado.Trim().Length > (int)this.longCodAbonadoSIA)
                                    ccoEvento.CodigoAbonado = ccoEvento.CodigoAbonado.Substring(ccoEvento.CodigoAbonado.Length - (int)this.longCodAbonadoSIA, 4).PadLeft(6, ' ');
                                int startIndex2 = startIndex1 + 2;
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex2, 2);
                                int startIndex3 = startIndex2 + 2;
                                if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                {
                                    flag3 = false;
                                    for (int index = startIndex3; index < ccoEvento.EventoOriginador.Length; ++index)
                                    {
                                        if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))))
                                        {
                                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index) + "/" + ccoEvento.EventoOriginador.Substring(index);
                                            break;
                                        }
                                    }
                                }
                                for (int length2 = 0; length2 < 4; ++length2)
                                {
                                    string str5;
                                    if (ccoEvento.EventoOriginador.Substring(startIndex3, 1) == "/")
                                    {
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex3 - length2, length2)).ToString().Trim();
                                        Evento evento = ccoEvento;
                                        fechaHora1 = ccoEvento.FechaHora;
                                        DateTime dateTime = fechaHora1.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime;
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str5 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                        }
                                        else
                                        {
                                            byte num2 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                            ccoEvento.Particion = num2;
                                        }
                                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                                        {
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                                        }
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        List<EventoZona> list2 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)7 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                        if (list2.Count != 0)
                                            ccoEvento.CodigoEvento = list2[0].CodigoEvento.Trim() + list2[0].CodigoZona.Trim();
                                        string str6 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str6 != "")
                                        {
                                            if (str6.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str6.Substring(6);
                                            else
                                                break;
                                        }
                                        fechaHora1 = ccoEvento.FechaHora;
                                        str1 = fechaHora1.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                        flag1 = true;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex3 + 1, 2);
                                        startIndex3 = startIndex3 + 2 + 1;
                                        length2 = 0;
                                    }
                                    if (ccoEvento.EventoOriginador.Substring(startIndex3, 1) == "]")
                                    {
                                        Evento evento = ccoEvento;
                                        fechaHora1 = ccoEvento.FechaHora;
                                        DateTime dateTime = fechaHora1.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime;
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex3 - length2, length2)).ToString().Trim();
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str5 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                            break;
                                        }
                                        byte num3 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                        ccoEvento.Particion = num3;
                                        break;
                                    }
                                    ++startIndex3;
                                    num1 = 0;
                                }
                                break;
                            }
                        }
                        if (!flag3)
                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Replace("/", "");
                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                        {
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                        }
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        List<EventoZona> list3 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)7 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                        if (list3.Count != 0)
                            ccoEvento.CodigoEvento = list3[0].CodigoEvento.Trim() + list3[0].CodigoZona.Trim();
                        string str7 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str7 != "")
                        {
                            if (str7.Substring(0, 6) == "@@$$¿¿")
                                str1 = str7.Substring(6);
                            else
                                break;
                        }
                        fechaHora1 = ccoEvento.FechaHora;
                        str1 = fechaHora1.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                        flag1 = true;
                        break;
                    case "s":
                        bool flag4 = true;
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(2, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(4, 1);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(51);
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        for (int length3 = 0; length3 < 11; ++length3)
                        {
                            int startIndex4 = 6 + length3;
                            if (ccoEvento.EventoOriginador.Substring(startIndex4, 1) == "|")
                            {
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(7, length3).PadLeft(6, ' ');
                                if (ccoEvento.CodigoAbonado.Trim().Length > (int)this.longCodAbonadoSIA)
                                    ccoEvento.CodigoAbonado = ccoEvento.CodigoAbonado.Substring(ccoEvento.CodigoAbonado.Length - (int)this.longCodAbonadoSIA, 4).PadLeft(6, ' ');
                                int startIndex5 = startIndex4 + 2;
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex5, 2);
                                int startIndex6 = startIndex5 + 2;
                                if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                {
                                    flag4 = false;
                                    for (int index = startIndex6; index < ccoEvento.EventoOriginador.Length; ++index)
                                    {
                                        if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))))
                                        {
                                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index) + "/" + ccoEvento.EventoOriginador.Substring(index);
                                            break;
                                        }
                                    }
                                }
                                for (int length4 = 0; length4 <= 4; ++length4)
                                {
                                    DateTime fechaHora2;
                                    if (ccoEvento.EventoOriginador.Substring(startIndex6, 1) == "/")
                                    {
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex6 - length4, length4)).ToString().Trim();
                                        Evento evento = ccoEvento;
                                        fechaHora2 = ccoEvento.FechaHora;
                                        DateTime dateTime = fechaHora2.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime;
                                        ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                        AbonadoPart abonadoPart = new AbonadoPart()
                                        {
                                            CodigoAbonado = ccoEvento.CodigoAbonado,
                                            CodigoReceptora = ccoEvento.CodigoReceptora
                                        };
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        List<EventoZona> list4 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)7 && p.CodigoProtocolo == "s" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                        if (list4.Count != 0)
                                            ccoEvento.CodigoEvento = list4[0].CodigoEvento.Trim() + list4[0].CodigoZona.Trim();
                                        string str8 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str8 != "")
                                        {
                                            if (str8.Substring(0, 6) == "@@$$¿¿")
                                                str1 = str8.Substring(6);
                                            else
                                                break;
                                        }
                                        fechaHora2 = ccoEvento.FechaHora;
                                        str1 = fechaHora2.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                        Thread.Sleep(200);
                                        flag1 = true;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex6 + 1, 2);
                                        startIndex6 = startIndex6 + 2 + 1;
                                        length4 = 0;
                                    }
                                    if (ccoEvento.EventoOriginador.Substring(startIndex6, 1) == "]")
                                    {
                                        Evento evento = ccoEvento;
                                        fechaHora2 = ccoEvento.FechaHora;
                                        DateTime dateTime = fechaHora2.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime;
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex6 - length4, length4)).ToString().Trim();
                                        AbonadoPart abonadoPart = new AbonadoPart()
                                        {
                                            CodigoAbonado = ccoEvento.CodigoAbonado,
                                            CodigoReceptora = ccoEvento.CodigoReceptora
                                        };
                                        break;
                                    }
                                    ++startIndex6;
                                    num1 = 0;
                                }
                                break;
                            }
                        }
                        break;
                    case "I":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(2, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(4, 1);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(10, 4).PadLeft(6, ' ');
                        ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(15, 1);
                        ccoEvento.Particion = (byte)MPTCP.Val(ccoEvento.EventoOriginador.Substring(14, 1));
                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(16, 2)).ToString().Trim();
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        string str9 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str9 != "")
                        {
                            if (str9.Substring(0, 6) == "@@$$¿¿")
                                str1 = str9.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "j":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(2, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(4, 1);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(9, 6).PadLeft(6, ' ');
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(15, 2);
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        string str10 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str10 != "")
                        {
                            if (str10.Substring(0, 6) == "@@$$¿¿")
                                str1 = str10.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "k":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(2, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(4, 1);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(11, 5).PadLeft(6, ' ');
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(17, 1);
                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(19, 2)).ToString().Trim();
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        string str11 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str11 != "")
                        {
                            if (str11.Substring(0, 6) == "@@$$¿¿")
                                str1 = str11.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "l":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(2, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(4, 1);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(6, 6).PadLeft(6, ' ');
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(13, 1);
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        string str12 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str12 != "")
                        {
                            if (str12.Substring(0, 6) == "@@$$¿¿")
                                str1 = str12.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "n":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(2, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(4, 1);
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, 6).PadLeft(6, ' ');
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        string str13 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str13 != "")
                        {
                            if (str13.Substring(0, 6) == "@@$$¿¿")
                                str1 = str13.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                    case "a":
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(1, 2).PadLeft(4, ' ');
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(3, 1);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(ccoEvento.EventoOriginador.Substring(10, 2));
                        ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(5, 4).PadLeft(6, ' ');
                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(11, 4);
                        ccoEvento.Particion = (byte)MPTCP.Val(ccoEvento.EventoOriginador.Substring(15, 2));
                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(17, 3)).ToString().Trim();
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        List<EventoZona> list5 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)7 && p.CodigoProtocolo == "a" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                        if (list5.Count != 0)
                            ccoEvento.CodigoEvento = list5[0].CodigoEvento.Trim() + list5[0].CodigoZona.Trim();
                        string str14 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str14 != "")
                        {
                            if (str14.Substring(0, 6) == "@@$$¿¿")
                                str1 = str14.Substring(6);
                            else
                                break;
                        }
                        flag1 = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2("MPC", "Puerto " + (object)ccoEvento.PuertoCommNum, "0", ex.HResult, ex.Message + " " + ex.StackTrace + " " + (object)ex.TargetSite);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                ccoEvento.EventoOriginador = "";
                ccoEvento.ReceptorNum = "";
                ccoEvento.LineaNum = "0";
                ccoEvento.CodigoAbonado = "  0000";
                ccoEvento.CodigoEvento = this.ccParam.CodEventoError;
                ccoEvento.CodigoZona = "0";
                ccoEvento.PuertoCommNum = (byte)0;
                ccoEvento.CodigoProtocolo = (byte)0;
                string s = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff }", (object)this.cadPuertos.GetServerDateTime());
                ccoEvento.FechaHora = DateTime.Parse(s);
                this.cadPuertos.InsertEventos(ccoEvento);
                ccoEvento.FechaHora = DateTime.MinValue;
            }
            return ccoEvento;
        }

        private Evento ProceRisco(string Evento, string PuertoCOMM, int CodigoReceptora)
        {
            string str1 = "";
            string str2 = "";
            bool flag1 = false;
            bool flag2 = false;
            Evento ccoEvento = new Evento();
            EventoVideo ccoEventoVideo = new EventoVideo();
            try
            {
                ccoEvento.LimpiaProps();
                DateTime dateTime1 = DateTime.Now;
                Evento = dateTime1.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " " + Evento;
                str2 = Evento.Substring(0, 24);
                ccoEvento.Estado = "A";
                ccoEvento.EventoOriginador = Evento.Substring(25);
                ccoEvento.FechaHora = Convert.ToDateTime(Evento.Substring(0, 25));
                for (int startIndex = 0; startIndex < 25; ++startIndex)
                {
                    if (ccoEvento.EventoOriginador.Substring(startIndex, 1) == "R")
                        ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(startIndex + 1, 1);
                }
                for (int startIndex = 0; startIndex < 25; ++startIndex)
                {
                    if (ccoEvento.EventoOriginador.Substring(startIndex, 1) == "L")
                        ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(startIndex + 1, 1);
                }
                ccoEvento.longitudCad = Evento.Substring(26, 2);
                string str3 = "";
                for (int startIndex = 0; startIndex < 10; ++startIndex)
                {
                    if (ccoEvento.EventoOriginador.Substring(startIndex, 1) == "S")
                        str3 = ccoEvento.EventoOriginador.Substring(startIndex, 1);
                }
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                switch (str3)
                {
                    case "S":
                        bool flag3 = true;
                        string str4;
                        double num1;
                        int num2;
                        for (int index1 = 0; index1 < 30; ++index1)
                        {
                            int startIndex1 = 6 + index1;
                            if (ccoEvento.EventoOriginador.Substring(startIndex1, 1) == "|")
                            {
                                int startIndex2 = startIndex1 - 4;
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(startIndex2, 4).PadLeft(6, ' ');
                                if (ccoEvento.CodigoAbonado.Trim().Length > (int)this.longCodAbonadoSIA)
                                    ccoEvento.CodigoAbonado = ccoEvento.CodigoAbonado.Substring(ccoEvento.CodigoAbonado.Length - (int)this.longCodAbonadoSIA, 4).PadLeft(6, ' ');
                                int startIndex3 = startIndex2 + 6;
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex3, 2);
                                int startIndex4 = startIndex3 + 2;
                                if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                {
                                    flag3 = false;
                                    for (int index2 = startIndex4; index2 < ccoEvento.EventoOriginador.Length; ++index2)
                                    {
                                        if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index2, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index2, 1))))
                                        {
                                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index2) + "/" + ccoEvento.EventoOriginador.Substring(index2);
                                            break;
                                        }
                                    }
                                }
                                for (int length = 0; length <= 4; ++length)
                                {
                                    if (ccoEvento.EventoOriginador.Substring(startIndex4, 1) == "/")
                                    {
                                        ccoEvento.CodigoZona = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex4 - length, length)).ToString().Trim();
                                        Evento evento = ccoEvento;
                                        dateTime1 = ccoEvento.FechaHora;
                                        DateTime dateTime2 = dateTime1.AddMilliseconds(1.0);
                                        evento.FechaHora = dateTime2;
                                        ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento.ToLower() == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str4 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                        }
                                        else
                                        {
                                            byte num3 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                            ccoEvento.Particion = num3;
                                        }
                                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                                        {
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                                        }
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        List<EventoZona> list = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                        if (list.Count != 0)
                                            ccoEvento.CodigoEvento = list[0].CodigoEvento.Trim() + list[0].CodigoZona.Trim();
                                        string str5 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str5 != "")
                                        {
                                            if (str5.Substring(0, 6) == "@@$$¿¿")
                                                str2 = str5.Substring(6);
                                            else
                                                break;
                                        }
                                        dateTime1 = ccoEvento.FechaHora;
                                        str2 = dateTime1.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                        Thread.Sleep(200);
                                        flag1 = true;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex4 + 1, 2);
                                        startIndex4 = startIndex4 + 2 + 1;
                                        length = 0;
                                    }
                                    if (ccoEvento.EventoOriginador.Substring(startIndex4, 1) == "]")
                                    {
                                        Evento evento1 = ccoEvento;
                                        dateTime1 = ccoEvento.FechaHora;
                                        DateTime dateTime3 = dateTime1.AddMilliseconds(1.0);
                                        evento1.FechaHora = dateTime3;
                                        Evento evento2 = ccoEvento;
                                        num1 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex4 - length, length));
                                        string str6 = num1.ToString().Trim();
                                        evento2.CodigoZona = str6;
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento.ToLower() == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str4 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                            break;
                                        }
                                        byte num4 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                        ccoEvento.Particion = num4;
                                        break;
                                    }
                                    ++startIndex4;
                                    num2 = 0;
                                }
                                break;
                            }
                        }
                        ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                        if (!flag3)
                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Replace("/", "");
                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                        {
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                        }
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        List<EventoZona> list1 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                        if (list1.Count != 0)
                            ccoEvento.CodigoEvento = list1[0].CodigoEvento.Trim() + list1[0].CodigoZona.Trim();
                        string str7 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str7 != "")
                        {
                            if (str7.Substring(0, 6) == "@@$$¿¿")
                                str2 = str7.Substring(6);
                            else
                                break;
                        }
                        dateTime1 = ccoEvento.FechaHora;
                        str2 = dateTime1.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                        flag1 = true;
                        for (int startIndex = 0; startIndex < 25; ++startIndex)
                        {
                            if (ccoEvento.EventoOriginador.Substring(startIndex, 1) == "R")
                                ccoEvento.ReceptorNum = ccoEvento.EventoOriginador.Substring(startIndex + 1, 1);
                            if (ccoEvento.EventoOriginador.Substring(startIndex, 1) == "L")
                                ccoEvento.LineaNum = ccoEvento.EventoOriginador.Substring(startIndex + 1, 1);
                        }
                        ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(4, 2);
                        ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                        ccoEvento.CodigoProtocolo = Convert.ToByte(83);
                        for (int length1 = 0; length1 < 8; ++length1)
                        {
                            int startIndex5 = 8 + length1;
                            if (ccoEvento.EventoOriginador.Substring(startIndex5, 1) == "|")
                            {
                                ccoEvento.CodigoAbonado = ccoEvento.EventoOriginador.Substring(8, length1).PadLeft(6, ' ');
                                int startIndex6 = startIndex5 + 2;
                                ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex6, 2);
                                int startIndex7 = startIndex6 + 2;
                                if (ccoEvento.EventoOriginador.IndexOf('/') == -1)
                                {
                                    flag3 = false;
                                    for (int index = startIndex7; index < ccoEvento.EventoOriginador.Length; ++index)
                                    {
                                        if (!char.IsNumber(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))) && char.IsLetter(Convert.ToChar(ccoEvento.EventoOriginador.Substring(index, 1))))
                                        {
                                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Substring(0, index) + "/" + ccoEvento.EventoOriginador.Substring(index);
                                            break;
                                        }
                                    }
                                }
                                ccoEvento.UsuarioNum = ccoEvento.EventoOriginador.Substring(4, 2);
                                ccoEvento.PuertoCommNum = Convert.ToByte(PuertoCOMM);
                                ccoEvento.CodigoProtocolo = Convert.ToByte(83);
                                for (int length2 = 0; length2 <= 4; ++length2)
                                {
                                    if (ccoEvento.EventoOriginador.Substring(startIndex7, 1) == "/")
                                    {
                                        Evento evento3 = ccoEvento;
                                        num1 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex7 - length2, length2));
                                        string str8 = num1.ToString().Trim();
                                        evento3.CodigoZona = str8;
                                        Evento evento4 = ccoEvento;
                                        dateTime1 = ccoEvento.FechaHora;
                                        DateTime dateTime4 = dateTime1.AddMilliseconds(1.0);
                                        evento4.FechaHora = dateTime4;
                                        ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str4 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                        }
                                        else
                                        {
                                            byte num5 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                            ccoEvento.Particion = num5;
                                        }
                                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                                        {
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                                        }
                                        if (this.CodigoAbonadoRepetido)
                                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                                        List<EventoZona> list2 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                                        if (list2.Count != 0)
                                            ccoEvento.CodigoEvento = list2[0].CodigoEvento.Trim() + list2[0].CodigoZona.Trim();
                                        string str9 = this.cadPuertos.InsertEventos(ccoEvento);
                                        if (str9 != "")
                                        {
                                            if (str9.Substring(0, 6) == "@@$$¿¿")
                                                str2 = str9.Substring(6);
                                            else
                                                break;
                                        }
                                        dateTime1 = ccoEvento.FechaHora;
                                        str2 = dateTime1.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                                        Thread.Sleep(200);
                                        if (str1 != "")
                                        {
                                            MessageError messageError = new MessageError();
                                            messageError.mensajeError = "Error con las direcciones IP. " + str1;
                                            this.Hide();
                                            int num6 = (int)messageError.ShowDialog();
                                            this.Close();
                                            break;
                                        }
                                        flag1 = true;
                                        ccoEvento.CodigoEvento = ccoEvento.EventoOriginador.Substring(startIndex7 + 1, 2);
                                        startIndex7 = startIndex7 + 2 + 1;
                                        length2 = 0;
                                    }
                                    if (ccoEvento.EventoOriginador.Substring(startIndex7, 1) == "]")
                                    {
                                        Evento evento5 = ccoEvento;
                                        dateTime1 = ccoEvento.FechaHora;
                                        DateTime dateTime5 = dateTime1.AddMilliseconds(1.0);
                                        evento5.FechaHora = dateTime5;
                                        Evento evento6 = ccoEvento;
                                        num1 = MPTCP.Val(ccoEvento.EventoOriginador.Substring(startIndex7 - length2, length2));
                                        string str10 = num1.ToString().Trim();
                                        evento6.CodigoZona = str10;
                                        AbonadoPart ccAbonPart = new AbonadoPart();
                                        ccAbonPart.CodigoAbonado = ccoEvento.CodigoAbonado;
                                        ccAbonPart.CodigoReceptora = ccoEvento.CodigoReceptora;
                                        if (ccoEvento.CodigoEvento == "ri")
                                        {
                                            ccAbonPart.NumParticion = Convert.ToByte(ccoEvento.CodigoZona);
                                            str4 = "";
                                            if (this.cadPuertos.InsertParticion(ccAbonPart).Trim() == "Llave duplicada")
                                                this.cadPuertos.UpdateParticion(ccAbonPart);
                                            flag1 = true;
                                            break;
                                        }
                                        byte num7 = this.cadPuertos.BuscaParticion(ccAbonPart);
                                        ccoEvento.Particion = num7;
                                        break;
                                    }
                                    ++startIndex7;
                                    num2 = 0;
                                }
                                break;
                            }
                        }
                        ccoEvento.CallerID = this.cadPuertos.ConsultaCallerID(ccoEvento);
                        if (!flag3)
                            ccoEvento.EventoOriginador = ccoEvento.EventoOriginador.Replace("/", "");
                        if (ccoEvento.CodigoEvento.Trim() == this.ccParam.CodEventoAReemp.Trim())
                        {
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona1.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp1.Trim();
                            if (ccoEvento.CodigoZona == this.ccParam.CodZona2.Trim())
                                ccoEvento.CodigoEvento = this.ccParam.CodEventoReemp2.Trim();
                        }
                        if (this.CodigoAbonadoRepetido)
                            ccoEvento.CodigoReceptora = !this.cadPuertos.AbonadoUnico(ccoEvento.CodigoAbonado) ? Convert.ToByte(CodigoReceptora) : (byte)0;
                        List<EventoZona> list3 = this.ltEZ.Where<EventoZona>((Func<EventoZona, bool>)(p => p.EmulaA == (byte)6 && p.CodigoProtocolo == "S" && p.CodigoEvento.Trim() == ccoEvento.CodigoEvento.Trim() && p.CodigoZona.Trim() == ccoEvento.CodigoZona.Trim())).ToList<EventoZona>();
                        if (list3.Count != 0)
                            ccoEvento.CodigoEvento = list3[0].CodigoEvento.Trim() + list3[0].CodigoZona.Trim();
                        string str11 = this.cadPuertos.InsertEventos(ccoEvento);
                        if (str11 != "")
                        {
                            if (str11.Substring(0, 6) == "@@$$¿¿")
                                str2 = str11.Substring(6);
                            else
                                break;
                        }
                        dateTime1 = ccoEvento.FechaHora;
                        str2 = dateTime1.ToString("yyyy/MM/dd HH:mm:ss.ffff");
                        flag1 = true;
                        break;
                }
                if (!flag2)
                {
                    for (int index = 0; index < 40; ++index)
                    {
                        if (ccoEvento.EventoOriginador.Substring(40 + index, 1) == "V")
                        {
                            int startIndex = ccoEvento.EventoOriginador.IndexOf("V");
                            if (ccoEvento.EventoOriginador.Substring(startIndex, 1) == "V")
                            {
                                ccoEventoVideo.FechaHora = ccoEvento.FechaHora;
                                ccoEventoVideo.CodigoAbonado = ccoEvento.CodigoAbonado;
                                ccoEventoVideo.Video = ccoEvento.EventoOriginador.Substring(startIndex - 1);
                                this.cadPuertos.InsertEventosVideo(ccoEventoVideo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2("MPC", "Puerto " + (object)ccoEvento.PuertoCommNum, "0", ex.HResult, ex.Message + " SYSTEMIII  " + ccoEvento.EventoOriginador + " " + ex.StackTrace + " " + (object)ex.TargetSite);
                ccoEvento.CodigoReceptoraReal = Convert.ToByte(CodigoReceptora);
                ccoEvento.CodigoReceptora = !this.CodigoAbonadoRepetido ? (byte)0 : Convert.ToByte(CodigoReceptora);
                ccoEvento.EventoOriginador = "";
                ccoEvento.ReceptorNum = "";
                ccoEvento.LineaNum = "0";
                ccoEvento.CodigoAbonado = "  0000";
                ccoEvento.CodigoEvento = this.ccParam.CodEventoError;
                ccoEvento.CodigoZona = "0";
                ccoEvento.PuertoCommNum = (byte)0;
                ccoEvento.CodigoProtocolo = (byte)0;
                string s = string.Format("{0:yyyy-MM-dd HH:mm:ss.ffff }", (object)this.cadPuertos.GetServerDateTime());
                ccoEvento.FechaHora = DateTime.Parse(s);
                string str12 = this.cadPuertos.InsertEventos(ccoEvento);
                if (str12 != "" && str12.Substring(0, 6) == "@@$$¿¿")
                    str2 = str12.Substring(6);
                ccoEvento.FechaHora = DateTime.MinValue;
            }
            return ccoEvento;
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            int num = (int)new EliminaIP().ShowDialog();
        }

        public void PintarBoton(Button btn, ToolTip ttp, string mensaje)
        {
            btn.BackColor = Color.Orange;
            if (mensaje.Trim() == "")
            {
                ttp.SetToolTip((Control)btn, "Cadena vacia muchas veces, se presupone desconección.");
                int num = (int)MessageBox.Show("Cadena vacia muchas veces, se presupone desconexión.", "Advertencia");
            }
            else
                ttp.SetToolTip((Control)btn, mensaje);
        }

        public void PintarBtnServerActivo(Button btn, ToolTip ttp, string mensaje)
        {
            btn.BackColor = Color.SteelBlue;
            ttp.SetToolTip((Control)btn, mensaje);
        }

        public void PintarBtnServerClientConnect(Button btn, ToolTip ttp, string mensaje)
        {
            btn.BackColor = Color.Green;
            ttp.SetToolTip((Control)btn, mensaje);
        }

        private void MPTCP_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.blError)
                    return;
                if (MessageBox.Show("Si sale de la aplicacion el proceso se detendra, ¿Desea salir?", "CONFIRMACION", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Console.WriteLine(String.Format("{0}{1}", "Cerrando....", Sistema.sysCodigoFuncion));
                    //try
                    //{
                    //    this.cloLogica.EliminateFunctionInUse((int)Sistema.sysCodigoFuncion);
                    //}
                    //catch (Exception ex)
                    //{
                    //    int num = (int)MessageBox.Show(ex.Message);
                    //}
                }
                else
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show(ex.Message);
            }
        }

        private void btnLog1_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP1).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto1).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP1).Text.Trim() + "  " + ((Control)this.txtPuerto1).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog2_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP2).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto2).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP2).Text.Trim() + "  " + ((Control)this.txtPuerto2).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog3_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP3).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto3).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP3).Text.Trim() + "  " + ((Control)this.txtPuerto3).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog4_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP4).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto4).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP4).Text.Trim() + "  " + ((Control)this.txtPuerto4).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog5_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP5).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto5).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP5).Text.Trim() + "  " + ((Control)this.txtPuerto5).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog6_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP6).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto6).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP6).Text.Trim() + "  " + ((Control)this.txtPuerto6).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog7_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP7).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto7).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP7).Text.Trim() + "  " + ((Control)this.txtPuerto7).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnLog8_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            string str = ((Control)this.txtIP8).Text;
            for (int index = 0; index < str.Length; ++index)
            {
                if (str.Substring(index, 1) == ".")
                    str = str.Substring(0, index) + "_" + str.Substring(index + 1);
            }
            List<strTXT> strTxtList = this.cadPuertos.ConsultaLog("X_" + string.Format("{0:yyyyMMdd}_IP{1}_P{2}", (object)this.cadPuertos.GetServerDateTime(), (object)str.Trim(), (object)((Control)this.txtPuerto8).Text.Trim()));
            if (strTxtList.Count != 0)
                log.cLog = strTxtList;
            log.strNombreArchivo = ((Control)this.txtIP8).Text.Trim() + "  " + ((Control)this.txtPuerto8).Text.Trim();
            int num = (int)log.ShowDialog();
        }

        private void btnSendAck_Click(object sender, EventArgs e)
        {
            Evento evento = new Evento();
            this.ValidaEventoYGuardaBD(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff") + " S02005[#9999|Nri2/OP0001]", this.Emula1, this.Recep1, "0");
            this.ValidaEventoYGuardaBD(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff") + " S03005[#9999|Nri2/OP0001]", 6, 1, "0");
            this.ValidaEventoYGuardaBD(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ffff") + " S01004[#0255|Nri0/RP00]", 6, 1, "0");
        }

        public void StarServer(RecepcionIPSERVER server, int numServer)
        {
            switch (numServer)
            {
                case 9:
                    try
                    {
                        this._listenerServer1 = new TcpListener(IPAddress.Parse(server.ip), server.Puerto);
                        this._listenerServer1.Start();
                        this.btnConectar1.Enabled = true;
                        this._listenerServer1.BeginAcceptTcpClient(new AsyncCallback(this.HandleAcceptTcpC1), (object)this._listenerServer1);
                    }
                    catch (Exception ex)
                    {
                        this.Invoke((Delegate)new MPTCP.Pintar(this.PintarBoton), (object)this.btnConectar1, (object)this.toolTip1, (object)ex.Message);
                        int num = (int)MessageBox.Show(ex.Message);
                    }
                    return;
            }
        }

        private void HandleAcceptTcpC1(IAsyncResult result)
        {
            string str1 = "";
            try
            {
                this._tcpClient1 = this._listenerServer1.EndAcceptTcpClient(result);
                this._listenerServer1.BeginAcceptTcpClient(new AsyncCallback(this.HandleAcceptTcpC1), (object)this._listenerServer1);
                NetworkStream stream = this._tcpClient1.GetStream();
                byte[] numArray = new byte[256];
                int count;
                while (true)
                {
                    Console.WriteLine(_tcpClient1.Connected);
                    while ((count = stream.Read(numArray, 0, numArray.Length)) != 0)
                    {
                        this.Invoke((Delegate)new MPTCP.Pintar(this.PintarBtnServerClientConnect), (object)this.btnConectar1, (object)this.toolTip1, (object)"Cliente conectado !!!");
                        string str2 = Encoding.UTF8.GetString(numArray, 0, count);
                        if (str2 != "" && str2 != null)
                        {
                            string str3 = str2;
                            if (str3.IndexOf(Convert.ToChar(20)) > 0)
                            {
                                string str4 = str3.Substring(0, str3.IndexOf(Convert.ToChar(20)));
                                if (str4.Trim() != "")
                                {
                                    DateTime dateTime = DateTime.Now;
                                    string str5 = dateTime.ToString("dd/MM/yyyy HH:mm:ss.ffff") + " " + str4;
                                    dateTime = Convert.ToDateTime(str5.Substring(0, 24));
                                    if (dateTime.Year > ComData.strAñoServidor)
                                    {
                                        dateTime = DateTime.Now;
                                        ComData.strAñoServidor = dateTime.Year;
                                        this.cadPuertos.ChecaTabla("Evento" + (object)ComData.strAñoServidor);
                                    }
                                    else
                                    {
                                        this.cadPuertos.ChecaTabla("Evento" + (object)ComData.strAñoServidor);
                                    }
                                    this.GuardaDatos(str5, ((Control)this.txtIP1).Text.Trim(), ((Control)this.txtPuerto1).Text.Trim());
                                    Evento Event = this.ValidaEventoYGuardaBD(str5, this.Emula1, this.Recep1, "0");
                                    bool flag = true;
                                    if (Event.FechaHora == DateTime.MinValue) flag = false;
                                    this.Invoke((Delegate)new MPTCP.AgregaEventos(this.AddEvent), (object)Event, (object)str5, (object)this.checkedListBox1, (object)flag);
                                }
                            }
                        }
                        stream.WriteByte((byte)6);//ACK
                    }

                }
            }
            catch (SocketException es)
            {
                MessageBox.Show("SocketException SERVER 9 " + es.Message + "  " + (object)es.HResult, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (ObjectDisposedException eobj)
            {
                MessageBox.Show("ObjectDisposedException SERVER 9 " + eobj.Message + "  " + (object)eobj.HResult, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (IOException eio)
            {
                this.cadPuertos.RegistroBitacora2("** MPSER", "Linea" + str1 + eio.TargetSite.DeclaringType.ToString(), eio.StackTrace.Substring(eio.StackTrace.LastIndexOf(" en ") < 0 ? 0 : eio.StackTrace.LastIndexOf(" en ")), eio.HResult, eio.Message);
                this.Invoke((Delegate)new MPTCP.Pintar(this.PintarBtnServerActivo), (object)this.btnConectar1, (object)this.toolTip1, (object)eio.Message + " Esperando conexión del cliente");
            }
            catch (Exception ex)
            {
                this.cadPuertos.RegistroBitacora2("** MPSER", "Linea" + str1 + ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ") < 0 ? 0 : ex.StackTrace.LastIndexOf(" en ")), ex.HResult, ex.Message);
                this.Invoke((Delegate)new MPTCP.Pintar(this.PintarBtnServerActivo), (object)this.btnConectar1, (object)this.toolTip1, (object)ex.Message+" Esperando conexión del cliente");
            }
            Console.Read();
        }

        private delegate void AgregaEventos1(
         Evento ccoEvento,
         string a,
         CheckedListBox ckblb,
         bool blnValidaEvento);

        private delegate void AgregaEventos(
          Evento ccEvento,
          string a,
          CheckedListBox ckblb,
          bool blnValidaEvento);

        public delegate string ConnectClient(string Cliente, string message, int port);

        public delegate void Pintar(Button btn, ToolTip ttp, string mensaje);


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MPTCP));
            this.btnSalir = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnBorrar = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip3 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip4 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip5 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip6 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSendAck = new System.Windows.Forms.Button();
            this.toolTip7 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip8 = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbTCP8 = new CustomControls.checkBoxNew();
            this.gb8 = new System.Windows.Forms.GroupBox();
            this.txtPuerto8 = new CustomControls.textBoxNum();
            this.cmbReceptora8 = new CustomControls.ComboBoxNew();
            this.btnLog8 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtIP8 = new CustomControls.textBoxText();
            this.btnConectar8 = new System.Windows.Forms.Button();
            this.ckbTCP7 = new CustomControls.checkBoxNew();
            this.gb7 = new System.Windows.Forms.GroupBox();
            this.txtPuerto7 = new CustomControls.textBoxNum();
            this.cmbReceptora7 = new CustomControls.ComboBoxNew();
            this.btnLog7 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtIP7 = new CustomControls.textBoxText();
            this.btnConectar7 = new System.Windows.Forms.Button();
            this.ckbTCP6 = new CustomControls.checkBoxNew();
            this.ckbTCP3 = new CustomControls.checkBoxNew();
            this.gb6 = new System.Windows.Forms.GroupBox();
            this.btnLog6 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPuerto6 = new CustomControls.textBoxNum();
            this.cmbReceptora6 = new CustomControls.ComboBoxNew();
            this.btnConectar6 = new System.Windows.Forms.Button();
            this.txtIP6 = new CustomControls.textBoxText();
            this.gb3 = new System.Windows.Forms.GroupBox();
            this.btnLog3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPuerto3 = new CustomControls.textBoxNum();
            this.cmbReceptora3 = new CustomControls.ComboBoxNew();
            this.txtIP3 = new CustomControls.textBoxText();
            this.btnConectar3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.ckbTCP5 = new CustomControls.checkBoxNew();
            this.ckbTCP2 = new CustomControls.checkBoxNew();
            this.gb5 = new System.Windows.Forms.GroupBox();
            this.btnLog5 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPuerto5 = new CustomControls.textBoxNum();
            this.cmbReceptora5 = new CustomControls.ComboBoxNew();
            this.txtIP5 = new CustomControls.textBoxText();
            this.btnConectar5 = new System.Windows.Forms.Button();
            this.gb2 = new System.Windows.Forms.GroupBox();
            this.btnLog2 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbReceptora2 = new CustomControls.ComboBoxNew();
            this.txtPuerto2 = new CustomControls.textBoxNum();
            this.txtIP2 = new CustomControls.textBoxText();
            this.btnConectar2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ckbTCP4 = new CustomControls.checkBoxNew();
            this.ckbTCP1 = new CustomControls.checkBoxNew();
            this.gb4 = new System.Windows.Forms.GroupBox();
            this.btnLog4 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtPuerto4 = new CustomControls.textBoxNum();
            this.cmbReceptora4 = new CustomControls.ComboBoxNew();
            this.btnConectar4 = new System.Windows.Forms.Button();
            this.txtIP4 = new CustomControls.textBoxText();
            this.gb1 = new System.Windows.Forms.GroupBox();
            this.btnLog1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPuerto1 = new CustomControls.textBoxNum();
            this.cmbReceptora1 = new CustomControls.ComboBoxNew();
            this.btnConectar1 = new System.Windows.Forms.Button();
            this.txtIP1 = new CustomControls.textBoxText();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkedListBox8 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox7 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox6 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox5 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox4 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox3 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gb8.SuspendLayout();
            this.gb7.SuspendLayout();
            this.gb6.SuspendLayout();
            this.gb3.SuspendLayout();
            this.gb5.SuspendLayout();
            this.gb2.SuspendLayout();
            this.gb4.SuspendLayout();
            this.gb1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSalir
            // 
            this.btnSalir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSalir.BackColor = System.Drawing.SystemColors.Control;
            this.btnSalir.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSalir.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalir.ForeColor = System.Drawing.Color.Black;
            this.btnSalir.Location = new System.Drawing.Point(1592, 4);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(88, 28);
            this.btnSalir.TabIndex = 2;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = false;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // btnBorrar
            // 
            this.btnBorrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBorrar.BackColor = System.Drawing.SystemColors.Control;
            this.btnBorrar.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
            this.btnBorrar.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBorrar.ForeColor = System.Drawing.Color.Black;
            this.btnBorrar.Location = new System.Drawing.Point(1150, 4);
            this.btnBorrar.Name = "btnBorrar";
            this.btnBorrar.Size = new System.Drawing.Size(142, 28);
            this.btnBorrar.TabIndex = 0;
            this.btnBorrar.Text = "&Borrar Parametros";
            this.btnBorrar.UseVisualStyleBackColor = false;
            this.btnBorrar.Click += new System.EventHandler(this.btnBorrar_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // toolTip2
            // 
            this.toolTip2.ShowAlways = true;
            // 
            // toolTip3
            // 
            this.toolTip3.ShowAlways = true;
            // 
            // toolTip4
            // 
            this.toolTip4.ShowAlways = true;
            // 
            // toolTip5
            // 
            this.toolTip5.ShowAlways = true;
            // 
            // toolTip6
            // 
            this.toolTip6.ShowAlways = true;
            // 
            // btnSendAck
            // 
            this.btnSendAck.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnSendAck.BackColor = System.Drawing.SystemColors.Control;
            this.btnSendAck.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnSendAck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendAck.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendAck.ForeColor = System.Drawing.Color.Black;
            this.btnSendAck.Location = new System.Drawing.Point(1298, 4);
            this.btnSendAck.Name = "btnSendAck";
            this.btnSendAck.Size = new System.Drawing.Size(86, 28);
            this.btnSendAck.TabIndex = 21;
            this.btnSendAck.Text = "Send Ack";
            this.btnSendAck.UseVisualStyleBackColor = false;
            this.btnSendAck.Click += new System.EventHandler(this.btnSendAck_Click);
            // 
            // toolTip7
            // 
            this.toolTip7.ShowAlways = true;
            // 
            // toolTip8
            // 
            this.toolTip8.ShowAlways = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Controls.Add(this.panel3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1694, 836);
            this.flowLayoutPanel1.TabIndex = 24;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(5);
            this.panel1.Size = new System.Drawing.Size(1688, 149);
            this.panel1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Controls.Add(this.ckbTCP8);
            this.groupBox1.Controls.Add(this.gb8);
            this.groupBox1.Controls.Add(this.ckbTCP7);
            this.groupBox1.Controls.Add(this.gb7);
            this.groupBox1.Controls.Add(this.ckbTCP6);
            this.groupBox1.Controls.Add(this.ckbTCP3);
            this.groupBox1.Controls.Add(this.gb6);
            this.groupBox1.Controls.Add(this.gb3);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.ckbTCP5);
            this.groupBox1.Controls.Add(this.ckbTCP2);
            this.groupBox1.Controls.Add(this.gb5);
            this.groupBox1.Controls.Add(this.gb2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ckbTCP4);
            this.groupBox1.Controls.Add(this.ckbTCP1);
            this.groupBox1.Controls.Add(this.gb4);
            this.groupBox1.Controls.Add(this.gb1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(5, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1678, 139);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP´s";
            // 
            // ckbTCP8
            // 
            this.ckbTCP8.AutoSize = true;
            this.ckbTCP8.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP8.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP8.FlatAppearance.BorderSize = 5;
            this.ckbTCP8.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP8.ForeColor = System.Drawing.Color.White;
            this.ckbTCP8.Location = new System.Drawing.Point(1268, 89);
            this.ckbTCP8.Name = "ckbTCP8";
            this.ckbTCP8.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP8.TabIndex = 39;
            this.ckbTCP8.UseVisualStyleBackColor = false;
            this.ckbTCP8.CheckedChanged += new System.EventHandler(this.ckbTCP8_CheckedChanged);
            // 
            // gb8
            // 
            this.gb8.Controls.Add(this.txtPuerto8);
            this.gb8.Controls.Add(this.cmbReceptora8);
            this.gb8.Controls.Add(this.btnLog8);
            this.gb8.Controls.Add(this.label11);
            this.gb8.Controls.Add(this.txtIP8);
            this.gb8.Controls.Add(this.btnConectar8);
            this.gb8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb8.ForeColor = System.Drawing.Color.Yellow;
            this.gb8.Location = new System.Drawing.Point(1286, 65);
            this.gb8.Name = "gb8";
            this.gb8.Size = new System.Drawing.Size(386, 55);
            this.gb8.TabIndex = 38;
            this.gb8.TabStop = false;
            // 
            // txtPuerto8
            // 
            this.txtPuerto8.AcceptsNegative = false;
            this.txtPuerto8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto8.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto8.Mask = "";
            this.txtPuerto8.Name = "txtPuerto8";
            this.txtPuerto8.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto8.TabIndex = 29;
            // 
            // cmbReceptora8
            // 
            this.cmbReceptora8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora8.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora8.FormattingEnabled = true;
            this.cmbReceptora8.Location = new System.Drawing.Point(6, 25);
            this.cmbReceptora8.Name = "cmbReceptora8";
            this.cmbReceptora8.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora8.TabIndex = 28;
            this.cmbReceptora8.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora8_SelectedIndexChanged);
            // 
            // btnLog8
            // 
            this.btnLog8.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog8.FlatAppearance.BorderSize = 0;
            this.btnLog8.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog8.Location = new System.Drawing.Point(357, 22);
            this.btnLog8.Name = "btnLog8";
            this.btnLog8.Size = new System.Drawing.Size(27, 24);
            this.btnLog8.TabIndex = 27;
            this.btnLog8.UseVisualStyleBackColor = true;
            this.btnLog8.Click += new System.EventHandler(this.btnLog8_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.ForeColor = System.Drawing.Color.Aqua;
            this.label11.Location = new System.Drawing.Point(142, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(64, 13);
            this.label11.TabIndex = 4;
            this.label11.Text = "Equipo 16";
            // 
            // txtIP8
            // 
            this.txtIP8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP8.ForeColor = System.Drawing.Color.Black;
            this.txtIP8.Location = new System.Drawing.Point(123, 26);
            this.txtIP8.Name = "txtIP8";
            this.txtIP8.Size = new System.Drawing.Size(93, 20);
            this.txtIP8.TabIndex = 0;
            // 
            // btnConectar8
            // 
            this.btnConectar8.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar8.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar8.ForeColor = System.Drawing.Color.White;
            this.btnConectar8.Location = new System.Drawing.Point(266, 23);
            this.btnConectar8.Name = "btnConectar8";
            this.btnConectar8.Size = new System.Drawing.Size(89, 25);
            this.btnConectar8.TabIndex = 2;
            this.btnConectar8.Text = "Conectar";
            this.btnConectar8.UseVisualStyleBackColor = false;
            this.btnConectar8.Click += new System.EventHandler(this.btnConectar8_Click);
            // 
            // ckbTCP7
            // 
            this.ckbTCP7.AutoSize = true;
            this.ckbTCP7.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP7.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP7.FlatAppearance.BorderSize = 5;
            this.ckbTCP7.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP7.ForeColor = System.Drawing.Color.White;
            this.ckbTCP7.Location = new System.Drawing.Point(851, 91);
            this.ckbTCP7.Name = "ckbTCP7";
            this.ckbTCP7.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP7.TabIndex = 37;
            this.ckbTCP7.UseVisualStyleBackColor = false;
            this.ckbTCP7.CheckedChanged += new System.EventHandler(this.ckbTCP7_CheckedChanged);
            // 
            // gb7
            // 
            this.gb7.Controls.Add(this.txtPuerto7);
            this.gb7.Controls.Add(this.cmbReceptora7);
            this.gb7.Controls.Add(this.btnLog7);
            this.gb7.Controls.Add(this.label10);
            this.gb7.Controls.Add(this.txtIP7);
            this.gb7.Controls.Add(this.btnConectar7);
            this.gb7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb7.ForeColor = System.Drawing.Color.Yellow;
            this.gb7.Location = new System.Drawing.Point(869, 65);
            this.gb7.Name = "gb7";
            this.gb7.Size = new System.Drawing.Size(386, 55);
            this.gb7.TabIndex = 28;
            this.gb7.TabStop = false;
            // 
            // txtPuerto7
            // 
            this.txtPuerto7.AcceptsNegative = false;
            this.txtPuerto7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto7.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto7.Mask = "";
            this.txtPuerto7.Name = "txtPuerto7";
            this.txtPuerto7.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto7.TabIndex = 30;
            // 
            // cmbReceptora7
            // 
            this.cmbReceptora7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora7.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora7.FormattingEnabled = true;
            this.cmbReceptora7.Location = new System.Drawing.Point(6, 25);
            this.cmbReceptora7.Name = "cmbReceptora7";
            this.cmbReceptora7.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora7.TabIndex = 29;
            this.cmbReceptora7.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora7_SelectedIndexChanged);
            // 
            // btnLog7
            // 
            this.btnLog7.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog7.FlatAppearance.BorderSize = 0;
            this.btnLog7.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog7.Location = new System.Drawing.Point(357, 22);
            this.btnLog7.Name = "btnLog7";
            this.btnLog7.Size = new System.Drawing.Size(27, 24);
            this.btnLog7.TabIndex = 27;
            this.btnLog7.UseVisualStyleBackColor = true;
            this.btnLog7.Click += new System.EventHandler(this.btnLog7_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.ForeColor = System.Drawing.Color.Aqua;
            this.label10.Location = new System.Drawing.Point(142, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Equipo 15";
            // 
            // txtIP7
            // 
            this.txtIP7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP7.ForeColor = System.Drawing.Color.Black;
            this.txtIP7.Location = new System.Drawing.Point(123, 26);
            this.txtIP7.Name = "txtIP7";
            this.txtIP7.Size = new System.Drawing.Size(93, 20);
            this.txtIP7.TabIndex = 0;
            // 
            // btnConectar7
            // 
            this.btnConectar7.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar7.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar7.ForeColor = System.Drawing.Color.White;
            this.btnConectar7.Location = new System.Drawing.Point(266, 23);
            this.btnConectar7.Name = "btnConectar7";
            this.btnConectar7.Size = new System.Drawing.Size(89, 25);
            this.btnConectar7.TabIndex = 2;
            this.btnConectar7.Text = "Conectar";
            this.btnConectar7.UseVisualStyleBackColor = false;
            this.btnConectar7.Click += new System.EventHandler(this.btnConectar7_Click);
            // 
            // ckbTCP6
            // 
            this.ckbTCP6.AutoSize = true;
            this.ckbTCP6.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP6.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP6.FlatAppearance.BorderSize = 5;
            this.ckbTCP6.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP6.ForeColor = System.Drawing.Color.White;
            this.ckbTCP6.Location = new System.Drawing.Point(435, 88);
            this.ckbTCP6.Name = "ckbTCP6";
            this.ckbTCP6.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP6.TabIndex = 11;
            this.ckbTCP6.UseVisualStyleBackColor = false;
            this.ckbTCP6.CheckedChanged += new System.EventHandler(this.ckbTCP6_CheckedChanged);
            // 
            // ckbTCP3
            // 
            this.ckbTCP3.AutoSize = true;
            this.ckbTCP3.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP3.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP3.FlatAppearance.BorderSize = 5;
            this.ckbTCP3.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP3.ForeColor = System.Drawing.Color.White;
            this.ckbTCP3.Location = new System.Drawing.Point(853, 34);
            this.ckbTCP3.Name = "ckbTCP3";
            this.ckbTCP3.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP3.TabIndex = 9;
            this.ckbTCP3.UseVisualStyleBackColor = false;
            this.ckbTCP3.CheckedChanged += new System.EventHandler(this.ckbTCP3_CheckedChanged);
            // 
            // gb6
            // 
            this.gb6.Controls.Add(this.btnLog6);
            this.gb6.Controls.Add(this.label9);
            this.gb6.Controls.Add(this.txtPuerto6);
            this.gb6.Controls.Add(this.cmbReceptora6);
            this.gb6.Controls.Add(this.btnConectar6);
            this.gb6.Controls.Add(this.txtIP6);
            this.gb6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb6.ForeColor = System.Drawing.Color.Yellow;
            this.gb6.Location = new System.Drawing.Point(450, 65);
            this.gb6.Name = "gb6";
            this.gb6.Size = new System.Drawing.Size(386, 55);
            this.gb6.TabIndex = 10;
            this.gb6.TabStop = false;
            // 
            // btnLog6
            // 
            this.btnLog6.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog6.FlatAppearance.BorderSize = 0;
            this.btnLog6.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog6.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog6.Location = new System.Drawing.Point(357, 23);
            this.btnLog6.Name = "btnLog6";
            this.btnLog6.Size = new System.Drawing.Size(27, 24);
            this.btnLog6.TabIndex = 27;
            this.btnLog6.UseVisualStyleBackColor = true;
            this.btnLog6.Click += new System.EventHandler(this.btnLog6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.ForeColor = System.Drawing.Color.Aqua;
            this.label9.Location = new System.Drawing.Point(152, 8);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Equipo 14";
            // 
            // txtPuerto6
            // 
            this.txtPuerto6.AcceptsNegative = false;
            this.txtPuerto6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto6.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto6.Mask = "";
            this.txtPuerto6.Name = "txtPuerto6";
            this.txtPuerto6.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto6.TabIndex = 2;
            // 
            // cmbReceptora6
            // 
            this.cmbReceptora6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora6.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora6.FormattingEnabled = true;
            this.cmbReceptora6.Location = new System.Drawing.Point(6, 24);
            this.cmbReceptora6.Name = "cmbReceptora6";
            this.cmbReceptora6.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora6.TabIndex = 0;
            this.cmbReceptora6.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora6_SelectedIndexChanged);
            // 
            // btnConectar6
            // 
            this.btnConectar6.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar6.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar6.ForeColor = System.Drawing.Color.White;
            this.btnConectar6.Location = new System.Drawing.Point(266, 23);
            this.btnConectar6.Name = "btnConectar6";
            this.btnConectar6.Size = new System.Drawing.Size(89, 25);
            this.btnConectar6.TabIndex = 3;
            this.btnConectar6.Text = "Conectar";
            this.btnConectar6.UseVisualStyleBackColor = false;
            this.btnConectar6.Click += new System.EventHandler(this.btnConectar6_Click);
            // 
            // txtIP6
            // 
            this.txtIP6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP6.ForeColor = System.Drawing.Color.Black;
            this.txtIP6.Location = new System.Drawing.Point(123, 25);
            this.txtIP6.Name = "txtIP6";
            this.txtIP6.Size = new System.Drawing.Size(93, 20);
            this.txtIP6.TabIndex = 1;
            // 
            // gb3
            // 
            this.gb3.Controls.Add(this.btnLog3);
            this.gb3.Controls.Add(this.label6);
            this.gb3.Controls.Add(this.txtPuerto3);
            this.gb3.Controls.Add(this.cmbReceptora3);
            this.gb3.Controls.Add(this.txtIP3);
            this.gb3.Controls.Add(this.btnConectar3);
            this.gb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb3.ForeColor = System.Drawing.Color.Yellow;
            this.gb3.Location = new System.Drawing.Point(869, 10);
            this.gb3.Name = "gb3";
            this.gb3.Size = new System.Drawing.Size(386, 55);
            this.gb3.TabIndex = 8;
            this.gb3.TabStop = false;
            // 
            // btnLog3
            // 
            this.btnLog3.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog3.FlatAppearance.BorderSize = 0;
            this.btnLog3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog3.Location = new System.Drawing.Point(357, 22);
            this.btnLog3.Name = "btnLog3";
            this.btnLog3.Size = new System.Drawing.Size(27, 24);
            this.btnLog3.TabIndex = 27;
            this.btnLog3.UseVisualStyleBackColor = true;
            this.btnLog3.Click += new System.EventHandler(this.btnLog3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.Aqua;
            this.label6.Location = new System.Drawing.Point(152, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Equipo 11";
            // 
            // txtPuerto3
            // 
            this.txtPuerto3.AcceptsNegative = false;
            this.txtPuerto3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto3.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto3.Mask = "";
            this.txtPuerto3.Name = "txtPuerto3";
            this.txtPuerto3.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto3.TabIndex = 2;
            // 
            // cmbReceptora3
            // 
            this.cmbReceptora3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora3.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora3.FormattingEnabled = true;
            this.cmbReceptora3.Location = new System.Drawing.Point(6, 24);
            this.cmbReceptora3.Name = "cmbReceptora3";
            this.cmbReceptora3.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora3.TabIndex = 0;
            this.cmbReceptora3.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora3_SelectedIndexChanged);
            // 
            // txtIP3
            // 
            this.txtIP3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP3.ForeColor = System.Drawing.Color.Black;
            this.txtIP3.Location = new System.Drawing.Point(123, 25);
            this.txtIP3.Name = "txtIP3";
            this.txtIP3.Size = new System.Drawing.Size(93, 20);
            this.txtIP3.TabIndex = 1;
            // 
            // btnConectar3
            // 
            this.btnConectar3.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar3.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar3.ForeColor = System.Drawing.Color.White;
            this.btnConectar3.Location = new System.Drawing.Point(266, 23);
            this.btnConectar3.Name = "btnConectar3";
            this.btnConectar3.Size = new System.Drawing.Size(89, 25);
            this.btnConectar3.TabIndex = 3;
            this.btnConectar3.Text = "Conectar";
            this.btnConectar3.UseVisualStyleBackColor = false;
            this.btnConectar3.Click += new System.EventHandler(this.btnConectar3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(852, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(11, 13);
            this.label3.TabIndex = 36;
            this.label3.Text = ".";
            this.label3.Visible = false;
            // 
            // ckbTCP5
            // 
            this.ckbTCP5.AutoSize = true;
            this.ckbTCP5.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP5.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP5.FlatAppearance.BorderSize = 5;
            this.ckbTCP5.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP5.ForeColor = System.Drawing.Color.White;
            this.ckbTCP5.Location = new System.Drawing.Point(17, 88);
            this.ckbTCP5.Name = "ckbTCP5";
            this.ckbTCP5.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP5.TabIndex = 7;
            this.ckbTCP5.UseVisualStyleBackColor = false;
            this.ckbTCP5.CheckedChanged += new System.EventHandler(this.ckbTCP5_CheckedChanged);
            // 
            // ckbTCP2
            // 
            this.ckbTCP2.AutoSize = true;
            this.ckbTCP2.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP2.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP2.FlatAppearance.BorderSize = 5;
            this.ckbTCP2.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP2.ForeColor = System.Drawing.Color.White;
            this.ckbTCP2.Location = new System.Drawing.Point(435, 34);
            this.ckbTCP2.Name = "ckbTCP2";
            this.ckbTCP2.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP2.TabIndex = 5;
            this.ckbTCP2.UseVisualStyleBackColor = false;
            this.ckbTCP2.CheckedChanged += new System.EventHandler(this.ckbTCP2_CheckedChanged);
            // 
            // gb5
            // 
            this.gb5.Controls.Add(this.btnLog5);
            this.gb5.Controls.Add(this.label8);
            this.gb5.Controls.Add(this.txtPuerto5);
            this.gb5.Controls.Add(this.cmbReceptora5);
            this.gb5.Controls.Add(this.txtIP5);
            this.gb5.Controls.Add(this.btnConectar5);
            this.gb5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb5.ForeColor = System.Drawing.Color.Yellow;
            this.gb5.Location = new System.Drawing.Point(32, 65);
            this.gb5.Name = "gb5";
            this.gb5.Size = new System.Drawing.Size(386, 55);
            this.gb5.TabIndex = 6;
            this.gb5.TabStop = false;
            // 
            // btnLog5
            // 
            this.btnLog5.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog5.FlatAppearance.BorderSize = 0;
            this.btnLog5.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog5.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog5.Location = new System.Drawing.Point(357, 23);
            this.btnLog5.Name = "btnLog5";
            this.btnLog5.Size = new System.Drawing.Size(27, 24);
            this.btnLog5.TabIndex = 27;
            this.btnLog5.UseVisualStyleBackColor = true;
            this.btnLog5.Click += new System.EventHandler(this.btnLog5_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.Aqua;
            this.label8.Location = new System.Drawing.Point(152, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Equipo 13";
            // 
            // txtPuerto5
            // 
            this.txtPuerto5.AcceptsNegative = false;
            this.txtPuerto5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto5.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto5.Mask = "";
            this.txtPuerto5.Name = "txtPuerto5";
            this.txtPuerto5.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto5.TabIndex = 2;
            // 
            // cmbReceptora5
            // 
            this.cmbReceptora5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora5.FormattingEnabled = true;
            this.cmbReceptora5.Location = new System.Drawing.Point(6, 24);
            this.cmbReceptora5.Name = "cmbReceptora5";
            this.cmbReceptora5.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora5.TabIndex = 0;
            this.cmbReceptora5.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora5_SelectedIndexChanged);
            // 
            // txtIP5
            // 
            this.txtIP5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP5.Location = new System.Drawing.Point(123, 25);
            this.txtIP5.Name = "txtIP5";
            this.txtIP5.Size = new System.Drawing.Size(93, 20);
            this.txtIP5.TabIndex = 1;
            // 
            // btnConectar5
            // 
            this.btnConectar5.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar5.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar5.ForeColor = System.Drawing.Color.White;
            this.btnConectar5.Location = new System.Drawing.Point(266, 23);
            this.btnConectar5.Name = "btnConectar5";
            this.btnConectar5.Size = new System.Drawing.Size(89, 25);
            this.btnConectar5.TabIndex = 3;
            this.btnConectar5.Text = "Conectar";
            this.btnConectar5.UseVisualStyleBackColor = false;
            this.btnConectar5.Click += new System.EventHandler(this.btnConectar5_Click);
            // 
            // gb2
            // 
            this.gb2.Controls.Add(this.btnLog2);
            this.gb2.Controls.Add(this.label5);
            this.gb2.Controls.Add(this.cmbReceptora2);
            this.gb2.Controls.Add(this.txtPuerto2);
            this.gb2.Controls.Add(this.txtIP2);
            this.gb2.Controls.Add(this.btnConectar2);
            this.gb2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb2.ForeColor = System.Drawing.Color.Yellow;
            this.gb2.Location = new System.Drawing.Point(450, 10);
            this.gb2.Name = "gb2";
            this.gb2.Size = new System.Drawing.Size(386, 55);
            this.gb2.TabIndex = 4;
            this.gb2.TabStop = false;
            // 
            // btnLog2
            // 
            this.btnLog2.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog2.FlatAppearance.BorderSize = 0;
            this.btnLog2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog2.Location = new System.Drawing.Point(357, 23);
            this.btnLog2.Name = "btnLog2";
            this.btnLog2.Size = new System.Drawing.Size(27, 24);
            this.btnLog2.TabIndex = 26;
            this.btnLog2.UseVisualStyleBackColor = true;
            this.btnLog2.Click += new System.EventHandler(this.btnLog2_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.Aqua;
            this.label5.Location = new System.Drawing.Point(152, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Equipo 10";
            // 
            // cmbReceptora2
            // 
            this.cmbReceptora2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora2.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora2.FormattingEnabled = true;
            this.cmbReceptora2.Location = new System.Drawing.Point(6, 24);
            this.cmbReceptora2.Name = "cmbReceptora2";
            this.cmbReceptora2.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora2.TabIndex = 0;
            this.cmbReceptora2.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora2_SelectedIndexChanged);
            // 
            // txtPuerto2
            // 
            this.txtPuerto2.AcceptsNegative = false;
            this.txtPuerto2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto2.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto2.Mask = "";
            this.txtPuerto2.Name = "txtPuerto2";
            this.txtPuerto2.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto2.TabIndex = 2;
            // 
            // txtIP2
            // 
            this.txtIP2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP2.ForeColor = System.Drawing.Color.Black;
            this.txtIP2.Location = new System.Drawing.Point(123, 25);
            this.txtIP2.Name = "txtIP2";
            this.txtIP2.Size = new System.Drawing.Size(93, 20);
            this.txtIP2.TabIndex = 1;
            // 
            // btnConectar2
            // 
            this.btnConectar2.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar2.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar2.ForeColor = System.Drawing.Color.White;
            this.btnConectar2.Location = new System.Drawing.Point(266, 23);
            this.btnConectar2.Name = "btnConectar2";
            this.btnConectar2.Size = new System.Drawing.Size(89, 25);
            this.btnConectar2.TabIndex = 3;
            this.btnConectar2.Text = "Conectar";
            this.btnConectar2.UseVisualStyleBackColor = false;
            this.btnConectar2.Click += new System.EventHandler(this.btnConectar2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(440, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(11, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = ".";
            this.label2.Visible = false;
            // 
            // ckbTCP4
            // 
            this.ckbTCP4.AutoSize = true;
            this.ckbTCP4.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP4.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
            this.ckbTCP4.FlatAppearance.BorderSize = 5;
            this.ckbTCP4.FlatAppearance.CheckedBackColor = System.Drawing.Color.Maroon;
            this.ckbTCP4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP4.ForeColor = System.Drawing.Color.White;
            this.ckbTCP4.Location = new System.Drawing.Point(1268, 34);
            this.ckbTCP4.Name = "ckbTCP4";
            this.ckbTCP4.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP4.TabIndex = 3;
            this.ckbTCP4.UseVisualStyleBackColor = false;
            this.ckbTCP4.CheckedChanged += new System.EventHandler(this.ckbTCP4_CheckedChanged);
            // 
            // ckbTCP1
            // 
            this.ckbTCP1.AutoSize = true;
            this.ckbTCP1.BackColor = System.Drawing.Color.DimGray;
            this.ckbTCP1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ckbTCP1.ForeColor = System.Drawing.Color.White;
            this.ckbTCP1.Location = new System.Drawing.Point(15, 34);
            this.ckbTCP1.Name = "ckbTCP1";
            this.ckbTCP1.Size = new System.Drawing.Size(12, 11);
            this.ckbTCP1.TabIndex = 1;
            this.ckbTCP1.UseVisualStyleBackColor = false;
            this.ckbTCP1.CheckedChanged += new System.EventHandler(this.ckbTCP1_CheckedChanged);
            // 
            // gb4
            // 
            this.gb4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gb4.Controls.Add(this.btnLog4);
            this.gb4.Controls.Add(this.label7);
            this.gb4.Controls.Add(this.txtPuerto4);
            this.gb4.Controls.Add(this.cmbReceptora4);
            this.gb4.Controls.Add(this.btnConectar4);
            this.gb4.Controls.Add(this.txtIP4);
            this.gb4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb4.ForeColor = System.Drawing.Color.Yellow;
            this.gb4.Location = new System.Drawing.Point(1286, 10);
            this.gb4.Name = "gb4";
            this.gb4.Size = new System.Drawing.Size(383, 55);
            this.gb4.TabIndex = 2;
            this.gb4.TabStop = false;
            // 
            // btnLog4
            // 
            this.btnLog4.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog4.FlatAppearance.BorderSize = 0;
            this.btnLog4.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog4.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog4.Location = new System.Drawing.Point(357, 23);
            this.btnLog4.Name = "btnLog4";
            this.btnLog4.Size = new System.Drawing.Size(27, 24);
            this.btnLog4.TabIndex = 26;
            this.btnLog4.UseVisualStyleBackColor = true;
            this.btnLog4.Click += new System.EventHandler(this.btnLog4_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.label7.ForeColor = System.Drawing.Color.Aqua;
            this.label7.Location = new System.Drawing.Point(152, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Equipo 12";
            // 
            // txtPuerto4
            // 
            this.txtPuerto4.AcceptsNegative = false;
            this.txtPuerto4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto4.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto4.Mask = "";
            this.txtPuerto4.Name = "txtPuerto4";
            this.txtPuerto4.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto4.TabIndex = 2;
            // 
            // cmbReceptora4
            // 
            this.cmbReceptora4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora4.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora4.FormattingEnabled = true;
            this.cmbReceptora4.Location = new System.Drawing.Point(6, 24);
            this.cmbReceptora4.Name = "cmbReceptora4";
            this.cmbReceptora4.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora4.TabIndex = 0;
            this.cmbReceptora4.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora4_SelectedIndexChanged);
            // 
            // btnConectar4
            // 
            this.btnConectar4.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar4.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar4.ForeColor = System.Drawing.Color.White;
            this.btnConectar4.Location = new System.Drawing.Point(266, 23);
            this.btnConectar4.Name = "btnConectar4";
            this.btnConectar4.Size = new System.Drawing.Size(89, 25);
            this.btnConectar4.TabIndex = 3;
            this.btnConectar4.Text = "Conectar";
            this.btnConectar4.UseVisualStyleBackColor = false;
            this.btnConectar4.Click += new System.EventHandler(this.btnConectar4_Click);
            // 
            // txtIP4
            // 
            this.txtIP4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP4.ForeColor = System.Drawing.Color.Black;
            this.txtIP4.Location = new System.Drawing.Point(123, 25);
            this.txtIP4.Name = "txtIP4";
            this.txtIP4.Size = new System.Drawing.Size(93, 20);
            this.txtIP4.TabIndex = 1;
            // 
            // gb1
            // 
            this.gb1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gb1.Controls.Add(this.btnLog1);
            this.gb1.Controls.Add(this.label4);
            this.gb1.Controls.Add(this.txtPuerto1);
            this.gb1.Controls.Add(this.cmbReceptora1);
            this.gb1.Controls.Add(this.btnConectar1);
            this.gb1.Controls.Add(this.txtIP1);
            this.gb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gb1.ForeColor = System.Drawing.Color.Yellow;
            this.gb1.Location = new System.Drawing.Point(30, 10);
            this.gb1.Name = "gb1";
            this.gb1.Size = new System.Drawing.Size(386, 55);
            this.gb1.TabIndex = 0;
            this.gb1.TabStop = false;
            // 
            // btnLog1
            // 
            this.btnLog1.BackgroundImage = global::MPSER.Properties.Resources.btnLog1_BackgroundImage;
            this.btnLog1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLog1.FlatAppearance.BorderSize = 0;
            this.btnLog1.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnLog1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnLog1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLog1.Location = new System.Drawing.Point(357, 23);
            this.btnLog1.Name = "btnLog1";
            this.btnLog1.Size = new System.Drawing.Size(27, 24);
            this.btnLog1.TabIndex = 25;
            this.btnLog1.UseVisualStyleBackColor = true;
            this.btnLog1.Click += new System.EventHandler(this.btnLog1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.Aqua;
            this.label4.Location = new System.Drawing.Point(151, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Equipo 9";
            // 
            // txtPuerto1
            // 
            this.txtPuerto1.AcceptsNegative = false;
            this.txtPuerto1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPuerto1.Location = new System.Drawing.Point(219, 25);
            this.txtPuerto1.Mask = "";
            this.txtPuerto1.Name = "txtPuerto1";
            this.txtPuerto1.Size = new System.Drawing.Size(41, 20);
            this.txtPuerto1.TabIndex = 2;
            // 
            // cmbReceptora1
            // 
            this.cmbReceptora1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbReceptora1.ForeColor = System.Drawing.Color.Black;
            this.cmbReceptora1.FormattingEnabled = true;
            this.cmbReceptora1.Location = new System.Drawing.Point(6, 24);
            this.cmbReceptora1.Name = "cmbReceptora1";
            this.cmbReceptora1.Size = new System.Drawing.Size(114, 21);
            this.cmbReceptora1.TabIndex = 0;
            this.cmbReceptora1.SelectedIndexChanged += new System.EventHandler(this.cmbReceptora1_SelectedIndexChanged);
            // 
            // btnConectar1
            // 
            this.btnConectar1.BackColor = System.Drawing.Color.Crimson;
            this.btnConectar1.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnConectar1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConectar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConectar1.ForeColor = System.Drawing.Color.White;
            this.btnConectar1.Location = new System.Drawing.Point(266, 23);
            this.btnConectar1.Name = "btnConectar1";
            this.btnConectar1.Size = new System.Drawing.Size(89, 25);
            this.btnConectar1.TabIndex = 3;
            this.btnConectar1.Text = "Conectar";
            this.btnConectar1.UseVisualStyleBackColor = false;
            this.btnConectar1.Click += new System.EventHandler(this.btnConectar1_Click);
            // 
            // txtIP1
            // 
            this.txtIP1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtIP1.ForeColor = System.Drawing.Color.Black;
            this.txtIP1.Location = new System.Drawing.Point(123, 25);
            this.txtIP1.Name = "txtIP1";
            this.txtIP1.Size = new System.Drawing.Size(93, 20);
            this.txtIP1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = ".";
            this.label1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 149);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(1688, 644);
            this.panel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.groupBox2.Controls.Add(this.checkedListBox8);
            this.groupBox2.Controls.Add(this.checkedListBox7);
            this.groupBox2.Controls.Add(this.checkedListBox6);
            this.groupBox2.Controls.Add(this.checkedListBox5);
            this.groupBox2.Controls.Add(this.checkedListBox4);
            this.groupBox2.Controls.Add(this.checkedListBox3);
            this.groupBox2.Controls.Add(this.checkedListBox2);
            this.groupBox2.Controls.Add(this.checkedListBox1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(5, 5);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(0);
            this.groupBox2.Size = new System.Drawing.Size(1678, 634);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detalle";
            // 
            // checkedListBox8
            // 
            this.checkedListBox8.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox8.FormattingEnabled = true;
            this.checkedListBox8.HorizontalScrollbar = true;
            this.checkedListBox8.Location = new System.Drawing.Point(1286, 330);
            this.checkedListBox8.Name = "checkedListBox8";
            this.checkedListBox8.ScrollAlwaysVisible = true;
            this.checkedListBox8.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox8.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox8.TabIndex = 7;
            this.checkedListBox8.Visible = false;
            // 
            // checkedListBox7
            // 
            this.checkedListBox7.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox7.FormattingEnabled = true;
            this.checkedListBox7.HorizontalScrollbar = true;
            this.checkedListBox7.Location = new System.Drawing.Point(867, 330);
            this.checkedListBox7.Name = "checkedListBox7";
            this.checkedListBox7.ScrollAlwaysVisible = true;
            this.checkedListBox7.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox7.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox7.TabIndex = 6;
            this.checkedListBox7.Visible = false;
            // 
            // checkedListBox6
            // 
            this.checkedListBox6.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox6.FormattingEnabled = true;
            this.checkedListBox6.HorizontalScrollbar = true;
            this.checkedListBox6.Location = new System.Drawing.Point(450, 330);
            this.checkedListBox6.Name = "checkedListBox6";
            this.checkedListBox6.ScrollAlwaysVisible = true;
            this.checkedListBox6.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox6.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox6.TabIndex = 5;
            this.checkedListBox6.Visible = false;
            // 
            // checkedListBox5
            // 
            this.checkedListBox5.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox5.FormattingEnabled = true;
            this.checkedListBox5.HorizontalScrollbar = true;
            this.checkedListBox5.Location = new System.Drawing.Point(30, 330);
            this.checkedListBox5.Name = "checkedListBox5";
            this.checkedListBox5.ScrollAlwaysVisible = true;
            this.checkedListBox5.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox5.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox5.TabIndex = 4;
            this.checkedListBox5.Visible = false;
            // 
            // checkedListBox4
            // 
            this.checkedListBox4.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox4.FormattingEnabled = true;
            this.checkedListBox4.HorizontalScrollbar = true;
            this.checkedListBox4.Location = new System.Drawing.Point(1286, 24);
            this.checkedListBox4.Name = "checkedListBox4";
            this.checkedListBox4.ScrollAlwaysVisible = true;
            this.checkedListBox4.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox4.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox4.TabIndex = 3;
            this.checkedListBox4.Visible = false;
            // 
            // checkedListBox3
            // 
            this.checkedListBox3.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox3.FormattingEnabled = true;
            this.checkedListBox3.HorizontalScrollbar = true;
            this.checkedListBox3.Location = new System.Drawing.Point(868, 24);
            this.checkedListBox3.Name = "checkedListBox3";
            this.checkedListBox3.ScrollAlwaysVisible = true;
            this.checkedListBox3.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox3.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox3.TabIndex = 2;
            this.checkedListBox3.Visible = false;
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.HorizontalScrollbar = true;
            this.checkedListBox2.Location = new System.Drawing.Point(450, 24);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.ScrollAlwaysVisible = true;
            this.checkedListBox2.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox2.Size = new System.Drawing.Size(386, 289);
            this.checkedListBox2.TabIndex = 1;
            this.checkedListBox2.Visible = false;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.checkedListBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkedListBox1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(30, 24);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox1.Size = new System.Drawing.Size(384, 289);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.panel3.Controls.Add(this.btnBorrar);
            this.panel3.Controls.Add(this.btnSendAck);
            this.panel3.Controls.Add(this.btnSalir);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(3, 796);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1685, 35);
            this.panel3.TabIndex = 4;
            // 
            // MPTCP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(74)))));
            this.ClientSize = new System.Drawing.Size(1694, 836);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MPTCP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monitor de Puertos IP SERVERS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MPTCP_FormClosing);
            this.Load += new System.EventHandler(this.MPTCP_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gb8.ResumeLayout(false);
            this.gb8.PerformLayout();
            this.gb7.ResumeLayout(false);
            this.gb7.PerformLayout();
            this.gb6.ResumeLayout(false);
            this.gb6.PerformLayout();
            this.gb3.ResumeLayout(false);
            this.gb3.PerformLayout();
            this.gb5.ResumeLayout(false);
            this.gb5.PerformLayout();
            this.gb2.ResumeLayout(false);
            this.gb2.PerformLayout();
            this.gb4.ResumeLayout(false);
            this.gb4.PerformLayout();
            this.gb1.ResumeLayout(false);
            this.gb1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox2;
        private CheckedListBox checkedListBox8;
        private CheckedListBox checkedListBox7;
        private CheckedListBox checkedListBox6;
        private CheckedListBox checkedListBox5;
        private CheckedListBox checkedListBox4;
        private CheckedListBox checkedListBox3;
        private CheckedListBox checkedListBox2;
        private CheckedListBox checkedListBox1;
        private checkBoxNew ckbTCP8;
        private GroupBox gb8;
        private textBoxNum txtPuerto8;
        private ComboBoxNew cmbReceptora8;
        private Button btnLog8;
        private Label label11;
        private textBoxText txtIP8;
        private Button btnConectar8;
        private checkBoxNew ckbTCP7;
        private GroupBox gb7;
        private textBoxNum txtPuerto7;
        private ComboBoxNew cmbReceptora7;
        private Button btnLog7;
        private Label label10;
        private textBoxText txtIP7;
        private Button btnConectar7;
        private checkBoxNew ckbTCP6;
        private checkBoxNew ckbTCP3;
        private GroupBox gb6;
        private Button btnLog6;
        private Label label9;
        private textBoxNum txtPuerto6;
        private ComboBoxNew cmbReceptora6;
        private Button btnConectar6;
        private textBoxText txtIP6;
        private GroupBox gb3;
        private Button btnLog3;
        private Label label6;
        private textBoxNum txtPuerto3;
        private ComboBoxNew cmbReceptora3;
        private textBoxText txtIP3;
        private Button btnConectar3;
        private Label label3;
        private checkBoxNew ckbTCP5;
        private checkBoxNew ckbTCP2;
        private GroupBox gb5;
        private Button btnLog5;
        private Label label8;
        private textBoxNum txtPuerto5;
        private ComboBoxNew cmbReceptora5;
        private textBoxText txtIP5;
        private Button btnConectar5;
        private GroupBox gb2;
        private Button btnLog2;
        private Label label5;
        private ComboBoxNew cmbReceptora2;
        private textBoxNum txtPuerto2;
        private textBoxText txtIP2;
        private Button btnConectar2;
        private Label label2;
        private checkBoxNew ckbTCP4;
        private checkBoxNew ckbTCP1;
        private GroupBox gb4;
        private Button btnLog4;
        private Label label7;
        private textBoxNum txtPuerto4;
        private ComboBoxNew cmbReceptora4;
        private Button btnConectar4;
        private textBoxText txtIP4;
        private GroupBox gb1;
        private Button btnLog1;
        private Label label4;
        private textBoxNum txtPuerto1;
        private ComboBoxNew cmbReceptora1;
        private Button btnConectar1;
        private textBoxText txtIP1;
        private Label label1;
        private GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
    }
}