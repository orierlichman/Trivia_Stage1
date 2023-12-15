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
            TriviaGamesDbContext db = new TriviaGamesDbContext();
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
                    this.currentPlayer = db.Login(email, password);
                    if (this.currentPlayer != null)
                    {
                        Console.WriteLine("Login is successed");
                    }
                    else
                    {
                        Console.WriteLine("Login failed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Login failed");
                }

                //Provide a proper message for example:
                Console.WriteLine("Press (B)ack to go back or any other key to try Login again");
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
            TriviaGamesDbContext db = new TriviaGamesDbContext();
            while (c != 'B' && c != 'b' && this.currentPlayer == null)
            {
                //Clear screen
                CleareAndTtile("Signup");

                string email="";
                bool z = false;
                while (z != true)
                {
                    Console.Write("Please Type your email: ");
                    email = Console.ReadLine();
                    while (!IsEmailValid(email))
                    {
                        Console.Write("Bad Email Format! Please try again:");
                        email = Console.ReadLine();
                    }
                    z = true;
                    foreach (Player p in db.Players)
                    {
                        if (p.Email == email)
                        {
                            z = false;
                        }
                    }
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
            char c = ' ';
            while (c != 'b' && c != 'B')
            {
                bool A = db.AddEligible(this.currentPlayer);
                if (A == false)
                {
                    Console.WriteLine("You are not eligible to add a question !!");
                    Console.WriteLine("press anything to continue");
                    c = Console.ReadKey(true).KeyChar;
                    c = 'b';
                }

                else
                {
                    Console.WriteLine("Choose question subject" + "\n");
                    foreach (Subject s in db.Subjects)
                    {
                        Console.WriteLine("Enter " + s.SubjectId + " for " + s.SubjectName);
                    }
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

                    Console.WriteLine("press B to back or press any key to continue add");
                    c = Console.ReadKey(true).KeyChar;
                }
                
            }
            

            //Console.WriteLine("Not implemented yet! Press any key to continue...");
            //Console.ReadKey(true);
        }

        public void ShowPendingQuestions()
        {
            char c = ' ';
            char x = ' ';
            char l = ' ';
            TriviaGamesDbContext db = new TriviaGamesDbContext();
            while (c != 'b' && c != 'B')
            {
                Console.WriteLine("Press 1 to check the pending questions, or press 2 to run through the In-Game questions");
                Console.WriteLine("Press 3 if you want to add a question subject for the game");
                Console.WriteLine("Press B to go back");
                c = char.Parse(Console.ReadLine());
                //c = Console.ReadKey(true).KeyChar;

                    if (c == '2')
                    {
                        if (db.CheckForManager(this.currentPlayer) == false)
                        {
                            Console.WriteLine("You are not eligible to run through In-Game questions");
                            Console.WriteLine("press anything to continue");
                        c = char.Parse(Console.ReadLine());
                            //Console.ReadKey(true).KeyChar;
                        }
                        else
                        {
                            foreach (Question q in db.Questions)
                            {
                                    if (q.StatusId == 2 && x != 'b' && x != 'B')
                                    {
                                        db.ShowQuestion1(q);
                                        Console.WriteLine("If the question is OK with you press 1, if you want to eliminate the question press 2, if you want to update it press 3");
                                        Console.WriteLine("Press B to go back to the question screen or N to check the next question");
                                        x = Console.ReadKey(true).KeyChar;
                                        //x = char.Parse(Console.ReadLine());
                                        if (x == 'n' || x == 'N' || x == '1')
                                        {

                                        }
                                        else if (x == '2')
                                        {
                                            q.StatusId = 3;
                                            try
                                            {
                                                db.UpdateQuestion(q);
                                                Console.WriteLine("question eliminated");
                                                int W = q.WriterId;
                                                foreach (Player p in db.Players)
                                                {
                                                    if (p.PlayerId == W)
                                                    {
                                                        p.NumOfQuestions--;
                                                        db.UpdatePlayer(p);
                                                    }
                                                }
                                                
                                                db.RankUpdator();
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        else if (x == '3')
                                        {
                                            while (x != 'b' && x != 'B' && x == 'n' && x == 'N')
                                            {
                                                Console.WriteLine("You chose to update the question, you can press B to go back to question screen or N to check the next question");
                                                Console.WriteLine("Press 1 if you want to update the question itself");
                                                Console.WriteLine("Press 2 if you want to update the correct answer");
                                                Console.WriteLine("Press 3 if you want to update the first wrong answer");
                                                Console.WriteLine("Press 4 if you want to update the second wrong answer");
                                                Console.WriteLine("Press 5 if you want to update the third wrong answer");
                                                x = Console.ReadKey(true).KeyChar;
                                                if (x == 'n' || x == 'N' || x == 'b' || x == 'B')
                                                {

                                                }
                                                else if (x == '1')
                                                {
                                                    string NewQ;
                                                    Console.WriteLine("enter new question");
                                                    NewQ = Console.ReadLine();
                                                    q.Question1 = NewQ;
                                                }
                                                else if (x == '2')
                                                {
                                                    string cAns;
                                                    Console.WriteLine("enter new correct answer");
                                                    cAns = Console.ReadLine();
                                                    q.CorrectAnswer = cAns;
                                                }
                                                else if (x == '3')
                                                {
                                                    string wAns1;
                                                    Console.WriteLine("enter the new first spot wrong answer");
                                                    wAns1 = Console.ReadLine();
                                                    q.WrongAnswer1 = wAns1;
                                                }
                                                else if (x == '4')
                                                {
                                                    string wAns2;
                                                    Console.WriteLine("enter the new second spot wrong answer");
                                                    wAns2 = Console.ReadLine();
                                                    q.WrongAnswer2 = wAns2;
                                                }
                                                else if (x == '5')
                                                {
                                                    string wAns3;
                                                    Console.WriteLine("enter the new third spot wrong answer");
                                                    wAns3 = Console.ReadLine();
                                                    q.WrongAnswer3 = wAns3;
                                                }
                                                try
                                                {
                                                    db.UpdateQuestion(q);
                                                    Console.WriteLine("question got updated successfully - you can keep updating it");
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
                    if (c == '1')
                    {
                        if (db.CheckForAcceptionEligible(this.currentPlayer) == false)
                        {
                            Console.WriteLine("You are not eligible check the pending questions");
                            Console.WriteLine("press anything to continue");
                            c = char.Parse(Console.ReadLine());
                            //Console.ReadKey(true).KeyChar;
                        }
                        else
                        {
                            foreach (Question q in db.Questions)
                            {
                                    if (q.StatusId == 1 && l != 'b' && l != 'B')
                                    {
                                        db.ShowQuestion1(q);
                                        Console.WriteLine("If you want to accept the question press 1, if you want to eliminate the question press 2");
                                        Console.WriteLine("Press B to go back to the question screen or N to check the next question");
                                        l = Console.ReadKey(true).KeyChar;
                                        if (l == 'n' || l == 'N')
                                        {

                                        }
                                        else if (l == '1')
                                        {
                                            q.StatusId = 2;
                                            try
                                            {
                                                db.UpdateQuestion(q);
                                                Console.WriteLine("question approved");
                                                int W = q.WriterId;
                                                foreach (Player p in db.Players)
                                                {
                                                    if (p.PlayerId == W)
                                                    {
                                                        p.NumOfQuestions++;
                                                        db.UpdatePlayer(p);
                                                    }
                                                }
                                                
                                                db.RankUpdator();
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                        else if (l == '2')
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
                    if (c == '3')
                    {
                        if (db.CheckForManager(this.currentPlayer) == false)
                        {
                            Console.WriteLine("You are not eligible to add a subject - only for managers");
                            Console.WriteLine("press anything to continue");
                            c = char.Parse(Console.ReadLine());
                            //Console.ReadKey(true).KeyChar;
                        }
                        else
                        {
                            Console.WriteLine("Enter your subject");
                            string newSubject = Console.ReadLine();
                            try
                            {
                                Subject Sub = db.AddSubject(newSubject);
                                Console.WriteLine("Subject was added successfully");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
            }


            //Console.WriteLine("Press any key to continue...");
            //Console.ReadKey(true);
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
                        if (currentPlayer.Score > 90)
                        {
                            currentPlayer.Score = 100;
                        }
                        else
                        {
                            currentPlayer.Score = currentPlayer.Score + 10;

                        }
                        playerUpdate = true;
                    }
                    else
                    {
                        CleareAndTtile("GAME");
                        Console.WriteLine("YOU WRONG!!!");
                        if(currentPlayer.Score < 5)
                        {
                            currentPlayer.Score = 0;
                        }
                        else
                        {
                            currentPlayer.Score = currentPlayer.Score - 5;
                        }
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
                    CleareAndTtile("");
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
