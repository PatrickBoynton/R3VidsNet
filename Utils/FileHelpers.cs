namespace R3vids.Utils;

public static class FileHelpers
{
    public static List<string> GetFiles()
    {
        const string directory = "./wwwroot/";
        var files = Directory.GetFiles(directory);

        return files.ToList();
    }

    public static bool CheckIp(string url)
    {
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json");
        var config = builder.Build();
        var ipAddress = config["IpAddress"];
        var ipAddressToCheck = url.Split('/')[2].Split(':')[0];

        return ipAddress == ipAddressToCheck;
    }
}