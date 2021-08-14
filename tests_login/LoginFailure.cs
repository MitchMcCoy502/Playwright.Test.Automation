using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;

namespace PlaywrightTests
{
    [Parallelizable(ParallelScope.Self)]
    public class LoginFailure
    {
        [Test]
        public async Task ShouldFailLogin()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "standard_user");
            await page.FillAsync("#password", "wrong_password");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Username and password do not match any user in this service");
        }
    }
}