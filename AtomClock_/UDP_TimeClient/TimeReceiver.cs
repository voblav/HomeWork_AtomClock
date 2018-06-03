using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDP_Time_Client
{
    public class TimeReceiver
    {
        static string remoteAddress = "192.168.1.102";
        static int remotePort = 1001;
        static int localPort = 1000;

        public static DateTime GetTime()
        {
            SendRequest();
            return ReceiveRequest();
        }

        static void SendRequest()
        {
            using (UdpClient sender = new UdpClient())
            {
                string message = "time";
                byte[] buffer = Encoding.ASCII.GetBytes(message);
                sender.Send(buffer, buffer.Length, remoteAddress, remotePort);
            }
        }

        static DateTime ReceiveRequest()
        {
            using (UdpClient receiver = new UdpClient(localPort))
            {
                IPEndPoint remoteIp = null;

                byte[] buffer = receiver.Receive(ref remoteIp);
                string time = Encoding.ASCII.GetString(buffer);
                return DateTime.Parse(time);
            }
        }
    }
}
