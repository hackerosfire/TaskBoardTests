using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;

namespace TaskBoardTests.SeleniumTests
{
    public class Tests
    {
        private const string url = "https://f0aee227-7ac0-432d-b64c-eba88613adad.id.repl.co/";
        private WebDriver driver;
        [SetUp]
        public void Setup()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void Teardown()
        {
            this.driver.Quit();
        }

        [Test]
        public void Test_ListTasks_VerifyFirstTaskTitle()
        {
            driver.Navigate().GoToUrl(url);
            var tasksLink = driver.FindElement(By.LinkText("Task Board"));
            tasksLink.Click();
            var doneFirstElementTitleText = driver.FindElement(By.CssSelector("#task1 > tbody > tr.title > td")).Text;
            Assert.That(doneFirstElementTitleText, Is.EqualTo("Project skeleton"));
        }

        [Test]
        public void Test_SearchTasks_VerifyFirstResultTitle()
        {
            driver.Navigate().GoToUrl(url);
            var SearchButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3) > span.icon"));
            SearchButton.Click();
            var searchTextField = driver.FindElement(By.Id("keyword"));
            searchTextField.Click();
            searchTextField.SendKeys("home");
            var SearchPageButton= driver.FindElement(By.Id("search"));
            SearchPageButton.Click();
            var FirstResultTitleText = driver.FindElement(By.CssSelector("#task2 > tbody > tr.title > td")).Text;
            Assert.That(FirstResultTitleText, Is.EqualTo("Home page"));

        }
        [Test]
        public void Test_SearchTasksWithInvalidKeyword_VerifyResultsEmpty()
        {
            Random random = new Random();
            driver.Navigate().GoToUrl(url);
            var SearchButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(3) > span.icon"));
            SearchButton.Click();
            var searchTextField = driver.FindElement(By.Id("keyword"));
            searchTextField.Click();
            searchTextField.SendKeys("missing"+random.Next(1,1000));
            var SearchPageButton = driver.FindElement(By.Id("search"));
            SearchPageButton.Click();
            var searchResults = driver.FindElement(By.Id("searchResult")).Text;
            Assert.That(searchResults, Is.EqualTo("No tasks found."));

        }

        [Test]
        public void Test_CreateTaskInvalidData_VerifyErrorReturned()
        {
            driver.Navigate().GoToUrl(url);
            var newTaskButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2) > span.icon"));
            newTaskButton.Click();
            var newTaskDescription = driver.FindElement(By.Id("description"));
            newTaskDescription.Click();
            newTaskDescription.SendKeys("description 123");
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();
            var errorMsg = driver.FindElement(By.CssSelector("body > main > div")).Text;
            Assert.That(errorMsg, Is.EqualTo("Error: Title cannot be empty!"));
        }

        [Test]
        public void Test_CreateTaskValidData_VerifyIsProperlyAdded()
        {
            driver.Navigate().GoToUrl(url);
            Random random = new Random();
            var newTaskButton = driver.FindElement(By.CssSelector("body > main > div > a:nth-child(2) > span.icon"));
            newTaskButton.Click();
            var newTaskTitle = driver.FindElement(By.Id("title"));
            newTaskTitle.Click();
            var inputTitle = "title" + random.Next(1, 100);
            newTaskTitle.SendKeys(inputTitle);
            var newTaskDescription = driver.FindElement(By.Id("description"));
            newTaskDescription.Click();
            var inputDescription = "description 123";
            newTaskDescription.SendKeys(inputDescription);
            var createButton = driver.FindElement(By.Id("create"));
            createButton.Click();
            driver.Navigate().GoToUrl(url+"/boards");
            var allTasks = driver.FindElements(By.CssSelector(".tasks-grid .task:nth-child(1) .task-entry"));
            var lastTask = allTasks.Last();
            var titleLabel = lastTask.FindElement(By.CssSelector("tr.title > td")).Text;
            var descriptionLabel = lastTask.FindElement(By.CssSelector("tr.description > td > div")).Text;

            Assert.That(titleLabel, Is.EqualTo(inputTitle));
            Assert.That(descriptionLabel, Is.EqualTo(inputDescription));
        }
    }
}