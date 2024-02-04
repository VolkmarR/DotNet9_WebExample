using Microsoft.EntityFrameworkCore;

namespace QuestionsApp.Web.DB
{
    public class QuestionsContext : DbContext
    {
        public QuestionsContext(DbContextOptions options) : base(options)
        { }

        public DbSet<QuestionDb> Questions { get; set; }
        public DbSet<VoteDb> Votes { get; set; }
    }
}
