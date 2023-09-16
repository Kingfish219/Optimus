using System.Net;
using System.Net.Sockets;

namespace Optimus.Communications
{
    public static class CommunicationsHelper
    {
        public static string GetLocalIpAddress()
        {
            string localIp;
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                localIp = endPoint?.Address.ToString();
            }

            if (!string.IsNullOrEmpty(localIp))
                return localIp;

            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
    }
}

