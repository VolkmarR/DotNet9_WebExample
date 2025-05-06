# DotNet9_WebExample

This is an example project that shows, step by step, how to build a simple web application with asp.net core 9.0. 

## Result

The example is a simplified version of www.sli.do, a website for audiences to ask questions to the speaker.

It shows a list of questions. The users can add new questions to the list and vote for questions. 

## Used server side frameworks

* .Net 9
* Asp.net core 9
* MediatR 12.4.1
* XUnit 2.9.3
* FluentAssertions 8.0.1
* Swashbuckle 7.2.0
* Entity framework core 9
* Asp.net SignalR core 9

## Used client side frameworks
* Twitter Bootstrap 5.2.3
* Vue.js 3.4.15
* Axios 1.6.7
* SignalR 8.0.0
 
## Steps

To recreate this project, these five steps should be followed. For a detailed description of each step, click on the corresponding header. This repository also contains a folder for each step, that shows the state of the solution after completing the corresponding step.

### [Step 0 - Set up the projects](Step0.md)

Create new projects for the MinimalApi and the Unit Tests.

### [Step 1 - Setup MediatR, MinimalApi and Unit Tests](Step1.md)

Add the MediatR handlers and the MinimalApi.

### [Step 2 - Web Api implementation](Step2.md)

Add the implementation for the database access (SQLite for Api, InMemory for tests), queries and commands.

### [Step 3 - Add website](Step3.md)

Add the implementation of the frontend.

### [Step 4 - Client Refresh](Step4.md)

Add the implementation for the server-to-client communication for the automatic page refresh.
