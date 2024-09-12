using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3vids.Data;
using R3vids.Dtos;
using R3vids.Models;
using R3vids.Queries;

namespace R3vids.Controllers;

[ApiController]
[Route("/api/videos")]
public class VideosController(VideoDbContext context) : ControllerBase
{
    // http://localhost:5000/api/videos                             
    [HttpGet]
    public async Task<ActionResult<List<Video>>> GetVideos()
    {
        var videos = await context.Videos
            .Include(v => v.VideoStatus)
            .Select(v => VideoDto.MapToDto(v))
            .ToListAsync();
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("-----> Getting all videos");
        Console.WriteLine("----------------------------------------------------");
        return Ok(videos);
    }

    //http://localhost:5000/api/videos/3b3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VideoDto>> GetVideo(Guid id)
    {
        var video = await context.Videos.Include(v => v.VideoStatus).Select(v => VideoDto.MapToDto(v))
            .FirstOrDefaultAsync(v => v.Id == id);

        if (video == null) return NotFound();

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Getting video: {video.Title}");
        Console.WriteLine("----------------------------------------------------");

        return Ok(video);
    }

    // http://localhost:5000/api/videos/search?title=video
    // Not quite implemented yet
    [HttpGet("search")]
    public async Task<ActionResult<List<Video>>> GetVideos([FromQuery] SearchQuery query)
    {
        var videos = await context.Videos.ToListAsync();

        if (query.Title != null) videos = await context.Videos.Where(v => v.Title.Contains(query.Title)).ToListAsync();

        if (query.LastPlayed != null)
            videos = await context.Videos.Where(v => v.VideoStatus.LastPlayed == query.LastPlayed).ToListAsync();

        if (query.Duration != null)
            videos = await context.Videos.Where(v => v.Duration == query.Duration).ToListAsync();

        if (query.UploadedDate != null)
            videos = await context.Videos.Where(v => v.UploadedDate == query.UploadedDate).ToListAsync();

        if (query.PlayCount != null)
            videos = await context.Videos.Where(v => v.VideoStatus.PlayCount == query.PlayCount).ToListAsync();

        return Ok(videos);
    }

    // http://localhost:5000/api/videos/played
    [HttpGet("played")]
    public async Task<ActionResult<List<VideoDto>>> GetPlayedVideos()
    {
        var videos = await context.Videos.Include(v => v.VideoStatus).Where(v => v.VideoStatus.Played == true).Select(
                v => VideoDto.MapToDto(v))
            .ToListAsync();
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("-----> Getting all played videos");
        Console.WriteLine("----------------------------------------------------");
        return videos;
    }

    // http://localhost:5000/api/videos/random
    [HttpGet("random")]
    public async Task<ActionResult<VideoDto>> GetRandomVideo([FromQuery] RandomQuery? query)
    {
        var video = await context.Videos
            .Include(v => v.VideoStatus)
            .OrderBy(x => Guid.NewGuid()).AsQueryable()
            .FirstOrDefaultAsync();
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Video: {video?.Title}");
        Console.WriteLine($"-----> Video Url: {video?.Url}");
        Console.WriteLine($"-----> Play count: {video?.VideoStatus.PlayCount}");
        Console.WriteLine($"-----> Played: {video?.VideoStatus.Played}");
        Console.WriteLine($"-----> Selected at: {DateTime.Now}");
        Console.WriteLine("----------------------------------------------------");

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Request: {Request.Path}");
        Console.WriteLine($"-----> Query: {Request.QueryString}");
        Console.WriteLine("----------------------------------------------------");

        if (video == null) return NotFound();
        //This is for less than or equal to ten minutes. All times are stored in seconds
        //Example: http://localhost:5000/api/videos/random?Duration=600&Type=lte
        // This is for greater than or equal to twenty minutes. 
        //Example: http://localhost:5000/api/videos/random?Duration=1200&Type=gte
        if (query is { Duration: not null, Type: not null })
            video = query switch
            {
                { Duration: not null, Type: "lte" } => await context.Videos.Include(v => v.VideoStatus)
                    .Where(v => v.Duration <= query.Duration)
                    .OrderBy(v => Guid.NewGuid())
                    .FirstOrDefaultAsync(),
                { Duration: not null, Type: "gte" } => await context.Videos.Include(v => v.VideoStatus)
                    .Where(v => v.Duration >= query.Duration)
                    .OrderBy(v => Guid.NewGuid())
                    .FirstOrDefaultAsync(),
                _ => video
            };

        if (query is { IsPlayed: not null })
            video = await context.Videos.Include(v => v.VideoStatus)
                .Where(v => v.VideoStatus.Played == query.IsPlayed.Value)
                .OrderBy(v => Guid.NewGuid())
                .FirstOrDefaultAsync();

        if (video == null) return NotFound();

        var videoDto = VideoDto.MapToDto(video);

        return Ok(videoDto);
    }

