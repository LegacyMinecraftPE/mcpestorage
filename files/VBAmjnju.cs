using System;
using System.Collections.Generic;
using System.Threading;
using MCPEReborhServer.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleRESTServer;

namespace MCPEReborhServer
{
    class Program
    {
        static List<ServerInfo> serverList = new List<ServerInfo>();
        static HttpServer _server = new HttpServer(81); 

        static void Main(string[] args)
        {
            serverList.Add(new ServerInfo(1, "TEST REALMS 2023", false, "Minecraft PE 0.8.1", false, 0, null, "public"));
            serverList.Add(new ServerInfo(2, "Flusitk World", true, "Flustik", true, 50, intStr(2), "public"));
            serverList.Add(new ServerInfo(3, "Test Server", true, "test_server", true, 10, intStr(4), "public"));
            serverList.Add(new ServerInfo(4, "Test Server 2", true, "0_0", true, 100, null, "public"));
            serverList.Add(new ServerInfo(6, "Test Server 3", true, "cool server 2023 year (server only for pro players)!!", true, 50, intStr(50), "public"));
            serverList.Add(new ServerInfo(5, "Hypixel", true, "mc.hypixel.net", false, 999, intStr(3), "public"));
            Random Rand = new Random();
            for (int i = 0; i < 10; i++)
            {
                int count = Rand.Next(0, 90);
                bool active = Rand.Next(0, 90) > 20;
                serverList.Add(new ServerInfo(i + 6, $"Realms {i}", active, "test_realms", false, count, active ? intStr(Rand.Next(0, count)) : null, "public"));
            }
            _server.OnRunningServer += OnRunningServer;
            _server.OnReceivedRequest += OnReceivedRequest;
            _server.StartAsync();
            Console.WriteLine("Start server!");
            while (true)
            {
                Console.ReadLine();
            }
        }

        private static void OnReceivedRequest(RequestPacket packet)
        {
            Console.WriteLine(packet.RequestURL);
            string[] data = packet.RequestURL.Split('/');

            if (packet.RequestURL.Contains("loginmc"))
            {
                string version = data[4].Split('?')[1].Split('&')[1].Split('=')[1];
                packet.SetRedirect("http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114&email=tttttstt@gmail.com&newUser=true");
                packet.Send("");
            }
            if (packet.RequestURL.Contains("peo/server/list"))
            {
                //packet.Send("[{ \"id\": 1, \"name\": \"test\", \"serverId\": \"test_server\", \"open\": true, \"ownerName\": \"Flustik\", \"myWorld\": true, \"maxNrPlayers\": 12, \"playerNames\": [\"Flustik1\"] }]");
                //Console.WriteLine(JsonConvert.SerializeObject(serverList));
                packet.Send(JsonConvert.SerializeObject(serverList));
                return;
            }
            if (packet.RequestURL.Contains("peo/info/status"))
            {
                //packet.SetRedirect("http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114&email=tttttstt@gmail.com&newUser=true");
                packet.Send("{  \"buyServerEnabled\": false,  \"createServerEnabled\": false,  \"serviceEnabled\": true }");
                return;
            }
            if (packet.RequestURL.Contains("refresh"))
            {
                //packet.Send("{\"new_registration\": false, \"automatic_login\": true}");
                //packet.Send("{ \"version\": \"0.7.6\", \"id\": \"10114\", \"accessToken\": \"ok\", \"clientToken\": \"test\", \"selectedProfile\": \"Flustik\"}");
                // http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114
                //packet.Send("{ \"version\": \"0.8.1\", \"accessToken\": \"ok\", \"clientToken\": \"test\", \"selectedProfile\": {\"sessionId\": \"1000\", \"username\": \"Flustik\"}, \"username\": \"Flustik\"");
                packet.Send("{ \"accessToken\": \"ok\", \"clientToken\": \"test\", \"selectedProfile\": { \"name\": \"Flustik_\"}");
                return;
            }
            if (packet.RequestURL.Contains("peo/server/5/join"))
            {
                //packet.SetRedirect("http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114&email=tttttstt@gmail.com&newUser=true");
                packet.Send("{ \"ip\": \"mc.hypixel.net\", \"port\": 25565, \"serverId\": \"0\" }");
                return;
            }
            if (packet.RequestURL.Contains("peo/server/4/join"))
            {
                //packet.SetRedirect("http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114&email=tttttstt@gmail.com&newUser=true");
                packet.Send("{ \"ip\": \"192.168.0.85\", \"port\": 19132, \"serverId\": \"0\" }");
                return;
            }
            if (packet.RequestURL.Contains("peo/server/2/join"))
            {
                //packet.SetRedirect("http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114&email=tttttstt@gmail.com&newUser=true");
                packet.SetStatusCode(System.Net.HttpStatusCode.NotFound);
                packet.Send("{ }");
                return;
            }
            if (packet.RequestURL.Contains("peo/server/3/join"))
            {
                //packet.SetRedirect("http://192.168.0.27:9172/m/launchmc?accessToken=ok&clientToken=test&sessionId=1000&identity=flustID&profileName=Flusitk&profileUuid=10114&email=tttttstt@gmail.com&newUser=true");
                packet.Send("{ \"ip\": \"nostalgiape.ddns.net\", \"port\": 19132, \"serverId\": \"0\" }");
                return;
            }
            //packet.Send("request is null");
        }

        private static string[] intStr(int count)
        {
            string[] list = new string[count];
            for (int i = 0; i < list.Length; i++)
                list[i] = "Player " + i;
            return list;
        }

        private static void OnRunningServer()
        {
            Console.WriteLine("ok!");
        }
    }

    [Serializable]
    class ServerInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("open")]
        public bool IsOpen { get; set; }

        [JsonProperty("ownerName")]
        public string OwnerName { get; set; }

        [JsonProperty("myWorld")]
        public bool IsMyRealms { get; set; }

        [JsonProperty("maxNrPlayers")]
        public int MaxPlayers { get; set; }

        [JsonProperty("playerNames")]
        public string[] PlayerList { get; set; }

        [JsonProperty("type")]
        public string RealmsType { get; set; }

        [JsonProperty("serverId")]
        public int ServerId { get; set; }

        [JsonProperty("invited")]
        public string[] InvitedPlayers { get; set; }

        public ServerInfo(int id, string name, bool open, string ownerName, bool myRealms, int maxPlayers, string[] playerList, string type)
        {
            ID = id;
            ServerId = id;
            Name = name;
            IsOpen = open;
            OwnerName = ownerName;
            IsMyRealms = myRealms;
            MaxPlayers = maxPlayers;
            PlayerList = playerList;
            RealmsType = type;
            InvitedPlayers = PlayerList;
        }
    }
}
