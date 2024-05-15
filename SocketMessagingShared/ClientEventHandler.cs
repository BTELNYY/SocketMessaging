using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingShared
{
    public class ClientEventHandler
    {
        public virtual bool ValidateLogin(LoginData loginData, out string reason)
        {
            reason = string.Empty;
            return true;
        }
    }
}
