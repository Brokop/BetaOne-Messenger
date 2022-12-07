using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Pastel;
using System.Drawing;

namespace BetaOne
{
    internal static class ServerLogger
    {

        public static void logServerInfo(object toLog, string origin = "server")
        {
            Console.WriteLine(String.Format("[{0}] {1}", origin, toLog.ToString()));
        }

        public static void logTraffic(object toLog, string from = "server", string target = "client")
        {
            Console.WriteLine(String.Format("[{0} -> {1}] {2}".Pastel(Color.Gray), from, target, toLog.ToString()));
        }


        public static void logTraffic(Command toLog, string from = "server", string target = "client")
        {
            string errCode = (toLog.code == ReturnCodes.OK ? "" : "["+toLog.code.ToString()+"]");
            string contentSize = (toLog.content == null ? "" : "[Len: " + toLog.content.Length.ToString()+"]");
            string reqID = ((toLog.requestId != null && toLog.requestId != 0) ? "[Req: " + toLog.requestId.ToString() + "]" : "");

            Console.WriteLine(String.Format($"[{from} -> {target}]".Pastel(Color.Gray) +  $" {toLog.name} {contentSize} {errCode} {reqID}"));
        }




        public static void LogError(object toLog)
        {
            Console.WriteLine(String.Format("[Excp] {0}", toLog.ToString()));
        }

    }
}
