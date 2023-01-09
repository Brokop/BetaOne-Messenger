using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Terminal.Gui;

namespace BetaOneCLI
{
    internal static class Persistency
    {

        public static ClientConfig LocalConfig {
            get { if (configuration == null)
                    configuration = loadConfiguration();
                return configuration;            
            }
            }

        static ClientConfig configuration = null;


        /// <summary>
        /// Load server config or create default
        /// </summary>
        /// <returns></returns>
        public static ClientConfig loadConfiguration()
        {
            try
            {
                System.IO.Directory.CreateDirectory("Data");
                string configRaw = null;


                if (File.Exists(Environment.CurrentDirectory + @"\Data\config.json"))
                {
                    configRaw = File.ReadAllText(Environment.CurrentDirectory + @"\Data\config.json");
                    return JsonConvert.DeserializeObject<ClientConfig>(configRaw);
                }
                else
                {


                    ClientConfig sCD = new ClientConfig();
                    File.WriteAllText(Environment.CurrentDirectory + @"\Data\config.json", JsonConvert.SerializeObject(sCD));

                    Notification.Show("Default config file has been created.");

                    return sCD;

                }
            }
            catch (Exception e)
            {
                Notification.Show("Cannot load server config: " + e.Message);
                return new ClientConfig();
            }

        }

        [System.Serializable]
        internal class ClientConfig
        {

            public int PORT = 7769;
            public string IP = "127.0.0.1";

            public string Name = "Username";
            public string Password = "";

        }

    }
}
