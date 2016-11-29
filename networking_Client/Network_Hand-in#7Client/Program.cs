using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace TcpEchoClient
{
	class TcpEchoClient
	{
        static public bool isJudge = false;

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

				writer.WriteLine(username);
                string playerRole = reader.ReadLine();
                if (playerRole == "Judge")
                {
                    Console.Clear();
                    Console.WriteLine("You are now the Judge! Waiting for other players...");
                    if (reader.ReadLine() == "Ready")
                    {
                        Console.WriteLine("Here are the responses. Which was funniest?");
                        //displayAnswers()
                    }
                }
                else
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
        static void displayAnswers()
        {
            //this should contain the code which will show the Judge the answers and let them vote
            //it should recieve input from the server after the other players have sent their answers.
            //Then it should send the client a respones
        }


	
	}
}
