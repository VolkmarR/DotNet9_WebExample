using FluentAssertions;
using QuestionsApp.Web.Api.Commands;
using QuestionsApp.Web.Api.Queries;

namespace QuestionsApp.Tests
{
    public class QuestionsTests
    {
        private GetQuestionsQuery NewGetQuestionsQueryHandler => new();
        private AskQuestionCommand NewAskQuestionCommandHandler => new();
        private VoteForQuestionCommand NewVoteForQuestionCommandHandler => new();

        [Fact]
        public async void Empty()
        {
            var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
            response.Should().BeEmpty();
        }

        [Fact]
        public async void OneQuestion()
        {
            var askResponse = await NewAskQuestionCommandHandler.Handle(new AskQuestionRequest { Content = "Dummy Question" }, default);
            askResponse.Should().NotBeNull();

            var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
            response.Should().HaveCount(1);
        }

        [Fact]
        public async void OneQuestionAndVote()
        {
            var askResponse = await NewAskQuestionCommandHandler.Handle(new AskQuestionRequest { Content = "Dummy Question" }, default);
            askResponse.Should().NotBeNull();

            var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
            response.Should().HaveCount(1);
            response[0].Votes.Should().Be(0);

            var voteResponse = await NewVoteForQuestionCommandHandler.Handle(new VoteForQuestionRequest { QuestionID = response[0].ID }, default);
            voteResponse.Should().NotBeNull();

            response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
            response.Should().HaveCount(1);
            response[0].Votes.Should().Be(1);
        }
    }
}