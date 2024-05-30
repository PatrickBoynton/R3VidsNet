namespace R3vids.Queries

;

public class SearchQuery
{
	public string? Title { get; set; } = null;
	public DateTime? LastPlayed { get; set; } = null;
	public decimal? Duration { get; set; } = null;
	public DateTime? UploadedDate { get; set; } = null;
	public int? PlayCount { get; set; } = null;
}