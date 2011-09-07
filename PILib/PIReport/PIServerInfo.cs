using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Diagnostics;

namespace PILib.PIReport
{
	public class PIServerInfo
	{
		public static string defPath="";
		protected static PISDK.PISDK g_SDK;
		protected static Dictionary<string,PIServerInfo> servers;

		public string Id { get; protected set; }
		public string ServerName { get; protected set; }

		public PIServerInfo(string id, string serverName) {
			this.Id = id;
			this.ServerName = serverName;
		}

		static PIServerInfo() {
			g_SDK = new PISDK.PISDK();
		}

		public static void initServers(){
			servers=new Dictionary<string,PIServerInfo>();
			XDocument doc = XDocument.Load(String.Format("{0}/Data/servers.xml",defPath));
			foreach (XElement server in doc.Root.Elements("server")) {
				string id=server.Attribute("id").Value;
				string serverName = server.Attribute("serverName").Value;
				PIServerInfo newServer = new PIServerInfo(id, serverName);
				servers.Add(id, newServer);				
			}			
		}

		public static void addServer(string id, string name) {
			PIServerInfo newServer = new PIServerInfo(id, name);
			servers.Add(id, newServer);	
		}

		public static PIServerInfo getServerInfo(string id){
			PIServerInfo server;
			if (servers.TryGetValue(id, out server)){
				return server;
			}else{
				return null;
			}
		}


		public static PISDK.Server getPIServer() {
			return getPIServer("DEFAULT");
		}

		public static PISDK.Server getPIServer(string id) {
			try {
				string serverName = getServerInfo(id).ServerName;
				PISDK.Server server = g_SDK.Servers[serverName];				
				return server;
			} catch (Exception e) {
				Logger.error(String.Format("Соединение с сервером, Исключение: {1}",  e.Message));				
				return null;
			}

		}
	}
}