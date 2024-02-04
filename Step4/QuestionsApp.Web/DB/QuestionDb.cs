using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuestionsApp.Web.DB
{
    public class QuestionDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; } = "";
        public ICollection<VoteDb> Votes { get; set; } = null!;
    }

}
