# Step 4 - Client Refresh

## Server Configuration

* Add the new folder Hubs to Question.Web
* Add new class QuestionsHub to folder Hub
* Set the base class of the QuestionsHub class to Hub

<details><summary>Add a static class with an extension method for the refresh command</summary>

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

### Configure and activate SignalR in program.cs

<details><summary>ConfigureServices</summary>

~~~c#
// Configuration for SignalR
builder.Services.AddSignalR();
~~~
</details>

<details><summary>Configure (after the Queries and Command Maps)</summary>

~~~c#
// Activate SignalR Hub
app.MapHub<QuestionsHub>("/hub");
~~~
</details>

## Add calls to the hub

### Commands\AskQuestionCommand

<details><summary>Add a IHubContext<QuestionsHub> as parameter to the constructor for dependency injection</summary>

~~~c#
private readonly IHubContext<QuestionsHub>? _hub;
public AskQuestionCommand(QuestionsContext context, IHubContext<QuestionsHub>? hub)
{
    _context = context;
    _hub = hub;
}
~~~
</details>

<details><summary>Call the refresh SendRefreshAsync in the handle method after the save</summary>

~~~c#
await _hub.SendRefreshAsync();
~~~
</details>

### Commands\VoteForQuestionCommand

<details><summary>Add a IHubContext<QuestionsHub> as parameter to the constructor for dependency injection</summary>

~~~c#
private readonly IHubContext<QuestionsHub>? _hub;
public VoteForQuestionCommand(QuestionsContext context, IHubContext<QuestionsHub>? hub)
{
    _context = context;
    _hub = hub;
}
~~~
</details>

<details><summary>Call the refresh SendRefreshAsync in the handle method after the save</summary>

~~~c#
await _hub.SendRefreshAsync();
~~~
</details>


### Fix the unit tests

<details><summary>Modify the initialisation of the RequestHandlers</summary>

~~~c#
private GetQuestionsQuery GetQuestionsQueryHandler => new(_context);
private AskQuestionCommand AskQuestionCommandHandler => new(_context, null);
private VoteForQuestionCommand VoteForQuestionCommandHandler => new(_context, null);
~~~
</details>

## Web site Implementation

<details><summary>Add SignalR script import</summary>

~~~html
<script src="https://cdn.jsdelivr.net/npm/@microsoft/signalr@7.0.3/dist/browser/signalr.min.js" integrity="sha256-zvQeaEXmmM78llGmEtKvwp5dG1kF3iJ3GhdjrO4b+fg=" crossorigin="anonymous"></script>

<title>Ask your questions</title>
~~~
</details>

Remove the call of getQuestions from the methods ask and vote

<details><summary>Add connection to hub and register to the refresh message to call getQuestions</summary>

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