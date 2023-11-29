using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class Player
{
    [Key]
    public int PlayerId { get; set; }

    [StringLength(20)]
    public string Name { get; set; } = null!;

    [StringLength(40)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    public string Password { get; set; } = null!;

    public int Score { get; set; }

    public int RankId { get; set; }

    public int NumOfQuestions { get; set; }

    [InverseProperty("Writer")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    [ForeignKey("RankId")]
    [InverseProperty("Players")]
    public virtual Rank Rank { get; set; } = null!;
}
