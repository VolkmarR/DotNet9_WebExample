# Step 1 - Setup the project

## Add the MediatR Nuget Package

* Add the MediatR Nuget Package to the QuestionsApp.Web Dependencies
* Save all

## Add folders for the Mediator Handlers

* Add the folders in the QuestionsApp.Web project
  * Handlers
  * Handlers/Commands
  * Handlers/Queries

## Add the GetQuestionsQuery class

### Add a class GetQuestionsQuery in the Handlers/Queries/ folder 

* Add ```using MediatR;```
* Remove the empty class GetQuestionsQuery 
* Save the file

### Add Request and Response Classes

<details><summary>Add the GetQuestionsResponse class with int ID, string Content, int Votes properties.</summary>

~~~c#
public class GetQuestionsResponse
{
	public int Id { get; set; }
	public string Content { get; set; } = "";
	public int Votes { get; set; }
}
~~~
</details>


<details><summary>Add the GetQuestionsRequest class with IRequest&lt;List&lt;GetQuestionsResponse&gt&gt; interface.</summary>

~~~c#
public class GetQuestionsRequest : IRequest<List<GetQuestionsResponse>>
{ }
~~~
</details>

### Setup the GetQuestionsQuery Request-Handler

<details><summary>Add the IRequestHandler&lt;GetQuestionsRequest, List&lt;GetQuestionsResponse&gt;&gt interface to the GetQuestionsQuery class and add the empty implementation.</summary>

~~~c#
public class GetQuestionsQuery : IRequestHandler<GetQuestionsRequest, List<GetQuestionsResponse>>
{
	public Task<List<GetQuestionsResponse>> Handle(GetQuestionsRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
~~~
</details>


## Add the AskQuestionCommand class

### Add a class AskQuestionCommand in the Handlers/Commands/ folder 

* Add ```using MediatR;``` 
* Remove the empty class AskQuestionCommand 
* Save the file

### Add Request Class

<details><summary>Add the AskQuestionRequest class with IRequest&lt;IResult&gt; interface.</summary>

~~~c#
public class AskQuestionRequest :IRequest<IResult>
{
	public string Content { get; set; } = "";
}
~~~
</details>

### Setup the AskQuestionCommand Request-Handler

<details><summary>Add the IRequestHandler&lt;AskQuestionRequest, IResult&gt interface to the AskQuestionCommand class and add the empty implementation.</summary>

~~~c#
public class AskQuestionCommand : IRequestHandler<AskQuestionRequest, IResult>
{
	public Task<IResult> Handle(AskQuestionRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
~~~
</details>

## Add the VoteForQuestionCommand class

### Add a class VoteForQuestionCommand in the Handlers/Commands/ folder 

* Add ```using MediatR;``` 
* Remove the empty class VoteForQuestionCommand 
* Save the file

### Add Request Class

<details><summary>Add the VoteForQuestionRequest class with IRequest&lt;IResult&gt; interface.</summary>

~~~c#
public class VoteForQuestionRequest : IRequest<IResult>
{
	public int QuestionId { get; set; }
}
~~~
</details>

### Setup the VoteForQuestionCommand Request-Handler

<details><summary>Add the IRequestHandler&lt;VoteForQuestionRequest, IResult&gt interface to the VoteForQuestionCommand class and add the empty implementation.</summary>

~~~c#
public class VoteForQuestionCommand : IRequestHandler<VoteForQuestionRequest, IResult>
{
	public Task<IResult> Handle(VoteForQuestionRequest request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
~~~
</details>


## Setup Minimal API in Program.cs File

### Register MediatR Handlers

* Add ```using MediatR;```

<details><summary>Add the MediatR Registration after builder.Services.AddSwaggerGen().</summary>

~~~c#
builder.Services.AddSwaggerGen();
// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
~~~
</details>

### Add Api Maps for Queries and Commands

* Add ```using QuestionsApp.Web.Handlers.Commands;```
* Add ```using QuestionsApp.Web.Handlers.Queries;```

<details><summary>Add the query and command maps before the app.Run() statement.</summary>
 
~~~c#
// Queries
app.MapGet("api/queries/questions", async (IMediator mediator) 
    => await mediator.Send(new GetQuestionsRequest()));

// Commands
app.MapPost("api/commands/questions/", async (IMediator mediator, string content) 
    => await mediator.Send(new AskQuestionRequest { Content = content }));

app.MapPost("api/commands/questions/{id:int}/vote", async (IMediator mediator, int id) 
    => await mediator.Send(new VoteForQuestionRequest { QuestionId = id }));

app.Run();
 ~~~
</details>

## Start swagger to check if the apis are shown

Run the QuestionsApp.Web application. A browser should open the page http://localhost:5000/swagger/index.html.

The page should show the three api routes: 
* GET /api/queries/questions
* PUT /api/commands/questions
* POST /api/commands/questions/{id}/vote

-----------------------


## Add Unittests for RequestHandlers

* Add the NuGet Package FluentAssertions
* Add a class QuestionsTests
* Add the using statements
  * ```using FluentAssertions;```
  * ```using QuestionsApp.Web.Handlers.Commands;```
  * ```using QuestionsApp.Web.Handlers.Queries;```

### Implement tests 

<details><summary>Helper methods to create new instances of the handlers.</summary>

~~~c#
private GetQuestionsQuery NewGetQuestionsQueryHandler => new();
private AskQuestionCommand NewAskQuestionCommandHandler => new();
private VoteForQuestionCommand NewVoteForQuestionCommandHandler => new();
~~~
</details>


<details><summary>Empty Test.</summary>

~~~c#
[Fact]
public async void Empty()
{
	var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
	response.Should().BeEmpty();
}
~~~
</details>

<details><summary>OneQuestion Test.</summary>

~~~c#
[Fact]
public async void OneQuestion()
{
	var askResponse = await NewAskQuestionCommandHandler.Handle(new AskQuestionRequest { Content = "Dummy Question" }, default);
	askResponse.Should().NotBeNull();

	var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
	response.Should().HaveCount(1);
}
~~~
</details>

<details><summary>OneQuestionAndVote Test.</summary>

~~~c#
[Fact]
public async void OneQuestionAndVote()
{
	var askResponse = await NewAskQuestionCommandHandler.Handle(new AskQuestionRequest { Content = "Dummy Question" }, default);
	askResponse.Should().NotBeNull();

	var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
	response.Should().HaveCount(1);
	response[0].Votes.Should().Be(0);

	var voteResponse = await NewVoteForQuestionCommandHandler.Handle(new VoteForQuestionRequest { QuestionId = response[0].Id }, default);
	voteResponse.Should().NotBeNull();

	response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
	response.Should().HaveCount(1);
	response[0].Votes.Should().Be(1);
}
~~~
</details>

### Run tests 

Run the unit tests. Since the handlers are not implemented yet, all three tests must fail. (red)