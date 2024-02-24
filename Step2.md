# Step 2 - Web Api implementation

## Add Nuget packages for database access 

### QuestionsApp.Web

* Add the following NuGet package to the QuestionsApp.Web project
  * Microsoft.EntityFrameworkCore.Sqlite
* Save All


### QuestionsApp.Test

* Add the following NuGet package to the QuestionsApp.Test project
  * Microsoft.EntityFrameworkCore.InMemory
* Save All


## Implement the database model

### Add folders and classes for the database model

* Add new folder DB to the QuestionsApp.Web project
* Add new classes for the database model
  * QuestionsContext.cs
  * QuestionDb.cs
  * VoteDb.cs

### Add properties for the database model

<details><summary>Add properties to QuestionDB class.</summary>
 
~~~c#
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public int Id { get; set; }
public string Content { get; set; } = "";
public ICollection<VoteDb> Votes { get; set; } = null!;
~~~
</details>

<details><summary>Add properties to VoteDB class.</summary>

~~~c#
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public int Id { get; set; }
public int QuestionId { get; set; }
public QuestionDb Question { get; set; } = null!;
~~~
</details>

### Fix the "Cannot resolve symbol" errors

Use the Context Action "Import missing references in file" to fix the "Cannot resolve symbol" errors. This action adds the missing using statements to the file.

### Implement the QuestionsContext

<details><summary>Add DbContext as base class for the QuestionsContext class. Add a constructor with an DbContextOptions options parameter for dependency injection.</summary>

~~~c#
public class QuestionsContext : DbContext
{
    public QuestionsContext(DbContextOptions options) : base(options)
    { }
}
~~~
</details>

<details><summary>Add DbSet for Questions and Votes to the  QuestionsContext class.</summary>

~~~c#
public DbSet<QuestionDb> Questions { get; set; }
public DbSet<VoteDb> Votes { get; set; }
~~~
</details>

### Configure EntityFramework in program.cs to use SQLite

<details><summary>Add the context to the Services and configure it to use the SQLite provider(before builder.Build()).</summary>

~~~c#
// Configuration for Entity Framework
var connectionString = new SqliteConnectionStringBuilder() { DataSource = "Production.db" }.ToString();
builder.Services.AddDbContext<QuestionsContext>(x => x.UseSqlite(connectionString));
~~~
</details>

<details><summary>Ensure, that the database exists (after builder.Build()).</summary>

~~~c#
// Make sure, that the database exists
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<QuestionsContext>().Database.EnsureCreated();
~~~
</details>


## Add the QuestionsContext as dependency to the request handlers

<details><summary>Add a constructor to GetQuestionsQuery for dependency injection.</summary>

~~~c#
private readonly QuestionsContext _context;
public GetQuestionsQuery(QuestionsContext context)
{
    _context = context;
}
~~~
</details>

<details><summary>Add a constructor to AskQuestionCommand for dependency injection.</summary>

~~~c#
private readonly QuestionsContext _context;
public AskQuestionCommand(QuestionsContext context)
{
    _context = context;
}
~~~
</details>

<details><summary>Add a constructor to VoteForQuestionCommand for dependency injection.</summary>

~~~c#
private readonly QuestionsContext _context;
public VoteForQuestionCommand(QuestionsContext context)
{
    _context = context;
}
~~~
</details>

### Fix the errors in the UnitTests

<details><summary>Add a QuestionContext property to the QuestionsTests class and initialize it in the constructor to use the InMemory provider.</summary>

~~~c#
private readonly QuestionsContext _context;

public QuestionsTests()
{
	var options = new DbContextOptionsBuilder<QuestionsContext>().
						UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
	_context = new QuestionsContext(options);
}
~~~
</details>

<details><summary>Change the helper methods to use the _context as parameter.</summary>

~~~c#
private GetQuestionsQuery NewGetQuestionsQueryHandler => new(_context);
private AskQuestionCommand NewAskQuestionCommandHandler => new(_context);
private VoteForQuestionCommand NewVoteForQuestionCommandHandler => new(_context);
~~~
</details>

## Implement Queries and Commands

### GetQuestionsQuery.cs

<details><summary>Make the Handle method async and implement it. The method reads the questions data using the context and returns a list of GetQuestionsResponse instances.</summary>

~~~c#
public async Task<List<GetQuestionsResponse>> Handle(GetQuestionsRequest request, CancellationToken cancellationToken)
{
    return await (from q in _context.Questions
                  select new GetQuestionsResponse { Id = q.Id, Content = q.Content, Votes = q.Votes.Count() }).ToListAsync(cancellationToken);
}
~~~
</details>

### AskQuestionCommand.cs

<details><summary>Make the Handle method async and implement it. The method uses the context to add a new entry to the questions table.</summary>

~~~c#
public async Task<IResult> Handle(AskQuestionRequest request, CancellationToken cancellationToken)
{
    if (string.IsNullOrWhiteSpace(request.Content))
        return Results.BadRequest("The Question Content can not be empty");

    _context.Questions.Add(new QuestionDb { Content = request.Content });
    await _context.SaveChangesAsync(cancellationToken);
    return Results.Ok();
}
~~~
</details>

### VoteForQuestionCommand.cs


<details><summary>Make the Handle method async and implement it. The method uses the context to add a new entry to the votes table.</summary>

~~~c#
public async Task<IResult> Handle(VoteForQuestionRequest request, CancellationToken cancellationToken)
{
    if (!await _context.Questions.AnyAsync(q => q.Id == request.QuestionId, cancellationToken))
        return Results.BadRequest("Invalid Question Id");

    _context.Votes.Add(new VoteDb { QuestionId = request.QuestionId });
    await _context.SaveChangesAsync(cancellationToken);
    return Results.Ok();
}
~~~
</details>

## Test the changes

* Run the unit tests to check, if the handlers are working as expected
* Run the web application and use the swagger ui to test the implementation manually
