# DotNetCore70_WebExample

This is an example project that shows step by step, how to build a simple web application with asp.net core 7.0. 

## Result

The example is a simplified version of www.sli.do. That is a website for audiences to ask questions to the speaker.

It shows a list of questions. The users can add new questions to the list and vote for questions. 

## Used server side frameworks

* .Net 7
* Asp.net core 7
* MediatR 12.0.1
* XUnit 2.4.2
* FluentAssertions 6.11.0
* Swashbuckle 6.5.0
* Entity framework core 7
* Asp.net SignalR core 7

## Used client side frameworks
* Twitter Bootstrap 5.2.3
* Vue.js 3.2.47
* Axios 1.3.4
* SignalR 7.0.3
 
## Steps

To recreate this project, these four steps should be followed. For a detailed description of each step, click on the corresponding header. To see the state of the project after each step, you can switch to the corresponding branch in this repository.

### [Step 1 - Setup the project](Step1.md)

Create new projects for the MinimalApi and the Unit Tests, and add the Swagger UI.

### [Step 2 - Web Api implementation](Step2.md)

Add the implementation for the database access, commands, queries,

### [Step 3 - Add website](Step3.md)

Add the implementation of the website.

### [Step 4 - Client Refresh](Step4.md)

Add the implementation for the server-to-client communication for the page refresh.
