using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3vids.Data;
using R3vids.Dtos;
using R3vids.Models;

namespace R3vids.Controllers;

[ApiController]
[Route("/api/navigation/")]
public class VideoNavigationController(VideoDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VideoNavigationDto>>> GetVideoNavigations()
    {
        var navigation = await context.VideoNavigations.FirstOrDefaultAsync();

        // if (navigation == null) return NotFound();
        var navigationMap = VideoNavigationDto.MapToDto(navigation, context);

        return Ok(navigationMap);
    }

    [HttpPost]
    public async Task<ActionResult<VideoNavigation>> CreateVideoNavigation([FromBody] VideoNavigation navigation)
    {
        var existingNavigation = await context.VideoNavigations.FirstOrDefaultAsync();

        if (existingNavigation == null)
        {
            existingNavigation = new VideoNavigation
            {
                Id = Guid.NewGuid(),
                CurrentVideo = null,
                PreviousVideo = null
            };
            context.VideoNavigations.Add(existingNavigation);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpPut]
    public async Task<ActionResult> UpdateNavigation([FromBody] VideoNavigation navigation)
    {
        var existingNavigation = await context.VideoNavigations.FirstOrDefaultAsync();

        if (existingNavigation != null)
        {
            existingNavigation.CurrentVideo = navigation.CurrentVideo;
            existingNavigation.PreviousVideo = navigation.PreviousVideo;
        }
        else
        {
            return NotFound();
        }

        await context.SaveChangesAsync();
        return NoContent();
    }

// This method does not delete the navigation, but removes the current video from the navigation table
    [HttpDelete]
    public async Task<ActionResult> RemoveNavigation([FromBody] VideoNavigation existingNavigation)
    {
        var navigation = await context.VideoNavigations.FirstOrDefaultAsync();

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