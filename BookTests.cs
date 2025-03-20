using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using System;

namespace TestProjectQA
{
    [TestFixture]  // test class for NUnit 
    public class BookTests
    {
        private ChromeDriver _driver;  // ChromeDriver instance
        private readonly string _appUrl = "http://localhost:5173/";  // App URL
        private WebDriverWait _wait;  // WebDriverWait for explicit waits

        [SetUp]  // Runs before each test
        public void Setup()
        {
            // Set up ChromeDriver
            new DriverManager().SetUpDriver(new ChromeConfig());
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));  // Explicit wait

            // Open the app
            _driver.Navigate().GoToUrl(_appUrl);
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);  // Set implicit wait (if needed)
        }

        [TearDown]  // Runs after each test
        public void Teardown()
        {
            if (_driver != null)
            {
                _driver.Quit();  // Close the browser
                _driver.Dispose();  // Release resources
            }
        }
        // Test 1: Check if the website opens correctly
        [Test]  // Test for website loading
        public void TestWebsiteOpens()
        {
            // Wait for page title and verify
            _wait.Until(driver => _driver.Title.Contains("Bookshelf"));
            Assert.That(_driver.Title.Contains("Bookshelf"), "Website did not open correctly");
        }
        // Test 2: Search Bar is broken (BUG-001)
        [Test]  // Test for search functionality
        public void TestSearchFunctionality()
        {
            // Wait for the search box to be visible
            var searchBox = _wait.Until(driver => _driver.FindElement(By.CssSelector("input[placeholder='Search']")));
            searchBox.SendKeys("The Golden Compass");
            searchBox.SendKeys(Keys.Enter);

            // Wait for search results and assert
            var results = _wait.Until(driver => _driver.FindElements(By.CssSelector(".book")));
            Assert.That(results.Count > 0, "Search is not working - No results found");
        }
        // Test 3: Pagination shows "Page -1" (BUG-002)
        [Test]  // Test for pagination functionality
        public void TestPagination()
        {
            // Click pagination buttons and verify
            var prevPageButton = _wait.Until(driver => _driver.FindElement(By.CssSelector("button[aria-label='Previous']")));
            prevPageButton.Click();
            prevPageButton.Click();
            prevPageButton.Click();

            var pageNumber = _wait.Until(driver => _driver.FindElement(By.CssSelector(".page-number")));
            Assert.That(!pageNumber.Text.Contains("-1"), "Pagination is broken - Showing Page -1");
        }

        [Test] // Test 4: Passing Test for user rating display
        public void TestUserRatingDisplayed()
        {
            // Find user rating and assert it's not null
            var ratingElement = _wait.Until(driver => _driver.FindElement(By.CssSelector("div.user-rating")));
            Assert.That(ratingElement, Is.Not.Null, "User rating is missing");
        }

       

    }
}
