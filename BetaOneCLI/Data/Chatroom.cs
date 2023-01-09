using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaOne
{
    [System.Serializable]
    public class Chatroom
    {

        public string roomName { get; set; }
        public List<UserSession> users { get; set; }


    }
}
