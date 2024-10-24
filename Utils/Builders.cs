using FFMpegCore;
using Microsoft.EntityFrameworkCore;
using R3vids.Data;
using R3vids.Models;

namespace R3vids.Utils;

public static class Builders
{
    public static async Task InitDatabase(WebApplication app)
    {
        try
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<VideoDbContext>();
            var video = context.Videos.FirstOrDefault();
            var videos = await BuildVideo(context, app);

            Console.WriteLine("---> Initializing database.");
            if (!context.Videos.Any() && !context.VideoStatus.Any())
            {
                var videoStatuses = BuildVideoStatus(context);

                context.Videos.AddRange(videos);

                context.VideoStatus.AddRange(videoStatuses);

                await context.SaveChangesAsync();

                Console.WriteLine("----> Videos saved to database.");
            }
            else
            {
                foreach (var vid in videos)
                {
                    var existingVideo = context.Videos.FirstOrDefault(v => v.Title == vid.Title);
                    if (existingVideo != null)
                    {
                        existingVideo.Url = vid.Url;
                        context.Videos.Update(existingVideo);
                    }
                    else
                    {
                        context.Videos.Add(vid);
                    }
                }

                await context.SaveChangesAsync();
                Console.WriteLine("----> Video urls updated..");
            }

            if (!context.VideoNavigations.Any())
            {
                var randomVideo = await context.Videos
                    .Include(v => v.VideoStatus)
                    .OrderBy(x => Guid.NewGuid()).AsQueryable()
                    .FirstOrDefaultAsync();

                var videoNavigation = new VideoNavigation
                {
                    Id = Guid.NewGuid(),
                    CurrentVideo = randomVideo?.Id,
                    PreviousVideo = null
                };
                context.VideoNavigations.Add(videoNavigation);
                // await context.SaveChangesAsync();
                Console.WriteLine("----> Video navigation saved to database.");
            }
            else
            {
                Console.WriteLine("----> Video navigation already exists in database.");
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }


    private static string BuildUrl()
    {
        try
        {
            var builder = new ConfigurationBuilder().SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.Development.json");
            var config = builder.Build();
            var ipAddress = config["IpAddress"];

            Console.WriteLine($"----> Building url: http://{ipAddress}:5070/");

            return $@"http://{ipAddress}:5070/";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static decimal BuildDuration(string file)
    {
        var mediaInfo = FFProbe.Analyse(file);
        var duration = mediaInfo.Duration.TotalSeconds;

        return Convert.ToDecimal(duration);
    }

    private static async Task<List<Video>> BuildVideo(VideoDbContext context, WebApplication app)
    {
        try
        {
            var files = FileHelpers.GetFiles();
            var baseUrl = BuildUrl();

            var videos = files.Where(File.Exists)
                .Where(file => file.EndsWith(".mp4"))
                .Select(file =>
                {
                    var videoStatus = new VideoStatus
                    {
                        LastPlayed = null,
                        CurrentPlayTime = 0,
                        PlayCount = 0,
                        Played = false,
                        IsWatchLater = false,
                        Video = null!,
                        VideoId = Guid.NewGuid()
                    };

                    var video = new Video
                    {
                        // Should build a title such as: Video
                        Title = Path.GetFileNameWithoutExtension(file),
                        // Should build a path such as: http://192.168.1.16:5070/Video.mp4
                        Url = baseUrl + Path.GetFileName(file),
                        Image = "https://via.placeholder.com/150",
                        // Shows the duration of the video in seconds
                        Duration = BuildDuration(file),
                        UploadedDate = DateTime.UtcNow,
                        VideoStatus = videoStatus
                    };

                    return video;
                }).ToList();

            var newVideos = files
                // Checks if the file exists
                .Select(file => new { FileName = file, Url = baseUrl + Path.GetFileName(file) })
                // Checks if the video is already in the database
                .Where(fileInfo => !context.Videos.Any(v => v.Url == fileInfo.Url))
                // Maps the file to a video object
                .Select(fileInfo => new Video
                {
                    Title = Path.GetFileNameWithoutExtension(fileInfo.FileName),
                    Url = fileInfo.Url,
                    Image = "https://via.placeholder.com/150",
                    Duration = BuildDuration(fileInfo.FileName),
                    UploadedDate = DateTime.UtcNow,
                    VideoStatus = new VideoStatus
                    {
                        LastPlayed = null,
                        CurrentPlayTime = 0,
                        PlayCount = 0,
                        Played = false,
                        IsWatchLater = false,
                        Video = null!,
                        VideoId = Guid.NewGuid()
                    }
                }).ToList();

            if (newVideos.Any()) context.Videos.AddRange(newVideos);
            // await context.SaveChangesAsync();
            return videos;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static List<VideoStatus> BuildVideoStatus(VideoDbContext context)
    {
        var videoStatuses = context.VideoStatus.Include(vs => vs.Video).ToList();

        context.AddRange(videoStatuses);
        return videoStatuses;
    }

    private static List<Video> UpdateUrl(List<Video> videos)
    {
        foreach (var video in videos)
            video.Url = BuildUrl() + Path.GetFileName(video.Url);

        return videos;
    }
}