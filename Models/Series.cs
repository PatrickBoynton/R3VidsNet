namespace R3vids.Models;

public class Series
{
    public Guid Id { get; set; }

    public string SeriesName { get; set; } = string.Empty;

    // Example: A = anime, T = tutorial
    public string GenreCode { get; set; } = string.Empty;

    public int NumberOfVideos { get; set; } = 0;

    public List<Video> Videos { get; set; } = [];
}