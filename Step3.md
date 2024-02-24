# Step 3 - Add web site

## Server Configuration

* Add a new folder called wwwroot

### Activate Static Files in the startup

<details><summary>Activate the static file serving in the program.cs (before the app.Map... calls).</summary>

~~~c#
// activate static files serving (before the Map...)
app.UseDefaultFiles();
app.UseStaticFiles();
~~~
</details>

## Website

* Add an index.html file to wwwroot

<details><summary>Add Twitter Bootstrap, Vue.JS and axios scripts imports.</summary>

~~~Html
<head>
    <!-- Required meta tags -->
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">

    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" integrity="sha256-MBffSnbbXwHCuZtgPYiwMQbfE7z+GOZ7fBPCNB06Z98=" crossorigin="anonymous">    <script src="https://cdn.jsdelivr.net/npm/vue@3.2.47/dist/vue.global.min.js"></script>
    <!-- Vue -->
    <script src="https://cdn.jsdelivr.net/npm/vue@3.4.15/dist/vue.global.min.js"></script>
    <!-- Axios -->
    <script src="https://cdn.jsdelivr.net/npm/axios@1.6.7/dist/axios.min.js" integrity="sha256-NDS2dZXGgHGCThQtB3zn4QXUCsQLFRZIltEeVAeNAhM=" crossorigin="anonymous"></script>
    <title>Ask your questions</title>
</head>
<body>
    <!-- Optional JavaScript -->
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.min.js" integrity="sha256-YMa+wAM6QkVyz999odX7lPRxkoYAan8suedu4k2Zur8=" crossorigin="anonymous"></script>
</body>
~~~
</details>

<details><summary>Create the layout</summary>

~~~Html
<nav class="navbar navbar-dark bg-dark">
    <div class="container-fluid">
        <a class="navbar-brand" href="#">Questions</a>
        <button class="btn btn-light my-2 my-sm-0" data-bs-toggle="collapse" data-bs-target="#questionBox">Ask</button>
    </div>
</nav>

<div id="questionView">
    <div class="collapse" id="questionBox">
        <div class="container mt-4">
            <div class="card card-body">
                <div class="row">
                    <div class="col-11">
                        <input type="text" class="form-control" v-model="newQuestion" placeholder="Question">
                    </div>
                    <div class="col-1">
                        <button class="btn btn-primary btn-block" v-on:click="ask"
                                data-bs-toggle="collapse" data-bs-target="#questionBox">Send</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="container mt-4">
        <table class="table">
            <thead>
                <tr>
                    <th scope="col" class="col-8">Question</th>
                    <th scope="col" class="col-3">Votes</th>
                    <th scope="col" class="col-1"></th>
                </tr>
            </thead>
            <tbody>
                <!-- display a tablerow for every item in the questionList -->
                <tr v-for="item in questionList">
                    <td>{{ item.content }}</td>
                    <td>{{ item.votes }}</td>
                    <td>
                      <button class="btn" style="font-size: xx-small" v-on:click="vote(item)">&#x25B2;</button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
~~~
</details>
  
<details><summary>Add the Vue app to make the site dynamic (after the layout).</summary>

~~~Html
<script>
    // vue app for the page
    var app = Vue.createApp({
        data: () => ({
            // the list of questions, displayed in the table
            questionList: [],
            // the value of the "add new question" edit
            newQuestion: '',
        }),
        methods: {
            // adds a new question to the list
            add: function (event) {
                // if the question is empty, call stopPropagation to stop twitter bootstrap from collapsing the card
                if (this.newQuestion === '') {
                    event.stopPropagation();
                    return;
                }
                // call the api to add a new question
                axios
                    .post('api/commands/questions', null, { params: { content: this.newQuestion } })
                    .then(() => {
                        this.getQuestions();
                        this.newQuestion = '';
                    })
                    .catch(function (error) { alert(error.response.data); });
            },
            vote: function (question) {
                // call the api to increment the votes of the question
                axios
                    .post('api/commands/questions/' + question.id + '/vote')
                    .then(() => this.getQuestions())
                    .catch(function (error) { alert(error.response.data); });
            },
            getQuestions: function () {
                axios
                    .get('api/queries/questions')
                    .then(response => (this.questionList = response.data))
            }
        },
        mounted: function () {
            this.getQuestions();
        }
    });
    app.mount("#questionView");
</script>
~~~
</details>

## Test the changes in the browser

Run the web application and open the url http://localhost:5000/. Try if you can add questions and vote on them.