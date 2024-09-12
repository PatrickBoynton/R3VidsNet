namespace R3vids.Models;

public class Playlist
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime LastUpdated { get; set; }
    public List<Video> Videos { get; set; }
}