using BetaOne_Server;
using System.IO;
using Newtonsoft.Json;

namespace BetaOne
{
    internal class Program
    {


        static void Main(string[] args)
        {

            ServerConfigData sCD = loadConfiguration();

            Server server = new Server(sCD.PORT);
            server.init();


            Client client = new Client();
            client.Init("127.0.0.1", sCD.PORT);

            Client client2 = new Client();
            client2.Init("127.0.0.1", sCD.PORT);


        }

        /// <summary>
        /// Load server config or create default
        /// </summary>
        /// <returns></returns>
        public static ServerConfigData loadConfiguration()
        {
            try
            {
                System.IO.Directory.CreateDirectory("Data");
                string configRaw = null;

                if (File.Exists(Environment.CurrentDirectory + @"\Data\config.json"))
                {
                    configRaw = File.ReadAllText(Environment.CurrentDirectory + @"\Data\config.json");
                    return JsonConvert.DeserializeObject<ServerConfigData>(configRaw);
                } else
                {
                    ServerConfigData sCD = new ServerConfigData();
                    File.WriteAllText(Environment.CurrentDirectory + @"\Data\config.json", JsonConvert.SerializeObject(sCD));
                    return sCD;
                }
            }
            catch(Exception e)
            {
                ServerLogger.LogError("Cannot load server config: " + e.Message);

                return new ServerConfigData();
            }

        }
    }



}