using Microsoft.Playwright;
using Microsoft.Playwright.Xunit;

namespace LAB2_Screenshot
{
    public class TheInternetTest : PageTest
    {
        [Fact]
        public async Task LoginPage()
        {
            await Page.SetViewportSizeAsync(1280, 800);
            await Page.GotoAsync("https://the-internet.herokuapp.com/login");
            await Page.WaitForLoadStateAsync();

            if (Environment.GetEnvironmentVariable("SIMULATE_DEFECTS") == "1")
            {
                await Page.AddStyleTagAsync(new()
                {
                    Content = "#login button { transform: translateY(-45px); }"
                });
            }
            using var screenshot = new MemoryStream(await Page.ScreenshotAsync());
            await Verify(screenshot, "png");
        }
        [Fact]
        public async Task TablesPage()
        {
            await Page.SetViewportSizeAsync(1280, 800);
            await Page.GotoAsync("https://the-internet.herokuapp.com/tables");
            await Page.WaitForLoadStateAsync();
            if (Environment.GetEnvironmentVariable("SIMULATE_DEFECTS") == "1")
            {
                await Page.AddStyleTagAsync(new()
                {
                    Content = "p { background-color: red; font-size: 48px; }"
                });
            }
            using var screenshot = new MemoryStream(await Page.ScreenshotAsync());
            await Verify(screenshot, "png");
        }

        [Fact]
        public async Task DragAndDropPage()
        {
            await Page.SetViewportSizeAsync(1280, 800);
            await Page.GotoAsync("https://the-internet.herokuapp.com/drag_and_drop");
            await Page.WaitForLoadStateAsync();
            var columnB = Page.Locator("#column-b");
            await Expect(columnB).ToHaveTextAsync("B");

            if (Environment.GetEnvironmentVariable("SIMULATE_DEFECTS") == "1")
            {
                await Page.AddStyleTagAsync(new()
                {
                    Content = """
                        #column-b {
                            transform: translateX(-70px);
                        }
                        """
                });
            }

            using var screenshot = new MemoryStream(await Page.ScreenshotAsync());
            await Verify(screenshot, "png");
        }
        [Fact]
        public async Task HoversPage()
        {
            await Page.SetViewportSizeAsync(1280, 800);
            await Page.GotoAsync("https://the-internet.herokuapp.com/hovers");
            await Page.WaitForLoadStateAsync();
            var firstAvatar = Page.GetByAltText("User Avatar", new() { Exact = true }).First;
            await firstAvatar.HoverAsync();
            await Expect(Page.GetByText("name: user1", new() { Exact = true }))
                .ToBeVisibleAsync();

            if (Environment.GetEnvironmentVariable("SIMULATE_DEFECTS") == "1")
            {
                await Page.AddStyleTagAsync(new()
                {
                    Content = """
                        .figure:hover .figcaption {
                            visibility: hidden;
                        }
                        """
                });
            }

            using var screenshot = new MemoryStream(await Page.ScreenshotAsync());
            await Verify(screenshot, "png");
        }

        [Fact]
        public async Task AddRemoveElementsPage()
        {
            await Page.SetViewportSizeAsync(1280, 800);
            await Page.GotoAsync("https://the-internet.herokuapp.com/add_remove_elements/");
            await Page.WaitForLoadStateAsync();
            var addButton = Page.GetByRole(
                Microsoft.Playwright.AriaRole.Button,
                new() { Name = "Add Element", Exact = true });

            for (int i = 0; i < 3; i++)
            {
                await addButton.ClickAsync();
            }

            var deleteButtons = Page.GetByRole(
                Microsoft.Playwright.AriaRole.Button,
                new() { Name = "Delete", Exact = true });

            await Expect(deleteButtons).ToHaveCountAsync(3);

            if (Environment.GetEnvironmentVariable("SIMULATE_DEFECTS") == "1")
            {
                await Page.AddStyleTagAsync(new()
                {
                    Content = """
                        #elements button {
                            box-sizing: border-box;
                            width: 20px;
                            padding-inline: 0;
                            margin-inline: 0;
                            overflow: hidden;
                            white-space: nowrap;
                        }
                        """
                });
            }

            using var screenshot = new MemoryStream(await Page.ScreenshotAsync());
            await Verify(screenshot, "png");
        }
    }
}

