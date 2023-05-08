using MediatR;
using Microsoft.EntityFrameworkCore;
using QuestionsApp.Web.DB;

namespace QuestionsApp.Web.Api.Commands
{
    public class VoteForQuestionCommand : IRequestHandler<VoteForQuestionRequest, IResult>
    {
        private readonly QuestionsContext _context;
        public VoteForQuestionCommand(QuestionsContext context)
        {
            _context = context;
        }

        public async Task<IResult> Handle(VoteForQuestionRequest request, CancellationToken cancellationToken)
        {
            if (!await _context.Questions.AnyAsync(q => q.ID == request.QuestionID, cancellationToken))
                return Results.BadRequest("Invalid Question ID");

            _context.Votes.Add(new VoteDB { QuestionID = request.QuestionID });
            await _context.SaveChangesAsync(cancellationToken);
            return Results.Ok();
        }
    }

    public class VoteForQuestionRequest : IRequest<IResult>
    {
        public int QuestionID { get; set; }
    }
}
