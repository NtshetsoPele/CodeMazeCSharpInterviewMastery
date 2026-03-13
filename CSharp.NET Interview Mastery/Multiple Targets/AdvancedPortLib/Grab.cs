namespace AdvancedPortLib;

public static class Grab
{
    public static async Task<string> DownloadAsync(string url, string filePath)
    {
        if (string.IsNullOrWhiteSpace(url)) throw new ArgumentException(message: null, nameof(url));
        if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException(message: null, nameof(filePath));

        var dir = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
#if NETFRAMEWORK
        using (var wc = new System.Net.WebClient())
        {
            await wc.DownloadFileTaskAsync(new Uri(url), filePath);
        }
#else
        using var http = new System.Net.Http.HttpClient();
        var bytes = await http.GetByteArrayAsync(url);
        await File.WriteAllBytesAsync(filePath, bytes);
        if (OperatingSystem.IsWindows())
        {
            // Optional Windows-only tweak
            FileAttributes attrs = File.GetAttributes(filePath);
            File.SetAttributes(filePath, attrs | FileAttributes.Archive);
        }
#endif
        return filePath;
    }
}