using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Utils
{
    public class Utils
    {

        private static Regex ipAddressRegex = new Regex(@"^([0-9]{1,3}).([0-9]{1,3}).([0-9]{1,3}).([0-9]{1,3})$");
        private static Regex portRegex = new Regex(@"^[0-9]{1,5}$");

        public static bool IsPortVaild(int port)
        {
            if (port < 0 || !portRegex.IsMatch(port.ToString())) return false;
            return true;
        }

        public static bool IsAddressValid(string address)
        {
            if ((address == null || address.Length == 0) || !ipAddressRegex.IsMatch(address)) return false;
            return true;
        }

        public static bool Ping(string address)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(address);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException) { }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
        }

        public static byte CountChecksum(byte[] bytes, Boolean omitLastElement=true)
        {
            int sum = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (omitLastElement && i == bytes.Length-1) break;
                sum += (int)bytes[i];
            }

            return (byte)(sum - (sum / 256) * 256);
        }

        public static string DecimalToHex(int decimalValue)
        {
            return decimalValue.ToString("X");
        }

        public static byte HexToDecimal(string hexValue)
        {
            return (byte)int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
        }

        public static string PrintByteArray(byte[] bytes)
        {
            string values = "";
            foreach (byte b in bytes)
            {
                values += (int)b + " ";
            }
            return values;
        }
    }

    public class Logger
    {
        private static TextBlock LOGGER_BLOCK = null;

        private Logger() { }

        public static void InitLogger(TextBlock block)
        {
            LOGGER_BLOCK = block;
        }

        public static void log(string mesg)
        {
            if (LOGGER_BLOCK == null) return;
            LOGGER_BLOCK.Inlines.Add("[ " + DateTime.Now.ToString("h:mm:ss tt") + " ] " + mesg + "\n");
            Console.WriteLine(Environment.NewLine + " >> " + mesg);
        }
    }
}
