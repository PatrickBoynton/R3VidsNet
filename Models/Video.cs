namespace R3vids.Models;

public class Video
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public DateTime UploadedDate { get; set; } = DateTime.UtcNow;
    public decimal Duration { get; set; }
    public VideoStatus VideoStatus { get; set; } = null!;
}