using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class TriviaGamesDbContext : DbContext
{
    const int PLAYER_ROOKIE = 3;
    public Player Signup(string email, string password, string name)
    {
        Player player = new Player();
        {
            player.Email = email;
            player.Password = password;
            player.Name = name;
            player.Score = 0;
            player.RankId = PLAYER_ROOKIE;
            player.NumOfQuestions = 0;
        }
        this.Players.Add(player);
        this.SaveChanges();

        return Login(player.Email, player.Password);
        
    }

    public Player Login(string email, string password)
    {
        Player? p = Players.Where(pp => pp.Email == email && pp.Password == password).Include(pp => pp.Rank).FirstOrDefault();
        return p;
    }

    public void RankUpdator()
    {
        foreach (Player p in this.Players)
        {
            if (p.RankId == 1 || p.RankId == 2 && p.NumOfQuestions > 9 || p.RankId == 3 && p.NumOfQuestions < 10)
            {

            }
            else 
            { 
                if (p.NumOfQuestions > 9)
                {
                    p.RankId = 2;
                }
                else
                {
                    p.RankId = 3;
                }
            }
            try
            {
                UpdatePlayer(p);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

    public bool AddEligible (Player p)
    {
        if (p.RankId == 1)
        {
            return true;
        }
        else if (p.Score == 100)
        {
            return true;
        }
        return false;
    }


    public Question AddQuestion(Player p ,string question, string CorrectAns, string WrongAns1, string WrongAns2, string WrongAns3, int SubId)
    {
        Question Q = new Question();
        {
            Q.Question1 = question;
            Q.CorrectAnswer = CorrectAns;
            Q.WrongAnswer1 = WrongAns1;
            Q.WrongAnswer2 = WrongAns2;
            Q.WrongAnswer3 = WrongAns3;
            Q.SubjectId = SubId;
            Q.StatusId = 1;
            Q.WriterId = p.PlayerId;
        }
        this.Questions.Add(Q);
        this.SaveChanges();
        return Q;
    }

    public Subject AddSubject (string sub)
    {
        Subject s = new Subject();
        {
            s.SubjectName = sub;
        }
        this.Subjects.Add(s);
        this.SaveChanges();
        return s;
    }

    public void ResetScores()
    {
        foreach (Player player in this.Players)
        {
            player.Score = 0;
        }
    }

    public bool CheckForManager(Player p)
    {
        if (p.RankId == 1)
        {
            return true;
        }
        return false;
    }


    public bool CheckForAcceptionEligible(Player p)
    {
        if (p.RankId != 3)
        {
            return true;
        }
        return false;
    }


    public void UpdatePlayer(Player p)
    {
        Entry(p).State = EntityState.Modified;
        SaveChanges();
    }

    public void UpdateQuestion(Question q)
    {
        Entry(q).State = EntityState.Modified;
        SaveChanges();
    }

    public void ShowQuestion1(Question q)
    {
        Console.WriteLine("Question : " + q.Question1);
        Console.WriteLine("Correct Answer : " + q.CorrectAnswer);
        Console.WriteLine("Wrong Answer 1 : " + q.WrongAnswer1);
        Console.WriteLine("Wrong Answer 2 : " + q.WrongAnswer2);
        Console.WriteLine("Wrong Answer 3 : " + q.WrongAnswer3);
    }


    public List<Question> GetApprovedQuestions()
    {
        return this.Questions.Where(q => q.StatusId == 2).ToList();
    }

    public int ShowQuestion2(Question q)
    {
        Random random = new Random();
        int correct = random.Next(1, 5);
        return correct;
    }
}
