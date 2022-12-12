using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json;
using System.Net.Http;
using LiteDB;

namespace BetaOne
{
    internal class Server
    {

        int port;
        bool enableLogging = true;

        LiteDatabase db;
        ILiteCollection<User> users;

        public Server(int port)
        {
            this.port = port;
        }

        /// <summary>
        /// Initializes this server instance on PORT given in constructor.
        /// </summary>
        public void init()
        {
            // Init database
            initDatabase();

            // Listen on PORT
            new Thread(listenForClients).Start();   
        }


        void initDatabase()
        {

            System.IO.Directory.CreateDirectory("Data");

            db = new LiteDatabase(Environment.CurrentDirectory + @"\Data\MainData.db");
                // Get a collection (or create, if doesn't exist)
                users = db.GetCollection<User>("users");
                users.EnsureIndex(x => x.username);
            
        }

        void listenForClients()
        {
            TcpListener tcpListener = new TcpListener(System.Net.IPAddress.Loopback, port);
            tcpListener.Start();
            ServerLogger.logServerInfo("Starting server on " + port);

            // Check for new clients here
            while (true)
            {
                TcpClient tcpClient = tcpListener.AcceptTcpClientAsync().Result;
                new Thread(() => RunClient(tcpClient)).Start();
            }

        }

        void RunClient(TcpClient tcpClient)
        {
            ServerLogger.logTraffic("Connected.", tcpClient.Client.RemoteEndPoint.ToString(), "server");
            var reader = new StreamReader(tcpClient.GetStream());
            var writer = new StreamWriter(tcpClient.GetStream());
            // tcpClient.NoDelay = true;

            UserSession u = new UserSession();
            u.client = tcpClient;
            u.writer = writer;


            // SEND IDENT()
            Command request = new Command("ident", null, ReturnCodes.IDENT_REQUESTED);
            sendToClient(request, u);


            // AWAIT IDENT
            while (u.user == null)
            {

                // RECEIVE IDENT
                var line = reader.ReadLineAsync().Result;

                // PROCESS
                commandHandler(commandParser(line), u);
            
            }

            ServerLogger.logServerInfo("User authorized " + u.user.username);

            // CLIENT LOOP
            while (true)
            {
                // RECEIVE
                var line = reader.ReadLineAsync().Result;

                // PROCESS
                new Thread(() => commandHandler(commandParser(line), u)).Start();
            }
        }


        /// <summary>
        /// Sends command to client
        /// </summary>
        /// <param name="cmd"></param>
        void sendToClient(Command cmd, UserSession clientSession)
        {

            clientSession.writer.WriteLineAsync(serializeCommand(cmd)).Wait();
            clientSession.writer.Flush();

            ServerLogger.logTraffic(cmd, "Server", clientSession.client.Client.RemoteEndPoint.ToString());

        }




        /*
         *  COMMAND HANDLER AREA
         */

        /// <summary>
        /// Serialize command to JSON string
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string serializeCommand(Command cmd)
        {
            return JsonConvert.SerializeObject(cmd);
        }

        /// <summary>
        /// Parse command from JSON string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public Command commandParser(string json)
        {
            try 
            { 
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Command>(json);
            }
            catch (Exception e)
            {
                ServerLogger.LogError(e.Message);

#if DEBUG
                throw (e);
#endif

                return null;
            }

        }


        /// <summary>
        /// Handle commands and responses
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="session"></param>
        public void commandHandler(Command cmd, UserSession session)
        {

            if (cmd == null)
                return;

            ServerLogger.logTraffic(cmd, session.client.Client.RemoteEndPoint.ToString(), "server");

            switch (cmd.name)
            {
                case "ident":
                    {

                        // WAS OK?
                        if(cmd.content[0] != null && long.Parse(cmd.content[1]) != 0)
                        {
                            Command result = new Command("ident");
                            result.requestId = cmd.requestId;

                            User u = new User(cmd.content[0], long.Parse(cmd.content[1]));

                            // If user does not exist
                            if (!users.Exists(Query.EQ("id", cmd.content[1])))
                            {
                                result.code = ReturnCodes.DOES_NOT_EXIST;
                                sendToClient(result, session);
                                return;
                            }


                            sendToClient(result, session);
                            Console.WriteLine("User authenticated: " + session.user.id + " " + session.user.username);

                            return;
                        }
                        else
                        {
                            // NOT OK
                            Command result = new Command("ident");
                            result.code = ReturnCodes.BAD_DATA;
                            result.requestId = cmd.requestId;

                            sendToClient(result, session);
                            return;
                        }   
                    }

                case "register":
                    {
                        // Bad length
                        if(cmd.content.Length < 2)
                        {
                            sendToClient(new Command("ident", null, ReturnCodes.BAD_DATA), session);
                            return;
                        }

                        session.user.username = cmd.content[0];
                        session.user.id = 101010;

                        // Send handle back
                        sendToClient(new Command("register", new string[] {session.user.username, session.user.id.ToString()}, ReturnCodes.OK), session);

                        return;
                    }

                default:
                    {
                        sendToClient(new Command("result", null, ReturnCodes.BAD_REQUEST), session);
                        return;
                    }
            }
        }
    }
}
