namespace R3vids.Dtos;

public class VideoStatusDto
{
    public Guid Id { get; set; }
    public bool Played { get; set; }
    public decimal CurrentPlayTime { get; set; }
    public int PlayCount { get; set; }
    public int SelectionCount { get; set; }
    public bool IsWatchLater { get; set; }
    public DateTime? LastPlayed { get; set; }
}