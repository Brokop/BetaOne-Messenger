using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.Http;

namespace BetaOne
{
    [System.Serializable]
    internal class User
    {

        public long? id = null;
        public string username;
        
        public TcpClient client;


    }
}
