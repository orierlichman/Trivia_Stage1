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

                try
                {
                    Question X = db.AddQuestion(this.currentPlayer, Q, cAnswer, wAnswer1, wAnswer2, wAnswer3, S);
                    Console.WriteLine("Question was added, and now pending");
                    db.ResetScores();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
            }

            Console.WriteLine("Not implemented yet! Press any key to continue...");
            Console.ReadKey(true);
        }

        public void ShowPendingQuestions()
        {
            char c = ' ';
            TriviaGamesDbContext db = new TriviaGamesDbContext();
            Console.WriteLine("Press 1 to check the pending questions, or press 2 to run through the In-Game questions" + "\n" + "Press B to go back");

            c = Console.ReadKey(true).KeyChar;
            while (c != 'b' || c != 'B')
            {
                if (c == '2')
                {
                    if (db.CheckForManager(this.currentPlayer) == false)
                    {
                        Console.WriteLine("You are not eligible to run through In-Game questions");
                    }
                    else
                    {
                        foreach (Question q in db.Questions)
                        {
                            while (c != 'b' || c != 'B')
                            {
                                if (q.StatusId == 2)
                                {
                                    db.ShowQuestion1(q);
                                    Console.WriteLine("If the question is OK with you press 1, if you want to eliminate the question press 2, if you want to update it press 3" + "\n" + "Press B to go back or N to check the next question");
                                    c = Console.ReadKey(true).KeyChar;
                                    if (c == 'n' || c == 'N' || c == '1')
                                    {

                                    }
                                    else if (c == '2')
                                    {
                                        q.StatusId = 3;
                                        try
                                        {
                                            db.UpdateQuestion(q);
                                            Console.WriteLine("question eliminated");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else if (c == '3')
                                    {
                                        Console.WriteLine("You chose to update the question, you can press B to go back or N to check the next question");
                                        Console.WriteLine("Press 1 if you want to update the question itself");
                                        Console.WriteLine("Press 2 if you want to update the correct answer");
                                        Console.WriteLine("Press 3 if you want to update the first wrong answer");
                                        Console.WriteLine("Press 4 if you want to update the second wrong answer");
                                        Console.WriteLine("Press 5 if you want to update the third wrong answer");
                                        c = Console.ReadKey(true).KeyChar;
                                        if (c == 'n' || c == 'N' || c == 'b' || c == 'B')
                                        {

                                        }
                                        else if(c == '1')
                                        {
                                            string NewQ;
                                            Console.WriteLine("enter new question");
                                            NewQ = Console.ReadLine();
                                            q.Question1 = NewQ;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
                if (c == '1')
                {
                    if (db.CheckForAcceptionEligible(this.currentPlayer) == false)
                    {
                        Console.WriteLine("You are not eligible check the pending questions");
                    }
                    else
                    {
                        foreach (Question q in db.Questions)
                        {
                            while (c != 'b' || c != 'B')
                            {
                                if (q.StatusId == 1)
                                {
                                    db.ShowQuestion1(q);
                                    Console.WriteLine("If you want to accept the question press 1, if you want to eliminate the question press 2" + "\n" + "Press B to go back or N to check the next question");
                                    c = Console.ReadKey(true).KeyChar;
                                    if (c == 'n' || c == 'N')
                                    {

                                    }
                                    else if (c == '1')
                                    {
                                        q.StatusId = 2;
                                        try
                                        {
                                            db.UpdateQuestion(q);
                                            Console.WriteLine("question approved");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else if (c == '2')
                                    {
                                        q.StatusId = 3;
                                        try
                                        {
                                            db.UpdateQuestion(q);
                                            Console.WriteLine("question eliminated");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }


            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }
        public void ShowGame()
        {
            TriviaGamesDbContext db = new TriviaGamesDbContext();
            List<Question> approvedQuest = db.GetApprovedQuestions();
            Random random = new Random();
            int correct;
            char c = ' ';
            while (c != 'B' && c != 'b')
            {
                foreach (Question quest in approvedQuest)
                {
                    if (c == 'B' || c == 'b')
                    {
                        break;
                    }

                    Console.WriteLine("the score : " + currentPlayer.Score);
                        Console.WriteLine("#" + quest.QuestionId);
                        Console.WriteLine(quest.Question1);
                    
                        correct = random.Next(1, 5);
                    
                    if (correct == 1)
                    {
                        Console.WriteLine("1. " + quest.CorrectAnswer);
                        Console.WriteLine("2. " + quest.WrongAnswer1);
                        Console.WriteLine("3. " + quest.WrongAnswer2);
                        Console.WriteLine("4. " + quest.WrongAnswer3);
                    }
                    else if (correct == 2)
                    {
                        Console.WriteLine("1. " + quest.WrongAnswer1);
                        Console.WriteLine("2. " + quest.CorrectAnswer);
                        Console.WriteLine("3. " + quest.WrongAnswer2);
                        Console.WriteLine("4. " + quest.WrongAnswer3);
                    }
                    else if (correct == 3)
                    {
                        Console.WriteLine("1. " + quest.WrongAnswer1);
                        Console.WriteLine("2. " + quest.WrongAnswer2);
                        Console.WriteLine("3. " + quest.CorrectAnswer);
                        Console.WriteLine("4. " + quest.WrongAnswer3);
                    }
                    else if (correct == 4)
                    {
                        Console.WriteLine("1. " + quest.WrongAnswer1);
                        Console.WriteLine("2. " + quest.WrongAnswer2);
                        Console.WriteLine("3. " + quest.WrongAnswer3);
                        Console.WriteLine("4. " + quest.CorrectAnswer);
                    }
                    bool playerUpdate;
                    int ans;
                    Console.WriteLine("select the right answer");
                    ans = int.Parse(Console.ReadLine());
                    if(ans == correct)
                    {
                        CleareAndTtile("GAME");
                        Console.WriteLine("YOU RIGHT!!!");
                        currentPlayer.Score = currentPlayer.Score + 10;
                        playerUpdate = true;
                    }
                    else
                    {
                        CleareAndTtile("GAME");
                        Console.WriteLine("YOU WRONG!!!");
                        currentPlayer.Score = currentPlayer.Score - 5;
                        playerUpdate = true;
                    }
                    if (playerUpdate == true)
                    {
                        try
                        {
                            db.UpdatePlayer(currentPlayer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to update player score");
                        }

                        Console.WriteLine("press B to back!!!");
                        Console.WriteLine("press any key to continue play!!!");
                        c = Console.ReadKey(true).KeyChar;
                        //c = char.Parse(Console.ReadLine());
                        CleareAndTtile("GAME");
                    }

                    

                }
            }


            //    Console.WriteLine("Not implemented yet! Press any key to continue...");
            //Console.ReadKey(true);
        }
        public void ShowProfile()
        {
            char c = ' ';
            while (c != 'B' && c != 'b')
            {


                Console.WriteLine("The Email : " + currentPlayer.Email);
                Console.WriteLine("The Name : " + currentPlayer.Name);
                Console.WriteLine("The Password : " + currentPlayer.Password);
                Console.WriteLine("The Rank : " + currentPlayer.Rank.RankStatus);
                Console.WriteLine("The Score : " + currentPlayer.Score);

                Console.WriteLine("     ");

                Console.WriteLine("Press Y if you want to change something?");
                Console.WriteLine("Press B to come back");
                c = Console.ReadKey(true).KeyChar;
                bool playerUpdate = false;
                if (c == 'Y' || c == 'y')
                {
                    CleareAndTtile("");
                    int num;
                    Console.WriteLine("Press 1 if you want to change name");
                    Console.WriteLine("Press 2 if you want to change Email");
                    Console.WriteLine("Press 3 if you want to change password");
                    num = int.Parse(Console.ReadLine());
                    while (num != 1 && num != 2 && num != 3)
                    {
                        Console.WriteLine("     ");

                        Console.WriteLine("Press 1 if you want to change name");
                        Console.WriteLine("Press 2 if you want to change Email");
                        Console.WriteLine("Press 3 if you want to change password");
                        num = int.Parse(Console.ReadLine());
                    }
                    if (num == 1)
                    {
                        string NewName;
                        Console.WriteLine("enter new name");
                        NewName = Console.ReadLine();
                        while (!IsNameValid(NewName))
                        {
                            Console.Write("name must be at least 3 characters! Please try again: ");
                            NewName = Console.ReadLine();
                        }
                        currentPlayer.Name = NewName;
                        playerUpdate = true;
                    }
                    else if (num == 2)
                    {
                        string NewEmail;
                        Console.WriteLine("enter new Email");
                        NewEmail = Console.ReadLine();
                        while (!IsEmailValid(NewEmail))
                        {
                            Console.Write("Bad Email Format! Please try again:");
                            NewEmail = Console.ReadLine();
                        }
                        currentPlayer.Email = NewEmail;
                        playerUpdate = true;
                    }
                    else if (num == 3)
                    {
                        string NewPass;
                        Console.WriteLine("enter new password");
                        NewPass = Console.ReadLine();
                        while (!IsPasswordValid(NewPass))
                        {
                            Console.Write("password must be at least 4 characters! Please try again: ");
                            NewPass = Console.ReadLine();
                        }
                        currentPlayer.Password = NewPass;
                        playerUpdate = true;
                    }
                    if (playerUpdate == true)
                    {
                        try
                        {
                            TriviaGamesDbContext db = new TriviaGamesDbContext();
                            db.UpdatePlayer(currentPlayer);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to update player");
                        }
                    }
                    Console.WriteLine("SAVE CHANGES!!! press B to back");
                    Console.WriteLine("press any key to continue update");
                    c = Console.ReadKey(true).KeyChar;
                }
            }
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
