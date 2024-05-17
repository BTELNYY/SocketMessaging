using SocketMessagingServer.ServerData;
using SocketMessagingServer.ServerData.Users;
using SocketMessagingShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocketMessagingShared.CustomTypes;

namespace SocketMessagingServer
{
    public class CustomServerSideClientEventHandler : ClientEventHandler
    {
        public override bool ValidateLogin(LoginData loginData, out string reason)
        {
            UserProfile profile = DataManager.GetProfileByUsername(loginData.Username);
            if (profile == null)
            {
                reason = "Username or password is incorrect.";
                return false;
            }
            if(profile.PasswordHash != loginData.PasswordHash)
            {
                reason = "Username or password is incorrect.";
                return false;
            }
            reason = string.Empty;
            return true;
        }

        public override bool ServerCreateAccount(string username, string password, out string reason)
        {
            return DataManager.CreateProfile(username, password, out reason);
        }
    }
}
