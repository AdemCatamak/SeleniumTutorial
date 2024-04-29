using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTutorial.Tests;

[Collection(AppFixtureDefinition.AppFixtureKey)]
public class CounterPageTests : IDisposable
{
    private readonly AppFixture _appFixture;
    private readonly WebDriver _driver;


    public CounterPageTests(AppFixture appFixture)
    {
        _appFixture = appFixture;
        _driver = new ChromeDriver();
    }

    [Fact]
    public Task WhenClickIncrementCountFirstTime__CounterShouldBeSetAsOne()
    {
        _driver.Navigate().GoToUrl(_appFixture.BaseAddress.ToString());

        var wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));

        var navigationButton = wait.Until(driver =>
        {
            var navLinks = driver.FindElements(By.ClassName("nav-link"));
            var btn = navLinks.First(element => element.Text == "Counter");
            return btn;
        });
        navigationButton.Click();

        var incrementCountButton = wait.Until(driver =>
        {
            var btn = driver.FindElement(By.TagName("button"));
            return btn;
        });

        Assert.Equal("Click me", incrementCountButton.Text);
        incrementCountButton.Click();

        var counterParagraph = wait.Until(driver =>
        {
            var p = driver.FindElement(By.TagName("p"));
            return p;
        });

        Assert.Equal("Current count: 1", counterParagraph.Text);
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _driver.Dispose();
    }
}