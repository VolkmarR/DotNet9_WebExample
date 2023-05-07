# Step 2 - Web Api implementation

## Add Nuget packages for database access 

### QuestionsApp.Web

* Add the following NuGet package to the QuestionsApp.Web project
  * Microsoft.EntityFrameworkCore.Sqlite
  * Microsoft.EntityFrameworkCore.Sqlite.Core
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
  * QuestionDB.cs
  * VoteDB.cs

### Add properties for the database model

<details><summary>Add properties to QuestionDB</summary>
 
~~~c#
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public int ID { get; set; }
public string Content { get; set; } = "";
public ICollection<VoteDB> Votes { get; set; } = null!;
~~~
</details>

<details><summary>Add properties to VoteDB</summary>

~~~c#
[Key]
[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
public int ID { get; set; }
public int QuestionID { get; set; }
public QuestionDB Question { get; set; } = null!;
~~~
</details>

### Implement the QuestionsContext

<details><summary>Add DbContext as base class and add constructor with DbContextOptions parameter for dependency</summary>

~~~c#
public class QuestionsContext : DbContext
{
    public QuestionsContext(DbContextOptions options) : base(options)
    { }
}
~~~
</details>

<details><summary>Add DbSet for Questions and Votes to QuestionsContext</summary>

~~~c#
public DbSet<QuestionDB> Questions { get; set; }
public DbSet<VoteDB> Votes { get; set; }
~~~
</details>

### Configure EntityFramework in program.cs to use SQLite

<details><summary>Add the context to the Sevices</summary>

~~~c#
// Configuration for Entity Framework
var connectionString = new SqliteConnectionStringBuilder() { DataSource = "Production.db" }.ToString();
builder.Services.AddDbContext<QuestionsContext>(x => x.UseSqlite(connectionString));
~~~
</details>

<details><summary>Ensure, that the database exists (after builder.Build())</summary>

~~~c#
// Make sure, that the database exists
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<QuestionsContext>().Database.EnsureCreated();
~~~
</details>


## Add the QuestionsContext as dependency to the request handlers

<details><summary>Add a constructor to GetQuestionsQuery for dependency injection</summary>

~~~c#
private readonly QuestionsContext _context;
public GetQuestionsQuery(QuestionsContext context)
{
    _context = context;
}
~~~
</details>

<details><summary>Add a constructor to AskQuestionCommand for dependency injection</summary>

~~~c#
private readonly QuestionsContext _context;
public AskQuestionCommand(QuestionsContext context)
{
    _context = context;
}
~~~
</details>

<details><summary>Add a constructor to VoteForQuestionCommand for dependency injection</summary>

~~~c#
private readonly QuestionsContext _context;
public VoteForQuestionCommand(QuestionsContext context)
{
    _context = context;
}
~~~
</details>

### Fix the errors in the UnitTests

<details><summary>Add a QuestionContext property to the QuestionsTests class and initialize it in the constructor</summary>

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

<details><summary>Change the helper methods to use the QuestionsContext</summary>

~~~c#
private GetQuestionsQuery NewGetQuestionsQueryHandler => new(_context);
private AskQuestionCommand NewAskQuestionCommandHandler => new(_context);
private VoteForQuestionCommand NewVoteForQuestionCommandHandler => new(_context);
~~~
</details>

## Implement Queries and Commands

### GetQuestionsQuery.cs

<details><summary>Implement the Handle method</summary>

~~~c#
public async Task<List<GetQuestionsResponse>> Handle(GetQuestionsRequest request, CancellationToken cancellationToken)
{
    return await(from q in _context.Questions
                  select new GetQuestionsResponse { ID = q.ID, Content = q.Content, Votes = q.Votes.Count() }).ToListAsync(cancellationToken);
}
~~~
</details>

### AskQuestionCommand.cs

<details><summary>Implement the Handle method</summary>

~~~c#
public async Task<IResult> Handle(AskQuestionRequest request, CancellationToken cancellationToken)
{
    if (string.IsNullOrWhiteSpace(request.Content))
        return Results.BadRequest("The Question Content can not be empty");

    _context.Questions.Add(new QuestionDB { Content = request.Content });
    await _context.SaveChangesAsync(cancellationToken);
    return Results.Ok();
}
~~~
</details>

### VoteForQuestionCommand.cs


<details><summary>Implement the Handle method</summary>

~~~c#
public async Task<IResult> Handle(VoteForQuestionRequest request, CancellationToken cancellationToken)
{
    if (!await _context.Questions.AnyAsync(q => q.ID == request.QuestionID, cancellationToken))
        return Results.BadRequest("Invalid Question ID");

    _context.Votes.Add(new VoteDB { QuestionID = request.QuestionID });
    await _context.SaveChangesAsync(cancellationToken);
    return Results.Ok();
}
~~~
</details>

### Run the UnitTests to check, if the Controller are working as expected
