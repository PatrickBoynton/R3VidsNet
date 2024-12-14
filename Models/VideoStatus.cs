namespace R3vids.Models;

public class VideoStatus
{
    public Guid Id { get; set; }
    public bool Played { get; set; } = false;
    public decimal CurrentPlayTime { get; set; } = 0;
    public int PlayCount { get; set; } = 0;
    public int SelectionCount { get; set; } = 0;
    public bool IsWatchLater { get; set; } = false;
    public DateTime? LastPlayed { get; set; }
    public Video Video { get; set; } = null!;
    public Guid VideoId { get; set; }
}