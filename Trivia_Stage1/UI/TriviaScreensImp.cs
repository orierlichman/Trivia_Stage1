using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Trivia_Stage1.Models;

namespace Trivia_Stage1.UI
{
    public class TriviaScreensImp:ITriviaScreens
    {

        //Place here any state you would like to keep during the app life time
        //For example, player login details...
        private Player currentPlayer;

        //Implememnt interface here
        public bool ShowLogin()
        {
            this.currentPlayer = null;

            char c = ' ';
            while (c != 'B' && c != 'b' && this.currentPlayer == null)
            {
                CleareAndTtile("Login");

                Console.Write("Please Type your email: ");
                string email = Console.ReadLine();
                while (!IsEmailValid(email))
                {
                    Console.Write("Bad Email Format! Please try again:");
                    email = Console.ReadLine();
                }

                Console.Write("Please Type your password: ");
                string password = Console.ReadLine();
                while (!IsPasswordValid(password))
                {
                    Console.Write("password must be at least 4 characters! Please try again: ");
                    password = Console.ReadLine();
                }

                try
                {
                    TriviaGamesDbContext db = new TriviaGamesDbContext();
                    this.currentPlayer = db.Login(email, password);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                //Provide a proper message for example:
                Console.WriteLine("Login is successed");
                Console.WriteLine("Press (B)ack to go back ...");
                //Get another input from user
                c = Console.ReadKey(true).KeyChar;
            }


            //return true if login suceeded!
            return (this.currentPlayer != null);


           
        }
        public bool ShowSignup()
        {
            //Logout user if anyone is logged in!
            //A reference to the logged in user should be stored as a member variable
            //in this class! Example:
            //
            this.currentPlayer = null;

            //Loop through inputs until a user/player is created or 
            //user choose to go back to menu
            char c = ' ';
            while (c != 'B' && c != 'b' && this.currentPlayer == null)
            {
                //Clear screen
                CleareAndTtile("Signup");

                Console.Write("Please Type your email: ");
                string email = Console.ReadLine();
                while (!IsEmailValid(email))
                {
                    Console.Write("Bad Email Format! Please try again:");
                    email = Console.ReadLine();
                }

                Console.Write("Please Type your password: ");
                string password = Console.ReadLine();
                while (!IsPasswordValid(password))
                {
                    Console.Write("password must be at least 4 characters! Please try again: ");
                    password = Console.ReadLine();
                }

                Console.Write("Please Type your Name: ");
                string name = Console.ReadLine();
                while (!IsNameValid(name))
                {
                    Console.Write("name must be at least 3 characters! Please try again: ");
                    name = Console.ReadLine();
                }


                Console.WriteLine("Connecting to Server...");
                 //Create instance of Business Logic and call the signup method
                 //For example:
                try
                {
                    TriviaGamesDbContext db = new TriviaGamesDbContext();
                    this.currentPlayer = db.Signup(email, password, name);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to signup! Email may already exist in DB!");
                }
                
                

                //Provide a proper message for example:
                Console.WriteLine("Press (B)ack to go back or any other key to signup again...");
                //Get another input from user
                c = Console.ReadKey(true).KeyChar;
            }
            //return true if signup suceeded!
            return (this.currentPlayer != null);
        }

        public void ShowAddQuestion()
        {
            TriviaGamesDbContext db = new TriviaGamesDbContext();
            bool A = db.AddEligible(this.currentPlayer);
            if (A == false)
            {
                Console.WriteLine("You are not eligible to add a question !!");
            }
            else
            {
                Console.WriteLine("Choose question subject" + "\n");
                db.ShowSubjects();
                int S = int.Parse(Console.ReadLine());
                Console.WriteLine("Enter your question");
                string Q = Console.ReadLine();
                Console.WriteLine("Enter the correct answer");
                string cAnswer = Console.ReadLine();
                Console.WriteLine("Enter wrong answer number 1");
                string wAnswer1 = Console.ReadLine();
                Console.WriteLine("Enter wrong answer number 2");
                string wAnswer2 = Console.ReadLine();
                Console.WriteLine("Enter wrong answer number 3");
                string wAnswer3 = Console.ReadLine();
            }


            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }

        public void ShowPendingQuestions()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }
        public void ShowGame()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }
        public void ShowProfile()
        {
            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }

        //Private helper methodfs down here...
        private void CleareAndTtile(string title)
        {
            Console.Clear();
            Console.WriteLine($"\t\t\t\t\t{title}");
            Console.WriteLine();
        }

        private bool IsEmailValid(string emailAddress)
        {
            var pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

            var regex = new Regex(pattern);
            return regex.IsMatch(emailAddress);
        }

        private bool IsPasswordValid(string password)
        {
            return password != null && password.Length >= 3;
        }

        private bool IsNameValid(string name)
        {
            return name != null && name.Length >= 3;
        }

    }
}
