using System;
using System.Net;
using System.Net.Sockets;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Fleck;
using System.Collections.Generic;

namespace AndroidControl.Hacker
{

	class Program
	{
		protected static IMongoClient _client;
		protected static IMongoDatabase _database;

		public static object SendKeys { get; private set; }

		static void Insert(string ip)
		{
			_client = new MongoClient("mongodb://campus:1234@ds243285.mlab.com:43285/campusdemo");
			_database = _client.GetDatabase("campusdemo");

			var document = new BsonDocument { { "ip", ip } };

			var collection = _database.GetCollection<BsonDocument>("Ips");
			var filter = Builders<BsonDocument>.Filter.Exists("ip");
			var result = collection.DeleteMany(filter);
			collection.InsertOne(document);

			Console.WriteLine("IP Updated with success!");
		}

		static void Main(string[] args)

		{
			var meuIp = GetLocalIPAddress();
			Console.WriteLine("My IP: " + meuIp);
			Insert(meuIp + ":12345");
			var _servidor = new WebSocketServer("ws://" + meuIp + ":12345");
			var _conexoes = new List<IWebSocketConnection>();
			_servidor.Start((conexao) =>
			{
				conexao.OnOpen = () =>
				{
					_conexoes.Add(conexao);
					Console.WriteLine("User connected: " + conexao.ConnectionInfo.ClientIpAddress);

				};

				conexao.OnClose = () =>
				{
					_conexoes.Remove(conexao);
				};

				conexao.OnMessage = (mensagem) =>
				{
					mensagem = mensagem.ToLower();


					switch (mensagem)
					{

						case "screen":
							{
								for (int i = 0; i < 60; i++)
								{
									Thread.Sleep(200);
									Screen.ChangeScreen();

								}
								Thread.Sleep(2000);
								Process.Start("notepad");
								Thread.Sleep(2000);

								System.Windows.Forms.SendKeys.SendWait(" Olha pra trás!{ENTER} Invadi sua talk hehehe{ENTER} by: Erick Wendel");


								break;
							}
					}

				};
			});

			Console.ReadKey();
		}

	

	public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("Local IP Address Not Found!");
		}
	}
}