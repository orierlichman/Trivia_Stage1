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
        Player p = Players.Where(pp => pp.Email == email && pp.Password == password).FirstOrDefault();
        return p;
    }
    
}
