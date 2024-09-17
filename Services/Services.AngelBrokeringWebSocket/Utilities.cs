using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services.AngelWebSocket
{
    public static class Utilities
    {
        public static string GetLocalIPAddress()
        {
            string clientIPAddress = "";
            try
            {
                var hostName = System.Environment.MachineName;
                var hostEntry = Dns.GetHostEntry(hostName);
                foreach (var ip in hostEntry.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        clientIPAddress = ip.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return clientIPAddress;
        }
    }
}
