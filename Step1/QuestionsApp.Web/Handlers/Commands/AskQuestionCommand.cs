using MediatR;

namespace QuestionsApp.Web.Api.Commands
{
    public class AskQuestionCommand : IRequestHandler<AskQuestionRequest, IResult>
    {
        public Task<IResult> Handle(AskQuestionRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }

    public class AskQuestionRequest :IRequest<IResult>
    {
        public string Content { get; set; } = "";
    }

}
