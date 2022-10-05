using System;
using System.IO;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace OutlineVpnKeyGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Outline VPN Access Key Generator - Developed By Soheil MV";

            Console.Write("Cipher : ");
            string cipher = Console.ReadLine().Trim();

            Console.Write("Secret : ");
            string secret = Console.ReadLine().Trim();

            Console.Write("Server : ");
            string server = Console.ReadLine().Trim();

            Console.Write("Port : ");
            string port = Console.ReadLine().Trim();

            Console.Write("Name : ");
            string name = HttpUtility.UrlEncode(Console.ReadLine().Trim());

            Console.WriteLine();

            if (!string.IsNullOrEmpty(cipher) && !string.IsNullOrEmpty(secret) && !string.IsNullOrEmpty(server) && !string.IsNullOrEmpty(port) && !string.IsNullOrEmpty(name))
            {
                if (!CheckTheServer(server))
                    Console.WriteLine("The server format is not correct (the correct format should be 127.0.0.1 or example.com).");
                else if (!CheckThePort(port))
                    Console.WriteLine("The selected port must be between 1 and 65535");
                else
                {
                    string key = GenerateAccessKey(cipher, secret, server, port, name);
                    File.WriteAllText($"{Environment.CurrentDirectory}\\Key.txt", key);
                    Console.WriteLine(key);
                }
            }
            else
                Console.WriteLine("Blank values are not allowed!");

            Console.ReadKey();
        }

        //https://gist.github.com/cho0o0/1b8ef91001919646a55950a08cb132be
        private static string GenerateAccessKey(string cipher, string secret, string server, string port, string name)
        {
            return $"ss://{Convert.ToBase64String(Encoding.ASCII.GetBytes($"{cipher.ToLower()}:{secret}"))}@{server}:{port}#{name}";
        }

        private static bool CheckTheServer(string address)
        {
            if (Regex.IsMatch(address, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?") || Regex.IsMatch(address, "((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)"))
                return true;
            else
                return false;
        }

        private static bool CheckThePort(string port)
        {
            try
            {
                int portInt = int.Parse(port);
                if (portInt >= 1 && portInt <= 65535)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
