using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BetaOne
{
    [System.Serializable]
    internal class Command
    {

        public string name;
        public string[] content;

        public ReturnCodes code = ReturnCodes.OK;
        public int requestId;


        public Command(string name, string[] content = null, ReturnCodes code = ReturnCodes.OK) { 
            
            this.name = name;
            this.content = content;
            this.code = code;
        }



    }
}
