using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;

namespace TcpEchoClient
{
    class TcpEchoClient
    {
        static public bool isJudge = false;
        static public bool readyToPlay = false;

        static void Main(string[] args)
        {
            string selection = "";

            string username = "";
            string ip = "";
            int port;

            Console.WriteLine("Starting Strings Against Medialogy game client...");

            Console.WriteLine("Select your username");
            username = Console.ReadLine();

            /*
             * outcommented for quick testing
             * 
			Console.WriteLine ("Enter server IP adress");
			ip = Console.ReadLine ();

			Console.WriteLine ("Enter server port");
			selection = Console.ReadLine ();
			port = Int32.Parse (selection);

			Console.WriteLine ("Connecting to server: " + ip + " on port: " + port);
            */

            TcpClient client = new TcpClient("192.168.43.29", 1234);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            Console.Clear();
            writer.WriteLine(username); //send username to server
            while (!readyToPlay)
            {
                string serverMessage = reader.ReadLine();
                if (serverMessage=="Ready!")
                {
                    readyToPlay = true;
                }
                Console.Clear();
                Console.WriteLine(serverMessage);
            }
            Console.WriteLine("Hello! Welcome to Strings Against Medialogy.");

            Console.Write("Press [p] to join game\n");
            Console.Write("Press [x] to exit game\n");

            string lineToSend = Console.ReadLine();
            writer.WriteLine(lineToSend);
            Console.Clear();

            while (true)
            {
                switch (lineToSend)
                {

                    case "p":

                        ////////////////////////////////////////////////////
                        //Recieve player role from the server
                        //////////////////////////////////////////////////
                        /*
                        string playerRole = reader.ReadLine();
                        Console.WriteLine("Your role this turn is: {0}", playerRole);
                        if (playerRole == "True")
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
                            string lineReceived = reader.ReadLine();
                            Console.WriteLine("Your hand of strings have been dealt \n Choose the string you find the most suitable \n for the missing part in the following statement: \n{0} \n", lineReceived);


                            lineReceived = reader.ReadLine();
                            List<string> yourHandOfCards = new List<string>(lineReceived.Split('.'));

                            for (int i = 0; i < 5; i++)
                            {
                                //Console.WriteLine("{0}: {1}", i + 1, yourHandOfCards[i]);
                            }

                            lineToSend = Console.ReadLine();

                            writer.WriteLine(lineToSend);
                        }*/
                        ////////////////////////////////////////////////////
                        //This is a duplicate of the code above. When Judge is implemented it should occur in the if statement above
                        ///////////////////////////////////////	
                        string questionString = reader.ReadLine();
                        Console.WriteLine("Your hand of strings have been dealt \n Choose the string you find the most suitable \n for the missing part in the following statement: \n{0} \n", questionString);


                        string answerString = reader.ReadLine();
                        List<string> yourHandOfCards = new List<string>(answerString.Split('.'));

                        for (int i = 0; i < 5; i++)
                        {
                            Console.WriteLine("{0}: {1}", i + 1, yourHandOfCards[i]);
                        }
                        //needs to input 1, 2, 3, 4 or 5
                        string answerChoice = Console.ReadLine();
                        int n;
                        bool validInput = false;
                        while (!validInput)
                        {
                            if (int.TryParse(answerChoice, out n))
                            {
                                if (n > 0 && n < 6)
                                {
                                    validInput = true;
                                    lineToSend = yourHandOfCards[n - 1];
                                    //remove this card from your hand here
                                }
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("try again");
                                Console.WriteLine("Question: {0}", questionString);
                                for (int i = 0; i < 5; i++)
                                {
                                    Console.WriteLine("{0}: {1}", i + 1, yourHandOfCards[i]);
                                }

                            }
                        }
 

                        writer.WriteLine(lineToSend);
                        break;
                    case "x":

                        break;
                }   //end switch statement
            }   //end while loop
        }   //end main()
        static void displayAnswers()
        {
            //this should contain the code which will show the Judge the answers and let them vote
            //it should recieve input from the server after the other players have sent their answers.
            //Then it should send the client a respones
        }   //end displayAnswers()
    }   //end class
}   //end namespace





