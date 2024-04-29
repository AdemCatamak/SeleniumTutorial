using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace SeleniumTutorial.Tests;

[Collection(AppFixtureDefinition.AppFixtureKey)]
public class WeatherPageTests : IDisposable
{
    private readonly AppFixture _appFixture;
    private readonly WebDriver _driver;


    public WeatherPageTests(AppFixture appFixture)
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
            var btn = navLinks.First(element => element.Text == "Weather");
            return btn;
        });
        navigationButton.Click();

        var weatherTable = wait.Until(driver =>
        {
            var table = driver.FindElement(By.TagName("table"));
            return table;
        });
        var headers = weatherTable.FindElement(By.TagName("thead")).FindElements(By.TagName("th"));
        Assert.Equal(4, headers.Count);
        Assert.Equal("Date", headers[0].Text);
        Assert.Equal("Temp. (C)", headers[1].Text);
        Assert.Equal("Temp. (F)", headers[2].Text);    
        Assert.Equal("Summary", headers[3].Text);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _driver.Dispose();
    }
}