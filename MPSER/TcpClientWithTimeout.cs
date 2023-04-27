using System;
using System.Net.Sockets;
using System.Threading;
namespace MPSER
{
    public class TcpClientWithTimeout
    {
        protected string _hostname;
        protected int _port;
        protected int _timeout_milliseconds;
        protected TcpClient connection;
        protected bool connected;
        protected Exception exception;

        public TcpClientWithTimeout(string hostname, int port, int timeout_milliseconds)
        {
            this._hostname = hostname;
            this._port = port;
            this._timeout_milliseconds = timeout_milliseconds;
        }

        public TcpClient Connect()
        {
            this.connected = false;
            this.exception = (Exception)null;
            Thread thread = new Thread(new ThreadStart(this.BeginConnect));
            thread.IsBackground = true;
            thread.Start();
            thread.Join(this._timeout_milliseconds);
            if (this.connected)
            {
                thread.Abort();
                return this.connection;
            }
            if (this.exception != null)
            {
                thread.Abort();
                throw this.exception;
            }
            thread.Abort();
            throw new TimeoutException(string.Format("TcpClient connection to {0}:{1} timed out " + this.exception.Message, (object)this._hostname, (object)this._port));
        }

        protected void BeginConnect()
        {
            try
            {
                this.connection = new TcpClient(this._hostname, this._port);
                this.connected = true;
            }
            catch (Exception ex)
            {
                this.exception = ex;
            }
        }
    }

}
