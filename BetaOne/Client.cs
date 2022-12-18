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

        string name = "Gasimo";
        long id = 011010;


        bool isVerified = false;

        // Query of awaiting responses from server!
        public Dictionary<int, Action> actions = new Dictionary<int, Action>();


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
        void sendToServer(Command cmd, Action? onResponse = null)
        {
            if(!disableDebug)
            ServerLogger.logTraffic(cmd, "PC(Direct)", "server");

            // If we expect a response
            if(onResponse != null && cmd.requestId != 0)
            {
                actions.Add(cmd.requestId, onResponse);
            }

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


            if (cmd.requestId != 0)
            {
                actions[cmd.requestId].Invoke();
                return;
            }


            switch (cmd.name)
            {
                // Server asked for identity
                case "ident":
                    {
                        // Auth

                        Command response = new Command("ident", null, ReturnCodes.OK);
                        response.requestId = 1;
                        response.content = new string[] { name, id.ToString() };

                        // Await response!
                        sendToServer(response, () => {

                            // Handle response code
                            if (cmd.code == ReturnCodes.OK)
                            {
                                Console.WriteLine("I got verified.");
                                isVerified = true;
                                return;
                            }

                            if (cmd.code != ReturnCodes.IDENT_REQUESTED)
                            {
                                // Handle showing issues here!

                            }

                        });

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
                case "echo":
                    {
                        Console.WriteLine("Echod: " + cmd.content[0]);
                        return;
                    }

            }

        }
    }
}
