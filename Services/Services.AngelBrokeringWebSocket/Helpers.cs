using System.Net;
using System.Linq;
using System.Globalization;
using System.Net.NetworkInformation;

namespace Services.AngelWebSocket
{
    internal static class Helpers
    {
        public static string GetLocalIPAddress()
        {
            try
            {
                var hostname = System.Environment.MachineName;
                var hostInfo = Dns.GetHostEntry(hostname);
                return
                    hostInfo
                    .AddressList
                    .Where(
                        address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .First()
                    .ToString();
            }
            catch (Exception ex)
            {
                return "127.0.0.1";
            }
        }

        public static string GetPublicIPAddress()
        {
            try
            {
                var request = WebRequest.Create("http://checkip.dyndns.org/");
                using var response = request.GetResponse();
                using var streamReader = new StreamReader(response.GetResponseStream());
                var rawAddress = streamReader.ReadToEnd();
                var startIndex = rawAddress.IndexOf("Address:") + 9;
                var lastIndex = rawAddress.LastIndexOf("</body");
                return rawAddress.Substring(startIndex, lastIndex - startIndex);
            }
            catch (Exception ex)
            {
                return "106.193.147.98";
            }
        }

        public static string GetMacAddress()
        {
            try
            {
                return
                    NetworkInterface
                    .GetAllNetworkInterfaces()
                    .Where(networkInterface =>
                        networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
                        networkInterface.OperationalStatus == OperationalStatus.Up)
                    .First()
                    .GetPhysicalAddress()
                    .ToString();
            }
            catch (Exception ex)
            {
                return "fe80::216e:6507:4b90:3719";
            }
        }
    }
}
