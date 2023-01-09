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
    public class UserSession
    {

        public User user;
        
        public TcpClient client;

        public StreamWriter? writer;

    }
}
