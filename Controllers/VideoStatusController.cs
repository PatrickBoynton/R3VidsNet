using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3vids.Data;
using R3vids.Models;

namespace R3vids.Controllers;

[ApiController]
[Route("/api/status")]
public class VideoStatusController(VideoDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<VideoStatus>> GetVideoStatus()
    {
        var videoStatuses = await context.VideoStatus.ToListAsync();

        return Ok(videoStatuses);
    }

    [HttpGet("played")]
    public async Task<ActionResult<VideoStatus>> GetPlayedStatus()
    {
        var videoStatuses = await context.VideoStatus.Where(v => v.Played == true).ToListAsync();

        return Ok(videoStatuses);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Video>> GetSingleStatus(Guid id)
    {
        var video = await context.VideoStatus.FindAsync(id);

        return Ok(video);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VideoStatus>> UpdateVideoStatus(Guid id, [FromBody] VideoStatus videoStatusUpdate)
    {
        var videoStatus = await context.VideoStatus.FindAsync(id);

        if (videoStatus != null)
        {
            videoStatus.Played = !videoStatus.Played;
            videoStatus.CurrentPlayTime = videoStatusUpdate.CurrentPlayTime;
            videoStatus.PlayCount = videoStatus.PlayCount += 1;
            videoStatus.IsWatchLater = !videoStatus.IsWatchLater;
            videoStatus.LastPlayed = DateTime.UtcNow;
        }


        await context.SaveChangesAsync();

        return Ok(videoStatus);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> ResetStatus(Guid id)
    {
        // Note this does not actually delete the VideoStatus object, just resets the properties
        var videoStatus = await context.VideoStatus.FindAsync(id);

        if (videoStatus != null)
        {
            videoStatus.Played = false;
            videoStatus.CurrentPlayTime = 0;
            videoStatus.PlayCount = 0;
            videoStatus.IsWatchLater = false;
            videoStatus.LastPlayed = null;
            videoStatus.SelectionCount = 0;
            Console.WriteLine($"Selection Count after reset: {videoStatus.SelectionCount}");
        } 

        await context.SaveChangesAsync();
        Console.WriteLine("---------------------------");
        Console.WriteLine("Reset Video Status");
        Console.WriteLine("---------------------------");
        return NoContent();
    }
}