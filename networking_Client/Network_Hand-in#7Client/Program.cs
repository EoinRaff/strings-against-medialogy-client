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
        static public bool readyToContinue = false;
        static public string username;
        static public List<string> hand = new List<string>();

        static void Main(string[] args)
        {
            string selection = "";

            username = "";
            string ip = "";
            int port;


            Console.WriteLine("Starting Strings Against Medialogy game client...");

            Console.WriteLine("Select your username");
            username = Console.ReadLine();

            /*
             * 
             * making testing easier
            
            Console.WriteLine ("Enter server IP adress");
            ip = Console.ReadLine ();

            Console.WriteLine ("Enter server port");
            selection = Console.ReadLine ();
            port = Int32.Parse (selection);

            Console.WriteLine ("Connecting to server: " + ip + " on port: " + port);
            */

            TcpClient client = new TcpClient("192.168.43.134", 1234);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            Console.Clear();
            writer.WriteLine(username); //send username to server
            while (!readyToPlay)
            {
                string serverMessage = reader.ReadLine();
                if (serverMessage == "Ready!")
                {
                    readyToPlay = true;
                }
                Console.Clear();
                Console.WriteLine(serverMessage);
            }

            while (true)
            {
                setToDefaults();

                Console.WriteLine("Hello! Welcome to Strings Against Medialogy.");
                Console.WriteLine("Hand: " + hand.Count);

                Console.Write("Press any key to play game\n");
                Console.Write("Press [x] to exit game\n");

                string lineToSend = Console.ReadLine();
                if (lineToSend != "x")
                {
                    lineToSend = "p";
                }
                writer.WriteLine(lineToSend); //send p to play or x to exit

                Console.WriteLine("Hand: " + hand.Count);

                Console.WriteLine("you need {0} cards", askForCards());
                writer.WriteLine(askForCards());
                Console.WriteLine("Hand: " + hand.Count);


                switch (lineToSend)
                {

                    default:
                        string Judge = reader.ReadLine();
                        string questionString = reader.ReadLine();
                        string answerString = reader.ReadLine();
                        List<string> tempHand = new List<string>(answerString.Split('.'));
                        for (int i = 0; i < tempHand.Count -1; i++)
                        {
                            Console.WriteLine("New card: {0}", tempHand[i]);
                            hand.Add(tempHand[i]);
                        }

                        Console.WriteLine("The Judge this turn is: {0}", Judge);
                        if (Judge == username)
                        {
                            Console.Clear();
                            writer.WriteLine("Judge Reply"); //reply to the server to stay in sync with other players. This counts as an "answer, but will be filtered out from the voting"
                            Console.WriteLine("You are now the Judge.");
                            Console.WriteLine("You can't play a card this round, but here is your hand anyway");
                            for (int i = 0; i < hand.Count; i++)
                            {
                                Console.WriteLine("{0}: {1}", i + 1, hand[i]);
                            }
                            while (!readyToContinue)
                            {
                                string serverMessage = reader.ReadLine();
                                if (serverMessage == "Ready!")
                                {
                                    readyToContinue = true;
                                    Console.WriteLine(serverMessage);
                                }
                            }// ready to contine = true

                        }
                        else
                        {
                            Console.WriteLine("Your hand of strings have been dealt \n Choose the string you find the most suitable \n for the missing part in the following statement: \n{0} \n", questionString);

                            for (int i = 0; i < hand.Count; i++)
                            {
                                Console.WriteLine("{0}: {1}", i + 1, hand[i]);
                            }
                            int n;
                            bool validInput = false;
                            while (!validInput)
                            {
                                if (int.TryParse(Console.ReadLine(), out n))
                                {
                                    Console.Clear();
                                    validInput = true;
                                    lineToSend = hand[n - 1];
                                    Console.WriteLine("Remaining cards:");
                                    for (int i = 0; i < hand.Count - 1; i++)
                                    {
                                        Console.WriteLine("{0}: {1}", i + 1, hand[i]);
                                    }
                                    writer.WriteLine(lineToSend); //send answer to server
                                    Console.WriteLine("Question: {0} \nYour answer:{1}", questionString, hand[n - 1]);
                                    hand.RemoveAt(n - 1);
                                    while (!readyToContinue)
                                    {
                                        string serverMessage = reader.ReadLine();
                                        if (serverMessage == "Ready!")
                                        {
                                            readyToContinue = true;
                                            Console.WriteLine(serverMessage);
                                        }
                                    }// ready to contine = true
                                    Console.Clear();
                                    Console.WriteLine("Client ready to continue");
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("try again");
                                    Console.WriteLine("Question: {0}", questionString);
                                    for (int i = 0; i < 5; i++)
                                    {
                                        Console.WriteLine("{0}: {1}", i + 1, hand[i]);
                                    }
                                }   //end if/else parse
                            }   //end while validInput
                        }//after if/else Judge
                        Console.Clear();
                        Console.WriteLine("Question: {0}: ", questionString);
                        Console.WriteLine("Answers:");

                        string judgeAnswersString = reader.ReadLine();
                        List<string> judgeAnswersList = new List<string>(judgeAnswersString.Split('.'));
                        for (int i = 0; i < judgeAnswersList.Count - 1; i++)
                        {
                            Console.WriteLine("{0}: {1}", i + 1, judgeAnswersList[i]);
                        }

                        readyToContinue = false;

                        /////////////////////////////////////////////////////////////////////
                        //Judges Verdict
                        ////////////////////////////////////////////////////////////////////////
                        if (username == Judge)
                        {
                            Console.WriteLine("Choose the winner...");
                            int n;
                            bool validInput = false;
                            while (!validInput)
                            {
                                if (int.TryParse(Console.ReadLine(), out n))
                                {
                                    Console.Clear();
                                    validInput = true;
                                    lineToSend = judgeAnswersList[n - 1];
                                    writer.WriteLine(lineToSend); //send answer to server
                                    Console.WriteLine("Question: {0} \nThe winner:{1}", questionString, judgeAnswersList[n - 1]);
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.WriteLine("try again");
                                    Console.WriteLine("Question: {0}", questionString);
                                    for (int i = 0; i < judgeAnswersList.Count - 1; i++)
                                    {
                                        Console.WriteLine("{0}: {1}", i + 1, judgeAnswersList[i]);
                                    }
                                }   //end if/else parse
                            }   //end while validInput
                        } // end if judge
                        else
                        {
                            Console.WriteLine("waiting for judge");
                            writer.WriteLine("waiting");

                            while (!readyToContinue)
                            {
                                string serverMessage = reader.ReadLine();
                                if (serverMessage == "Ready!")
                                {
                                    readyToContinue = true;
                                    Console.WriteLine(serverMessage);
                                    Console.Clear();
                                }
                            }
                        }
                        Console.WriteLine(reader.ReadLine());
                        Console.WriteLine("Press any key to continue"); //this is a bad mechanic and needs to be fixed
                        Console.ReadKey();
                        writer.WriteLine("{0} wants to continue", username);
                        Console.Clear();
                        Console.WriteLine("Waiting for other players to restart");
                        break;
                    case "x":
                        return;
                }   //end switch statement
            }   //end while loop
        }   //end main()
        static public void setToDefaults()
        {
            isJudge = false;
            readyToPlay = false;
            readyToContinue = false;
        }
        static int askForCards()
        {
            return 5 - (hand.Count);
        }
    }   //end class
}   //end namespace





