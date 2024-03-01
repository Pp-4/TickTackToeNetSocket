using System;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
        static IPAddress ZnajdźAdresRozgłoszeniowySieci()
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
                                IPAddress ip = unicastAddress.Address;
                                IPAddress maska = unicastAddress.IPv4Mask;
                                //Debug.WriteLine("  IP Address: " + ip.ToString());
                                //Debug.WriteLine("  Subnet Mask: " + maska.ToString());
                                return ip;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return new(new byte(0));
        }
        static void Rozgłaszanie(string wiadomość)
        {
            UdpClient udpClient = new UdpClient();
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
                                Debug.WriteLine("  IP Address: " + unicastAddress.Address.ToString());
                                Debug.WriteLine("  Subnet Mask: " + unicastAddress.IPv4Mask.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}