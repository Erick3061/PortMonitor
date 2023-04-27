using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MPSER
{
    public class TcpClients
    {
        private _MPCLogica cLogica = new _MPCLogica();

        public string Connect(string server, string message, int port)
        {
            string str = "";
            bool flag = false;
            byte num1 = 0;
            byte num2;
            while (num1 < (byte)3)
            {
                try
                {
                    TcpClient tcpClient = new TcpClientWithTimeout(server, port, 2000).Connect();
                    NetworkStream stream = tcpClient.GetStream();
                    tcpClient.ReceiveTimeout = 1000;
                    tcpClient.SendTimeout = 1000;
                    byte[] bytes = Encoding.ASCII.GetBytes(message);
                    stream.Write(bytes, 0, bytes.Length);
                    byte[] numArray = new byte[256];
                    string empty = string.Empty;
                    stream.ReadTimeout = 2000;
                    int count = stream.Read(numArray, 0, numArray.Length);
                    empty = Encoding.ASCII.GetString(numArray, 0, count);
                    stream.Close();
                    tcpClient.Close();
                    num1 = (byte)3;
                }
                catch (ArgumentNullException ex)
                {
                    this.cLogica.RegistroBitacora2("MPC", ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ")), ex.HResult, ex.Message + " 1* --> IP: " + server + " Puerto: " + (object)port + " " + message + " Tu dirección IP es: " + this.LocalIPAddress());
                    flag = true;
                    num1 = (byte)3;
                    str = ex.Message;
                }
                catch (SocketException ex)
                {
                    num2 = (byte)((uint)num1 + 1U);
                    num1 = (byte)3;
                    if (num1 >= (byte)3)
                    {
                        flag = true;
                        str = ex.Message;
                    }
                }
                catch (IOException ex)
                {
                    num2 = (byte)((uint)num1 + 1U);
                    num1 = (byte)3;
                    if (num1 >= (byte)3)
                    {
                        flag = true;
                        str = ex.Message;
                    }
                }
                catch (TimeoutException ex)
                {
                    this.cLogica.RegistroBitacora2("MPC", ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ")), ex.HResult, ex.Message + " 5* --> IP: " + server + " Puerto: " + (object)port + " " + message + " Tu dirección IP es: " + this.LocalIPAddress());
                    flag = true;
                    num1 = (byte)3;
                }
                catch (Exception ex)
                {
                    this.cLogica.RegistroBitacora2("MPC", ex.TargetSite.DeclaringType.ToString(), ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(" en ")), ex.HResult, ex.Message + " 4* --> IP: " + server + " Puerto: " + (object)port + " " + message + " Tu dirección IP es: " + this.LocalIPAddress());
                    flag = true;
                    num1 = (byte)3;
                    str = ex.Message;
                }
            }
            if (flag)
                ;
            return str;
        }

        public string LocalIPAddress()
        {
            string str = "";
            foreach (IPAddress address in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (address.AddressFamily.ToString() == "InterNetwork")
                    str = address.ToString();
            }
            return str;
        }
    }

}
