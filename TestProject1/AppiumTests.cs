using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace TestProject1
{
    public class Tests
    {
        private const string AppiumUrl = "http://127.0.0.1:4723/wd/hub";
        private const string ContactsBookUrl = "https://f0aee227-7ac0-432d-b64c-eba88613adad.id.repl.co/api";
        private const string appLocation = @"C:\TaskBoard\TaskBoard.DesktopClient.exe";

        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability("app", appLocation);

            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }


        [Test]
        public void Test_Find()
        {
            var urlField = driver.FindElementByAccessibilityId("textBoxApiUrl");
            urlField.Clear();
            urlField.SendKeys(ContactsBookUrl);

            var buttonConnect = driver.FindElementByAccessibilityId("buttonConnect");
            buttonConnect.Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            var allTasks = driver.FindElementByAccessibilityId("ListViewSubItem-1");
            //Assert.That(allTaska.)


        }
    }
}