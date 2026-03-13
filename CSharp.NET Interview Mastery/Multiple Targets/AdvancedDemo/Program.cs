
var outPath = Path.Combine(Path.GetTempPath(), "grab.txt");
Console.WriteLine(await Grab.DownloadAsync("https://example.com", outPath));
Console.WriteLine(RuntimeInformation.FrameworkDescription);