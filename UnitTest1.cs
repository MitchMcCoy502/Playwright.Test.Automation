using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;

namespace PlaywrightTests
{
    [Parallelizable(ParallelScope.Self)]
    public class Tests
    {
        [Test]
        public async Task ShouldBeWhyPlaywright()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev/dotnet/docs/why-playwright");
            var content = await page.TitleAsync();
            Assert.AreEqual(content, "Why Playwright? | Playwright .NET");
        }

        [Test]
        public async Task ShouldLogIn(){
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "standard_user");
            await page.FillAsync("#password","secret_sauce");
            await page.ClickAsync("#login-button");

            var title = await page.TextContentAsync(".title");
            Assert.AreEqual(title, "Products"); 
        }
    }
}