using Microsoft.AspNetCore.Mvc;
using R3vids.Data;
using R3vids.Models;

namespace R3vids.Controllers;

[ApiController]
[Route("/api/navigation/{id:guid}")]
public class VideoNavigationController(VideoDbContext context) : ControllerBase
{
    [HttpGet("")]
    public async Task<ActionResult<VideoNavigation>> GetVideoNavigation(Guid id)
    {
        var navigation = await context.VideoNavigations.FindAsync(id);

        return Ok(navigation);
    }

    [HttpPut("")]
    public async Task<ActionResult> UpdateNavigation(Guid id, [FromBody] VideoNavigation navigation)
    {
        var existingNavigation = await context.VideoNavigations.FindAsync(id);

        if (existingNavigation != null)
        {
            existingNavigation.CurrentVideo = navigation.CurrentVideo;
            existingNavigation.PreviousVideo = navigation.PreviousVideo;
        }

        await context.SaveChangesAsync();
        return NoContent();
    }

    // This method does not delete the navigation, but removes the current video from the navigation table
    [HttpDelete("")]
    public async Task<ActionResult> RemoveNavigation(Guid id, [FromBody] VideoNavigation existingNavigation)
    {
        var navigation = await context.VideoNavigations.FindAsync(id);

        if (navigation == null) return NotFound();

        switch (existingNavigation)
        {
            case { CurrentVideo: not null }:
                navigation.CurrentVideo = null;
                break;
            case { PreviousVideo: not null }:
                navigation.PreviousVideo = null;
                break;
        }

        return NoContent();
    }
}