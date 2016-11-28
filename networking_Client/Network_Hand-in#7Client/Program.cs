using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpEchoClient
{
	class TcpEchoClient
	{

		static void Main(string[] args)
		{
			string selection = "";

			string username = "";
			string ip = "";
			int port;

			Console.WriteLine("Starting Strings against Medialogy game client...");

			Console.WriteLine ("Select your username");
			username = Console.ReadLine ();

			Console.WriteLine ("Enter server IP adress");
			ip = Console.ReadLine ();

			Console.WriteLine ("Enter server port");
			selection = Console.ReadLine ();
			port = Int32.Parse (selection);

			Console.WriteLine ("Connecting to server: " + ip + " on port: " + port);


			TcpClient client = new TcpClient(ip, port);
			NetworkStream stream = client.GetStream();
			StreamReader reader = new StreamReader(stream);
			StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };
		
			while (true)
			{
				Console.Write("Enter to send: ");
				string lineToSend = Console.ReadLine();
				Console.WriteLine("Sending to server: " + lineToSend);
				writer.WriteLine(lineToSend);
				string lineReceived = reader.ReadLine();
				Console.WriteLine("Received from server: " + lineReceived);




			}
		}


	
	}
}
