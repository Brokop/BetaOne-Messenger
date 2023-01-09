using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaOne
{
    [System.Serializable]
    public  class User
    {

        public long? id { get; set; } = null;
        public string username { get; set; }


        public User(string username, long id) { 

            this.id = id;
            this.username = username;
        }

    }

}
