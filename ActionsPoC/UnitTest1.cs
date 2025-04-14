using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace ActionsPoC;

public class ExampleTests : PageTest
{
    [OneTimeSetUp]
    public void ConfiguredPageTestOneTimeSetup()
    {
        
        Environment.SetEnvironmentVariable("BROWSER", "chromium");
        Environment.SetEnvironmentVariable("HEADED", "0");
        Environment.SetEnvironmentVariable("PWDEBUG", "0");
        InstallPlaywright();
    }
    
    private static void InstallPlaywright()
    {
        var exitCode = Program.Main(["install"]);
        if (exitCode != 0)
        {
            throw new Exception($"Playwright exited with code {exitCode}");
        }
    }
    
    [Test]
    public async Task CanNavigateToWebPage()
    {
        //arrange
        await Page.GotoAsync("https://playwright.dev");
        
        // Expect a title "to contain" a substring.
        await Expect(Page).ToHaveTitleAsync(new Regex("Playwright"));

        // Expect an attribute "to be str   ictly equal" to the value.
        var getStartedLink = Page.GetByRole(AriaRole.Link, new PageGetByRoleOptions { Name = "Get started" });
        await Expect(getStartedLink).ToHaveAttributeAsync("href", "/docs/intro");

        // Click the link.
        await getStartedLink.ClickAsync();
        
        // Expects the URL to be at the proper destination. .
        await Expect(Page).ToHaveURLAsync(new Regex(".*/intro"), new PageAssertionsToHaveURLOptions());
    }
}