using MediatR;

namespace QuestionsApp.Web.Api.Commands
{
    public class VoteForQuestionCommand : IRequestHandler<VoteForQuestionRequest, IResult>
    {
        public Task<IResult> Handle(VoteForQuestionRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class VoteForQuestionRequest : IRequest<IResult>
    {
        public int QuestionID { get; set; }
    }
}
