using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Playwright;


namespace PlaywrightTests
{
    [Parallelizable(ParallelScope.Self)]
    public class LoginFailure
    {
        [Test]
        public async Task WrongPasswordShouldFailLogin()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "standard_user");
            await page.FillAsync("#password", "wrong_password");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task WrongUsernameShouldFailLogin()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "wrong_user");
            await page.FillAsync("#password", "secret_sauce");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Username and password do not match any user in this service");
        }

        [Test]
        public async Task NoUserName()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "");
            await page.FillAsync("#password", "");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Username is required");
        }

        [Test]
        public async Task NoPassword()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "standard_user");
            await page.FillAsync("#password", "");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Password is required");
        }

        [Test]
        public async Task LockedOutUserErrorMessageHeadless()
        {
            var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "locked_out_user");
            await page.FillAsync("#password", "secret_sauce");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Sorry, this user has been locked out.");
        }

        [Test]
        public async Task LockedOutUserErrorMessageHeaded()
        {
            var playwright = await Playwright.CreateAsync();

            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });
            await using var context = await browser.NewContextAsync();
            await context.Tracing.StartAsync(new TracingStartOptions
            {
                Screenshots = true,
                Snapshots = true
            });

            var page = await context.NewPageAsync();
            await page.GotoAsync("https://www.saucedemo.com/");

            await page.FillAsync("#user-name", "locked_out_user");
            await page.FillAsync("#password", "secret_sauce");
            await page.ClickAsync("#login-button");

            var error = await page.TextContentAsync("#login_button_container > div > form > div.error-message-container.error > h3");
            Assert.AreEqual(error, "Epic sadface: Sorry, this user has been locked out.");

            await context.Tracing.StopAsync(new TracingStopOptions
            {
                //TODO: You will need to change this to your path
                //run this command when you get done running the tests for the first time
                //playwright show-trace {PATHBELOW}/trace.zip
                Path = "/Users/mitch.mccoy/PlaywrightTests/tests/tests_login/trace.zip"
            });
        }
    }
}