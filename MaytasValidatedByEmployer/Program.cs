using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

static string GetValidator(string method)
{
    return method.ToLowerInvariant() switch
    {
        "classroom training" => "A",
        "self-directed study" => "E",
        "assignments" => "A",
        "research" => "E",
        "online training" => "A",
        "job related projects" => "E",
        "mentoring" => "E",
        _ => "E",
    };
}

using var driver = new ChromeDriver();

try
{
    await driver.Navigate().GoToUrlAsync("https://apprenticeships.shu.ac.uk/etrack/Login");
    driver.Manage().Window.Maximize();

    Console.WriteLine("Login to Maytas and visit the OTJ page, then press [ENTER] to continue...");
    Console.ReadLine();

    var rows = driver.FindElements(By.CssSelector("tr.customGridRow"));

    for (var i = 0; i < rows.Count; i++)
    {
        var row = rows[i];

        var method = row.FindElement(By.CssSelector("select[fieldname='OTJT_METHOD']"));
        var selectedMethod = new SelectElement(method);
        
        var editButton = row.FindElement(By.CssSelector("a[onclick*='editRow']"));

        if (i >= rows.Count - 5)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
        }
        else
        {
            ((IJavaScriptExecutor)driver).ExecuteScript(
                "arguments[0].scrollIntoView(true); window.scrollBy(0, -window.innerHeight / 2);", editButton);
        }

        await Task.Delay(50);

        editButton.Click();

        await Task.Delay(100);

        var validatedBySelector = driver.FindElement(By.Id("section1body1_col1_control5_ddl"));

        var selectElement = new SelectElement(validatedBySelector);

        selectElement.SelectByValue(GetValidator(selectedMethod.SelectedOption.Text));

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