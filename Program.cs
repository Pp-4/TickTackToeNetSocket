using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace TickTackToeNetSocket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            string wiadomość = "test wiadomośći";
            Rozgłaszanie(wiadomość);
        }
        static (IPAddress ip, IPAddress maska, IPAddress broadcast) ZnajdźAdresy()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            try
            {
                foreach (NetworkInterface networkInterface in networkInterfaces)
                {
                    if ((networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                        networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                        networkInterface.Name == "Wi-Fi")
                    {
                        IPInterfaceProperties properties = networkInterface.GetIPProperties();

                        foreach (UnicastIPAddressInformation unicastAddress in properties.UnicastAddresses)
                        {
                            if (unicastAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                Debug.WriteLine("Interface Name: " + networkInterface.Name);
                                byte[] ip = unicastAddress.Address.GetAddressBytes();
                                byte[] maska = unicastAddress.IPv4Mask.GetAddressBytes();
                                byte[] adresRozgłoszenioy = new byte[ip.Length];

                                for (int i = 0; i < ip.Length; i++)
                                {
                                    adresRozgłoszenioy[i] = (byte)(ip[i] | ~maska[i]);
                                }
                                return (new(ip), new(maska), new(adresRozgłoszenioy));
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return (new(0), new(0), new(0));
        }
        static void Rozgłaszanie(string wiadomość)
        {
            var adresy = ZnajdźAdresy();
            try
            {
                var Server = new UdpClient(8888);
                var ResponseData = Encoding.ASCII.GetBytes(wiadomość);
                while (true)
                {
                    Server.Send(ResponseData, ResponseData.Length, new IPEndPoint(adresy.broadcast, 0));
                }
                /*while (true)
                {
                    //var ClientEp = new IPEndPoint(adresy.broadcast, 0);
                    var ClientEp = new IPEndPoint(IPAddress.Any, 0);
                    Console.WriteLine("test");
                    var ClientRequestData = Server.Receive(ref ClientEp);
                    var ClientRequest = Encoding.ASCII.GetString(ClientRequestData);

                    Console.WriteLine("Recived {0} from {1}, sending response", ClientRequest, ClientEp.Address.ToString());
                    Server.Send(ResponseData, ResponseData.Length, ClientEp);
                }*/
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}