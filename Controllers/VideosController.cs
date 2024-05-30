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
    [HttpGet]
    public async Task<ActionResult<List<Video>>> GetVideos()
    {
        var videos = await context.Videos
            .Include(v => v.VideoStatus)
            .Select(v => VideoDto.MapToDto(v))
            .ToListAsync();

        return Ok(videos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VideoDto>> GetVideo(Guid id)
    {
        var video = await context.Videos.Include(v => v.VideoStatus).Select(v => VideoDto.MapToDto(v))
            .FirstOrDefaultAsync(v => v.Id == id);

        if (video == null) return NotFound();

        return Ok(video);
    }

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

    [HttpGet("played")]
    public async Task<ActionResult<List<VideoDto>>> GetPlayedVideos()
    {
        var videos = await context.Videos.Include(v => v.VideoStatus).Where(v => v.VideoStatus.Played == true).Select(
                v => VideoDto.MapToDto(v))
            .ToListAsync();
        return videos;
    }


    [HttpGet("random")]
    public async Task<ActionResult<VideoDto>> GetRandomVideo([FromQuery] RandomQuery? query)
    {
        var video = await context.Videos
            .Include(v => v.VideoStatus)
            .OrderBy(x => Guid.NewGuid())
            .FirstOrDefaultAsync();

        if (video == null) return NotFound();

        await UpdateVideoStatus(video, context);

        video = query switch
        {
            { Duration: not null, Type: "lte" } => await context.Videos.Where(v => v.Duration <= query.Duration)
                .OrderBy(v => Guid.NewGuid())
                .FirstOrDefaultAsync(),
            { Duration: not null, Type: "gte" } => await context.Videos.Where(v => v.Duration >= query.Duration)
                .OrderBy(v => Guid.NewGuid())
                .FirstOrDefaultAsync(),
            _ => video
        };

        if (query is { IsPlayed: not null })
            video = await context.Videos.Where(v => v.VideoStatus.Played == query.IsPlayed.Value)
                .OrderBy(v => Guid.NewGuid())
                .FirstOrDefaultAsync();

        if (video == null) return NotFound();

        var videoDto = VideoDto.MapToDto(video);

        return Ok(videoDto);
    }

    [HttpGet("random/played")]
    public async Task<ActionResult<VideoDto>> GetRandomPlayedVideos()
    {
        var videos = await context.Videos.Include(v => v.VideoStatus).Where(v => v.VideoStatus.Played).ToListAsync();
        var randomPlayedVideo = videos.MinBy(x => Guid.NewGuid());

        if (randomPlayedVideo == null)
            return NotFound();

        await UpdateVideoStatus(randomPlayedVideo, context);

        var videoDto = VideoDto.MapToDto(randomPlayedVideo);

        return Ok(videoDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VideoDto>> UpdateVideo(Guid id, [FromBody] VideoDto videoDto)
    {
        var video = await context.Videos.Include(v => v.VideoStatus).FirstOrDefaultAsync(v => v.Id == id);

        if (video == null) return NotFound();

        video.Title = videoDto.Title;
        video.Duration = videoDto.Duration;
        video.UploadedDate = videoDto.UploadedDate;
        video.VideoStatus.Played = videoDto.VideoStatus.Played;
        video.VideoStatus.CurrentPlayTime = videoDto.VideoStatus.CurrentPlayTime;
        video.VideoStatus.PlayCount = videoDto.VideoStatus.PlayCount;
        video.VideoStatus.IsWatchLater = videoDto.VideoStatus.IsWatchLater;
        video.VideoStatus.LastPlayed = videoDto.VideoStatus.LastPlayed;

        await context.SaveChangesAsync();

        return Ok(videoDto);
    }

    [HttpDelete("reset")]
    public async Task<ActionResult> ResetVideoStatus()
    {
        var videos = await context.Videos.Include(v => v.VideoStatus).ToListAsync();

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

    private static async Task UpdateVideoStatus(Video video, VideoDbContext context)
    {
        video.VideoStatus.LastPlayed = DateTime.UtcNow;
        video.VideoStatus.Played = true;
        video.VideoStatus.PlayCount++;

        context.VideoNavigations.Add(new VideoNavigation
        {
            CurrentVideo = video.Id
        });

        context.Videos.Update(video);
        await context.SaveChangesAsync();
    }
}