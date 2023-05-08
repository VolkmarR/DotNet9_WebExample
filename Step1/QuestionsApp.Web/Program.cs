using MediatR;
using QuestionsApp.Web.Api.Commands;
using QuestionsApp.Web.Api.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Queries
app.MapGet("api/queries/questions", async (IMediator mediator) 
    => await mediator.Send(new GetQuestionsRequest()));

// Commands
app.MapPost("api/commands/questions/", async (IMediator mediator, string content) 
    => await mediator.Send(new AskQuestionRequest { Content = content }));

app.MapPost("api/commands/questions/{id:int}/vote", async (IMediator mediator, int id) 
    => await mediator.Send(new VoteForQuestionRequest { QuestionID = id }));

app.Run();
