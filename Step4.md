# Step 4 - Client Refresh

## Server Configuration

* Add the new folder Hubs to Question.Web
* Add new class QuestionsHub to folder Hub
* Set the base class of the QuestionsHub class to Hub

<details><summary>Add a static class with an extension method for the refresh command.</summary>

~~~c#
public static class QuestionsHubExtensions
{
    public static async Task SendRefreshAsync(this IHubContext<QuestionsHub>? hub)
    {
        if (hub != null)
            await hub.Clients.All.SendAsync("refresh");
    }
}
~~~
</details>

### Register and activate SignalR in program.cs

<details><summary>Register SignalR.</summary>

~~~c#
// Configuration for SignalR
builder.Services.AddSignalR();
~~~
</details>

<details><summary>Activate (after the Queries and Command Maps).</summary>

~~~c#
// Activate SignalR Hub
app.MapHub<QuestionsHub>("/hub");
~~~
</details>

## Add calls to the hub

### Commands\AskQuestionCommand

<details><summary>Add a IHubContext<QuestionsHub> as parameter to the constructor for dependency injection.</summary>

~~~c#
private readonly IHubContext<QuestionsHub>? _hub;
public AskQuestionCommand(QuestionsContext context, IHubContext<QuestionsHub>? hub)
{
    _context = context;
    _hub = hub;
}
~~~
</details>

<details><summary>Call the refresh SendRefreshAsync in the handle method after the SaveChangesAsync call.</summary>

~~~c#
await _hub.SendRefreshAsync();
~~~
</details>

### Commands\VoteForQuestionCommand

<details><summary>Add a IHubContext<QuestionsHub> as parameter to the constructor for dependency injection.</summary>

~~~c#
private readonly IHubContext<QuestionsHub>? _hub;
public VoteForQuestionCommand(QuestionsContext context, IHubContext<QuestionsHub>? hub)
{
    _context = context;
    _hub = hub;
}
~~~
</details>

<details><summary>Call the refresh SendRefreshAsync in the handle method after the SaveChangesAsync call.</summary>

~~~c#
await _hub.SendRefreshAsync();
~~~
</details>


### Fix the unit tests

<details><summary>Modify the initialisation of the RequestHandlers.</summary>

~~~c#
private GetQuestionsQuery GetQuestionsQueryHandler => new(_context);
private AskQuestionCommand AskQuestionCommandHandler => new(_context, null);
private VoteForQuestionCommand VoteForQuestionCommandHandler => new(_context, null);
~~~
</details>

## Web site Implementation

<details><summary>Add SignalR script import</summary>

~~~html
<!-- SignalR -->
<script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@8.0.0/dist/browser/signalr.min.js" integrity="sha256-+k7RplYeBZa1wx3zb0fBaSYRX6xF3PAvq1Cgp55YC04=" crossorigin="anonymous"></script>
<title>Ask your questions</title>
~~~
</details>

Remove the call of getQuestions from the methods add and vote.

<details><summary>Add connection to hub and register to the refresh message to call getQuestions.</summary>

~~~js
// app.mount("#questionView");
const vm = app.mount("#questionView");

const connection = new signalR.HubConnectionBuilder()
    .withUrl("hub")
    .build();
connection.start().catch(err => console.error(err.toString()));
connection.on("Refresh", () => { console.log("Refresh"); vm.getQuestions(); });
~~~
</details>

## Test the changes

* Run the unit tests to see if they still work
* Run the web application and open the url http://localhost:5000/ in two browser tabs. 
  * Add a question on one tab
  * Check if the question is shown on the other tab
  * Vote on a question on one tab
  * Check if the vote is shown on the other tab
