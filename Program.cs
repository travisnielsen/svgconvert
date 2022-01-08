using Microsoft.Playwright;

class Program
{   
    public static async Task Main()
    {
        // Ensure browsers are installed: https://playwright.dev/dotnet/docs/browsers#prerequisites-for-net
        Microsoft.Playwright.Program.Main(new string[] {"install" });
        
        var inputDir = @"media\input";
        var outputDir = @"media\output";
        var width = 70;
        var height = 70;

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
        var page = await browser.NewPageAsync();
        await page.SetViewportSizeAsync(width, height);

        var files = new DirectoryInfo(inputDir).GetFiles("*.svg").Select(x => x.FullName).ToArray();

        foreach(var filePath in files)
        {
            var fileName = filePath.Split(@"\").Last();
            await page.GotoAsync(filePath);
            var item = await page.QuerySelectorAsync("svg");
            var pngFileName = fileName.Replace("svg", "png");
            string workingDirectory = Environment.CurrentDirectory;
            string outputPath = Path.Combine(new string[] {outputDir, pngFileName });
            await item!.ScreenshotAsync(new() { Path = outputPath, OmitBackground = true });
        }
        
    }
    
}