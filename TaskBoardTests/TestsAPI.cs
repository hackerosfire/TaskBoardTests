using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using TaskBoardTests.APITests;

namespace TaskBoardTests
{
    public class Tests
    {
        private const string url = "https://f0aee227-7ac0-432d-b64c-eba88613adad.id.repl.co/api";
        private RestClient client;
        private RestRequest request;
        [SetUp]
        public void Setup()
        {
            this.client = new RestClient();
        }

        [Test]
        public void Test_GetDoneBoard_VerifyFirstTaskTitle()
        {
            // /tasks/board/
            this.request = new RestRequest(url + "/tasks/board/Done");
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            Assert.That(tasks[0].title.ToString, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_GetTasks_VerifyFirstTaskTitle()
        {
            this.request = new RestRequest(url + "/tasks/search/home");
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            //Home page
            Assert.That(tasks[0].title, Is.EqualTo("Home page"));
        }

        [Test]
        public void Test_GetTasksByRandKeyword_VerifyResultIsNull()
        {
            Random random = new Random();
            this.request = new RestRequest(url + "/tasks/search/missing"+random.Next(1,1000));
            var response = this.client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            //Home page
            Assert.That(tasks.Count, Is.EqualTo(0));
        }

        [Test]
        public void Test_AddNewTaskWithWrongData_VerifyResultIsError()
        {
            this.request = new RestRequest(url + "/tasks",Method.Post);
            var response = this.client.Execute(request);
            //var tasks = JsonSerializer.Deserialize<List<Task>>(response.Content);
            //Home page
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));
        }

        [Test]
        public void Test_CreateTask_VerifyIsProperlyAdded()
        {
            this.request = new RestRequest(url + "/tasks", Method.Post);
            var requestBody = new
            {
                title = "asdasd",
                description="description",
                board = "Open"
            };
            request.AddJsonBody(requestBody);
            var response = this.client.Execute(request);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            var allTasks = this.client.Execute(request, Method.Get);
            var tasks = JsonSerializer.Deserialize<List<Task>>(allTasks.Content);
            Assert.That(tasks.Last().title, Is.EqualTo(requestBody.title));
            Assert.That(tasks.Last().description, Is.EqualTo(requestBody.description));
            Assert.That(tasks.Last().board.name, Is.EqualTo(requestBody.board));
        }
    }
}