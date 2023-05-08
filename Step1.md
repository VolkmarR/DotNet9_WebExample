# Step 1 - Setup the project

## Create the Solution

* Create new, ASP.NET Core Web Api Project (c#, Linux, macOS, ...)
  * project name: QuestionsApp.Web
  * solution name: QuestionsApp
  * target framework: .NET 7.0
  * Authentication Type: None
  * Configure HTTPS: Not checked
  * Enable Docker: Not checked
  * Use controllers: Not checked
  * Enable OpenApi Support: Checked
  * Do not use top-level statements: Not checked
* Add new xUnit Test Project (c#, Linux, macOS, ...)
  * project name: QuestionsApp.Tests
  * target framework .NET 7.0
* Save all

## Add the MediatR Nuget Package

* Add the MediatR Package to the QuestionsApp.Web Dependencies
* Save all

## Add folders for the Web Api

* Add the folders in the QuestionsApp.Web project
  * Api
  * Api/Commands
  * Api/Queries

## Add the GetQuestionsQuery class

### Add a class GetQuestionsQuery in the Api/Queries/ folder 

* Add ```using MediatR```; 
* Save the file

### Add Request and Response Classes

<details><summary>Add the GetQuestionsResponse class with int ID, string Content, int Votes properies</summary>

~~~c#
public class GetQuestionsResponse
{
	public int ID { get; set; }
	public string Content { get; set; } = "";
	public int Votes { get; set; }
}
~~~
</details>


<details><summary>Add the GetQuestionsRequest class with IRequest&lt;List&lt;GetQuestionsResponse&gt&gt; interface</summary>

~~~c#
public class GetQuestionsRequest : IRequest<List<GetQuestionsResponse>>
{ }
~~~
</details>

### Setup the GetQuestionsQuery Request-Handler

<details><summary>Add the IRequestHandler&lt;GetQuestionsRequest, List&lt;GetQuestionsResponse&gt;&gt interface to the GetQuestionsQuery class and add the empty implementation</summary>

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

### Add a class AskQuestionCommand in the Api/Commands/ folder 

* Add ```using MediatR;``` 
* Save the file

### Add Request Class

<details><summary>Add the AskQuestionRequest class with IRequest&lt;IResult&gt; interface</summary>

~~~c#
public class AskQuestionRequest :IRequest<IResult>
{
	public string Content { get; set; } = "";
}
~~~
</details>

### Setup the AskQuestionCommand Request-Handler

<details><summary>Add the IRequestHandler&lt;AskQuestionRequest, IResult&gt interface to the AskQuestionCommand class and add the empty implementation</summary>

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

### Add a class VoteForQuestionCommand in the Api/Commands/ folder 

* Add ```using MediatR;``` 
* Save the file

### Add Request Class

<details><summary>Add the VoteForQuestionRequest class with IRequest&lt;IResult&gt; interface</summary>

~~~c#
public class VoteForQuestionRequest : IRequest<IResult>
{
	public int QuestionID { get; set; }
}
~~~
</details>

### Setup the VoteForQuestionCommand Request-Handler

<details><summary>Add the IRequestHandler&lt;VoteForQuestionRequest, IResult&gt interface to the VoteForQuestionCommand class and add the empty implementation</summary>

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

### Clean Up example code

<details><summary>Remove the Example code (Summary, app.MapGet and WeatherForecast record</summary>
 
~~~c#
// remove summaries
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// remove MapGet
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

// remove WeatherForecast
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
~~~
</details>

### Register MediatR Handlers

* Add ```using MediatR;```

<details><summary>Add the MediatR Registration after builder.Services.AddSwaggerGen()</summary>

~~~c#
builder.Services.AddSwaggerGen();
// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
~~~
</details>

### Add Maps for Queries and Commands

* Add ```using QuestionsApp.Web.Api.Commands;```
* Add ```using QuestionsApp.Web.Api.Queries;```

<details><summary>Add the query and command maps before the app.Run() statement </summary>
 
~~~c#
// Queries
app.MapGet("api/queries/questions", async (IMediator mediator) 
    => await mediator.Send(new GetQuestionsRequest()));

// Commands
app.MapPost("api/commands/questions/", async (IMediator mediator, string content) 
    => await mediator.Send(new AskQuestionRequest { Content = content }));

app.MapPost("api/commands/questions/{id:int}/vote", async (IMediator mediator, int id) 
    => await mediator.Send(new VoteForQuestionRequest { QuestionID = id }));

app.Run();
 ~~~
</details>

-----------------------


## Add Unittests for RequestHandlers

* Add a project refererence to the QuestionsApp.Web project in the QuestionsApp.Tests project
* Add the NuGet Package FluentAssertions
* Rename the Unittest1.cs file to QuestionsTests.cs
* Remove the Test1 Method in the QuestionsTests.cs
* Add ```using FluentAssertions;```
* Add ```using QuestionsApp.Web.Api.Commands;```
* Add ```using QuestionsApp.Web.Api.Queries;```

### Implement tests 

<details><summary>Helper methods to create new instances of the handlers</summary>

~~~c#
private GetQuestionsQuery NewGetQuestionsQueryHandler => new();
private AskQuestionCommand NewAskQuestionCommandHandler => new();
private VoteForQuestionCommand NewVoteForQuestionCommandHandler => new();
~~~
</details>


<details><summary>Empty Test</summary>

~~~c#
[Fact]
public async void Empty()
{
	var response = await NewGetQuestionsQueryHandler.Handle(new GetQuestionsRequest(), default);
	response.Should().BeEmpty();
}
~~~
</details>

<details><summary>OneQuestion</summary>

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

<details><summary>OneQuestionAndVote Test</summary>

~~~c#
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
~~~
</details>

