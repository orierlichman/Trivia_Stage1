using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class Question
{
    [Key]
    public int QuestionId { get; set; }

    [Column("Question")]
    [StringLength(300)]
    public string Question1 { get; set; } = null!;

    public int SubjectId { get; set; }

    public int StatusId { get; set; }

    public int WriterId { get; set; }

    [StringLength(150)]
    public string CorrectAnswer { get; set; } = null!;

    [StringLength(150)]
    public string WrongAnswer1 { get; set; } = null!;

    [StringLength(150)]
    public string WrongAnswer2 { get; set; } = null!;

    [StringLength(150)]
    public string WrongAnswer3 { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Questions")]
    public virtual QuestionsStatus Status { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("Questions")]
    public virtual Subject Subject { get; set; } = null!;

    [ForeignKey("WriterId")]
    [InverseProperty("Questions")]
    public virtual Player Writer { get; set; } = null!;
}