    // http://localhost:5000/api/videos/3b3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VideoDto>> UpdateVideo(Guid id, [FromBody] VideoDto videoDto)
    {
        var video = await context.Videos.Include(v => v.VideoStatus).FirstOrDefaultAsync(v => v.Id == id);

        if (video == null) return NotFound();
        Console.WriteLine("Updating Video ");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Updating video: {video.Title}");
        Console.WriteLine("----------------------------------------------------");

        video.Title = videoDto.Title;
        video.Duration = videoDto.Duration;
        video.UploadedDate = videoDto.UploadedDate;
        video.Url = videoDto.Url;
        video.Image = videoDto.Image;
        video.VideoStatus = new VideoStatus
        {
            Played = videoDto.VideoStatus.Played,
            CurrentPlayTime = videoDto.VideoStatus.CurrentPlayTime,
            PlayCount = videoDto.VideoStatus.PlayCount,
            IsWatchLater = videoDto.VideoStatus.IsWatchLater,
            LastPlayed = videoDto.VideoStatus.LastPlayed
        };

        await context.SaveChangesAsync();

        return Ok(videoDto);
    }

    // http://localhost:5000/api/videos/reset
    [HttpDelete("reset")]
    public async Task<ActionResult> ResetVideoStatus()
    {
        var videos = await context.Videos.Include(v => v.VideoStatus).ToListAsync();
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("-----> Resetting all video status");
        Console.WriteLine($"-----> Deleted at: {DateTime.Now}");
        Console.WriteLine("----------------------------------------------------");
        foreach (var video in videos)
        {
            video.VideoStatus.Played = false;
            video.VideoStatus.CurrentPlayTime = 0;
            video.VideoStatus.PlayCount = 0;
            video.VideoStatus.IsWatchLater = false;
            video.VideoStatus.LastPlayed = null;
        }

        await context.SaveChangesAsync();

        return NoContent();
    }

    // http://localhost:5000/api/videos/update/1f3b3b3b-3b3b-3b3b-3b3b-3b3b3b3b3b3b
    [HttpPatch("update/{id:guid}")]
    public async Task<ActionResult> UpdateVideoStatus(Guid id)
    {
        var video = await context.Videos.Include(v => v.VideoStatus).FirstOrDefaultAsync(v => v.Id == id);

        if (video == null) return NotFound();

        video.VideoStatus.LastPlayed = DateTime.UtcNow;
        video.VideoStatus.Played = true;
        video.VideoStatus.PlayCount++;
        Console.WriteLine("Updating Video Status");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Updating video status: {video.Title}");
        Console.WriteLine("----------------------------------------------------");

        // context.VideoNavigations.Add(new VideoNavigation
        // {
        //     CurrentVideo = video.Id
        // });

        context.Videos.Update(video);
        await context.SaveChangesAsync();

        return NoContent();
    }
}