using SocketMessagingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer
{
    public class CustomServerSideClientEventHandler : ClientEventHandler
    {
        public override bool ValidateLogin(LoginData loginData, out string reason)
        {
            
            reason = string.Empty;
            return true;
        }
    }
}
