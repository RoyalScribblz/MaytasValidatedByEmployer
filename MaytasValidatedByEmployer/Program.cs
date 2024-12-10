using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

using var driver = new ChromeDriver();

try
{
    await driver.Navigate().GoToUrlAsync("https://apprenticeships.shu.ac.uk/etrack/Login");
    driver.Manage().Window.Maximize();

    Console.WriteLine("Login to Maytas and visit the OTJ page, then press [ENTER] to continue...");
    Console.ReadLine();

    var editButtons = driver.FindElements(By.XPath("//a[contains(@onclick, 'editRow')]"));

    foreach (var editButton in editButtons)
    {
        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView(true); window.scrollBy(0, -window.innerHeight / 2);", editButton);

        await Task.Delay(50);

        editButton.Click();

        await Task.Delay(100);

        var validatedBySelector = driver.FindElement(By.Id("section1body1_col1_control5_ddl"));

        var selectElement = new SelectElement(validatedBySelector);

        selectElement.SelectByValue("E");

        await Task.Delay(50);

        var saveButton = driver.FindElement(By.ClassName("saveButton"));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", saveButton);
        saveButton.Click();

        await Task.Delay(50);
    }
}
catch (Exception exception)
{
    Console.WriteLine(exception.Message);
}

await Task.Delay(1_000_000);

driver.Quit();