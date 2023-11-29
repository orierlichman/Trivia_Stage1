using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Trivia_Stage1.Models;

[Table("QuestionsStatus")]
public partial class QuestionsStatus
{
    [Key]
    public int QuestionStatusId { get; set; }

    [StringLength(30)]
    public string StatusName { get; set; } = null!;

    [InverseProperty("Status")]
    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
