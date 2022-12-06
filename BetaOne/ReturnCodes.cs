using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetaOne
{
    internal enum ReturnCodes
    {
        OK,
        INTERNAL_ERROR,
        BAD_REQUEST,
        BAD_DATA,
        IDENT_REQUESTED,
        DOES_NOT_EXIST,
        TIMED_OUT
    }
}
