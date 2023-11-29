using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

public partial class Subject
{
    [Key]
    public int SubjectId { get; set; }

    [StringLength(30)]
    public string SubjectName { get; set; } = null!;

    [InverseProperty("Subject")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
