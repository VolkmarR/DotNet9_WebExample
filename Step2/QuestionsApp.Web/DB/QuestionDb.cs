using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QuestionsApp.Web.Db
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
