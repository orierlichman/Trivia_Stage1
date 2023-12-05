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
        return player;
    }

    public Player Login(string email, string password)
    {
        Player? p = Players.Where(pp => pp.Email == email && pp.Password == password).Include(pp => pp.Rank).FirstOrDefault();
        return p;
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

    public void ShowSubjects()
    {
        foreach (Subject s in this.Subjects)
        {
            Console.WriteLine("Enter " + s.SubjectId + " for " + s.SubjectName);
        }
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


    public void ShowQuestion(Question q)
    {
        Console.WriteLine("Question : " + q.Question1);
        Console.WriteLine("Correct Answer : " + q.CorrectAnswer);
        Console.WriteLine("Wrong Answer 1 : " + q.WrongAnswer1);
        Console.WriteLine("Wrong Answer 2 : " + q.WrongAnswer2);
        Console.WriteLine("Wrong Answer 3 : " + q.WrongAnswer3);
        Console.WriteLine("Question Writer : " + (Players.Where(pp => pp.PlayerId == q.WriterId)).
    }

}
