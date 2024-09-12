using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using R3vids.Data;
using R3vids.Models;

namespace R3vids.Controllers;

[ApiController]
[Route("/api/playlist")]
public class PlaylistController(VideoDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Playlist>>> GetPlaylists()
    {
        var playlists = await context.Playlists.ToListAsync();

        if (playlists == null) return NotFound();

        return Ok(playlists);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Playlist>> GetPlaylist(Guid id)
    {
        var playlist = await context.Playlists.FirstOrDefaultAsync(p => p.Id == id);

        if (playlist == null) return NotFound();

        return Ok(playlist);
    }

    [HttpPost]
    public async Task<ActionResult<Playlist>> CreatePlaylist([FromBody] Playlist playlist)
    {
        context.Playlists.Add(playlist);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPlaylist), new { id = playlist.Id }, playlist);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Playlist>> UpdatePlaylist(Guid id, Playlist updatedPlaylist)
    {
        var playlist = await context.Playlists.FirstOrDefaultAsync(p => p.Id == id);

        if (playlist == null) return NotFound();

        playlist.Name = updatedPlaylist.Name;
        playlist.Description = updatedPlaylist.Description;
        playlist.Videos = updatedPlaylist.Videos;
        playlist.LastUpdated = DateTime.UtcNow;

        context.Playlists.Update(playlist);

        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<Playlist>> DeletePlaylist(Guid id)
    {
        var playlist = await context.Playlists.FirstOrDefaultAsync(p => p.Id == id);

        if (playlist == null) return NotFound();

        context.Playlists.Remove(playlist);

        await context.SaveChangesAsync();

        return NoContent();
    }
}