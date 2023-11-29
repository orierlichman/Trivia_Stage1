using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class Rank
{
    [Key]
    public int RankId { get; set; }

    [StringLength(30)]
    public string RankStatus { get; set; } = null!;

    [InverseProperty("Rank")]
    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
