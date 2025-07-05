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

    // http://192.168.1.x:5070/api/videos/search?title=videoTitle
    // Not quite implemented yet.
    // Currently only searching by title is set up.
    [HttpGet("search")]
    public async Task<ActionResult<List<VideoDto>>> GetVideos([FromQuery] SearchQuery query)
    {
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Searching for video: {query.Title}");
        Console.WriteLine("----------------------------------------------------");
        var videos = await context.Videos.Include(v => v.VideoStatus).ToListAsync();

        var mappedVideos = videos.Select(VideoDto.MapToDto).ToList();

        if (query.Title != null)
            mappedVideos = mappedVideos.Where(v => v.Title.Contains(query.Title)).ToList();

        if (query.LastPlayed != null)
            videos = await context.Videos.Where(v => v.VideoStatus.LastPlayed == query.LastPlayed).ToListAsync();

        if (query.Duration != null)
            videos = await context.Videos.Where(v => v.Duration == query.Duration).ToListAsync();

        if (query.UploadedDate != null)
            videos = await context.Videos.Where(v => v.UploadedDate == query.UploadedDate).ToListAsync();

        if (query.PlayCount != null)
            videos = await context.Videos.Where(v => v.VideoStatus.PlayCount == query.PlayCount).ToListAsync();

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Found {mappedVideos.Count} videos");
        Console.WriteLine("----------------------------------------------------");

        return Ok(mappedVideos);
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
        var videoQuery = context.Videos.Include(v => v.VideoStatus)
            .AsQueryable();

        //This is for less than or equal to ten minutes. All times are stored in seconds
        //Example: http://localhost:5070/api/videos/random?Duration=600&Type=lte
        // This is for greater than or equal to twenty minutes. 
        //Example: http://localhost:5070/api/videos/random?Duration=1200&Type=gte
        if (query is { Duration: not null, Type: not null })
            videoQuery = query.Type switch
            {
                "lte" => videoQuery.Where(v => v.Duration <= query.Duration),
                "gte" => videoQuery.Where(v => v.Duration >= query.Duration),
                _ => videoQuery
            };

        // This is for random videos that aren't played.
        // Example:  http://localhost:5070/api/videos/random?IsPlayed=false
        if (query is { IsPlayed: not null })
            videoQuery = videoQuery.Where(v => v.VideoStatus.Played == query.IsPlayed);


        var video = await videoQuery
            .OrderBy(x => Guid.NewGuid())
            .FirstOrDefaultAsync();

        if (video == null) return NotFound();

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Video: {video?.Title}");
        Console.WriteLine($"-----> Video Url: {video?.Url}");
        Console.WriteLine($"-----> Video Duration: {video?.Duration}");
        Console.WriteLine($"-----> Play count: {video?.VideoStatus.PlayCount}");
        Console.WriteLine($"-----> Played: {video?.VideoStatus.Played}");
        Console.WriteLine($"-----> Selected at: {DateTime.Now:MM/dd/yyy hh:mm:ss tt}");
        Console.WriteLine("----------------------------------------------------");

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Query Duration: {query?.Duration}");
        Console.WriteLine($"-----> Query Type: {query?.Type}");
        Console.WriteLine($"-----> Query Played: {query?.IsPlayed}");
        Console.WriteLine("----------------------------------------------------");

        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine($"-----> Request: {Request.Path}");
        Console.WriteLine($"-----> Query: {Request.QueryString}");
        Console.WriteLine("----------------------------------------------------");


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
        Console.WriteLine($"-----> Time updated at: {DateTime.Now:MM/dd/yyy hh:mm:ss tt}");
        Console.WriteLine("----------------------------------------------------");
        Console.WriteLine("-----> UPDATED FROM FULL UPDATE CONTROLLER");
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
            SelectionCount = videoDto.VideoStatus.SelectionCount,
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
        Console.WriteLine($"-----> Deleted at: {DateTime.Now:MM/dd/yyy hh:mm:ss tt}");
        Console.WriteLine("----------------------------------------------------");
        foreach (var video in videos)
        {
            video.VideoStatus.Played = false;
            video.VideoStatus.CurrentPlayTime = 0;
            video.VideoStatus.PlayCount = 0;
            video.VideoStatus.IsWatchLater = false;
            video.VideoStatus.LastPlayed = null;
            video.VideoStatus.SelectionCount = 0;
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

        context.Videos.Update(video);
        await context.SaveChangesAsync();

        return NoContent();
    }
}