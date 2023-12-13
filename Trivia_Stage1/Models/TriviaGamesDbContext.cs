using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class TriviaGamesDbContext : DbContext
{
    public TriviaGamesDbContext()
    {
    }

    public TriviaGamesDbContext(DbContextOptions<TriviaGamesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionsStatus> QuestionsStatuses { get; set; }

    public virtual DbSet<Rank> Ranks { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB;Initial Catalog=TriviaGames;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=False;Trust Server Certificate=False;Command Timeout=0;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PK_Player");

            entity.Property(e => e.Email).IsFixedLength();
            entity.Property(e => e.Name).IsFixedLength();
            entity.Property(e => e.Password).IsFixedLength();

            entity.HasOne(d => d.Rank).WithMany(p => p.Players)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Player");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.Property(e => e.CorrectAnswer).IsFixedLength();
            entity.Property(e => e.Question1).IsFixedLength();
            entity.Property(e => e.WrongAnswer1).IsFixedLength();
            entity.Property(e => e.WrongAnswer2).IsFixedLength();
            entity.Property(e => e.WrongAnswer3).IsFixedLength();

            entity.HasOne(d => d.Status).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionStatus");

            entity.HasOne(d => d.Subject).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionSubjcet");

            entity.HasOne(d => d.Writer).WithMany(p => p.Questions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_QuestionWriter");
        });

        modelBuilder.Entity<QuestionsStatus>(entity =>
        {
            entity.Property(e => e.StatusName).IsFixedLength();
        });

        modelBuilder.Entity<Rank>(entity =>
        {
            entity.Property(e => e.RankStatus).IsFixedLength();
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("PK_Subject");

            entity.Property(e => e.SubjectName).IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
