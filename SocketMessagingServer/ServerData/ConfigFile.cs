using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketMessagingServer.ServerData
{
    public class ConfigFile
    {
        [JsonIgnore]
        public virtual string Filename { get; } = "file.json";

        [JsonIgnore]
        public virtual string Directory { get; } = "./data";
    }
}
