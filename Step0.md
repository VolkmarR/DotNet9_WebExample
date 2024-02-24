# Step 0 - Setup the project

## Create the Solution

* Create new, ASP.NET Core Web Api Project (c#, Linux, macOS, ...)
  * project name: QuestionsApp.Web
  * solution name: QuestionsApp
  * target framework: .NET 8.0
  * Authentication Type: None
  * Configure HTTPS: Not checked
  * Enable Docker: Not checked
  * Use controllers: Not checked
  * Enable OpenApi Support: Checked
  * Do not use top-level statements: Not checked
* Add new xUnit Test Project (c#, Linux, macOS, ...)
  * project name: QuestionsApp.Tests
    * target framework .NET 8.0
* Save all

## Setup the dependency from the QuestionsApp.Tests project to the QuestionsApp.Web project

* Add a reference to the QuestionsApp.Web project in the dependencies of the QuestionsApp.Tests project

## Clean Up example code

### Remove Minimal API example in Program.*cs

<details><summary>Remove the Example code (Summary, app.MapGet and WeatherForecast record).</summary>
 
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
