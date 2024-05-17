using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketMessagingShared.CustomTypes;

namespace SocketMessagingShared
{
    public class ClientEventHandler
    {
        public virtual bool ValidateLogin(LoginData loginData, out string reason)
        {
            reason = string.Empty;
            return true;
        }

        public virtual bool ServerCreateAccount(string username, string password, out string reason)
        {
            reason = string.Empty;
            return true;
        }

        public virtual void ClientFailCreatingNewAccount(string reason)
        {

        }

        public virtual void ClientFailLogin(string reason)
        {

        }
    }
}
