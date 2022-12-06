using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;
using System.Data;
using System.ComponentModel.Design;

namespace BetaOne
{
    internal class Client
    {
        bool disableDebug = true;

        StreamWriter serverWriter;
        StreamReader serverReader;

        string name;
        long id;

        bool isVerified = false;

        // Query of awaiting responses from server!
        public Dictionary<string, Action> actions = new Dictionary<string, Action>();


        public void Init(string address, int port)
        {
            new Thread(() => RunClientAsync(address, port)).Start();
        }

        /// <summary>
        /// Read Data from server loop
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public void RunClientAsync(string address, int port)
        {
            var tcpServer = new TcpClient(address, port);
            serverWriter = new StreamWriter(tcpServer.GetStream());
            tcpServer.NoDelay = true;
            serverReader = new StreamReader(tcpServer.GetStream());

            // Receive server messages in this loop
            while (true)
            {
                // RECEIVE
                var line = serverReader.ReadLineAsync().Result;

                // PROCEED
                new Thread(() => commandHandler(commandParser(line), tcpServer)).Start();
            }
        }

        /// <summary>
        /// Sends command to server directly
        /// </summary>
        /// <param name="cmd"></param>
        void sendToServer(Command cmd)
        {
            if(!disableDebug)
            ServerLogger.logTraffic(cmd, "PC(Direct)", "server");

            serverWriter.WriteLineAsync(serializeCommand(cmd)).Wait();
            serverWriter.FlushAsync().Wait();
        }



        /*
         *  COMMAND HANDLER AREA
         */

        public string serializeCommand(Command cmd)
        {
            return JsonConvert.SerializeObject(cmd);
        }

        public Command commandParser(string json)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(json);
            }
            catch (Exception e)
            {
                ServerLogger.LogError("JSON Parse error on: " + json + "\n" + e.Message);
#if DEBUG
                throw (e);
#endif                
                return null;
            }
        }





        public async void commandHandler(Command cmd, TcpClient tcpServer)
        {

            if (cmd == null)
                return;

            if(!disableDebug)
            ServerLogger.logTraffic(cmd, "server", "PC");

            switch (cmd.name)
            {
                // Server asked for identity
                case "ident":
                    {
                        // Handle response code
                        if (cmd.code == ReturnCodes.OK)
                        {
                            Console.WriteLine("I got verified.");
                            isVerified = true;
                            return;
                        }

                        if(cmd.code != ReturnCodes.IDENT_REQUESTED)
                        {
                            // Handle showing issues here!
                        }

                        // Auth

                        Command response = new Command("ident");
                        response.content = new string[] { name, id.ToString() };
                        sendToServer(response);

                        return;
                    }

                // Receive
                case "register":
                    {

                        if(cmd.code == ReturnCodes.OK)
                        {
                            name = cmd.content[0];
                            id = long.Parse(cmd.content[1]);
                        }

                        return;
                    }

            }

        }
    }
}
