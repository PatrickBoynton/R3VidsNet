using R3vids.Models;

namespace R3vids.Dtos;

public class VideoDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Image { get; set; }
    public DateTime UploadedDate { get; set; }
    public decimal Duration { get; set; }
    public VideoStatusDto VideoStatus { get; set; }
    public VideoNavigationDto VideoNavigation { get; set; }


    public static VideoDto MapToDto(Video video)
    {
        return new VideoDto
        {
            Id = video.Id,
            Title = video.Title,
            Url = video.Url,
            Image = video.Image,
            UploadedDate = video.UploadedDate,
            Duration = video.Duration,
            VideoStatus = new VideoStatusDto
            {
                Id = video.VideoStatus.Id,
                Played = video.VideoStatus.Played,
                CurrentPlayTime = video.VideoStatus.CurrentPlayTime,
                PlayCount = video.VideoStatus.PlayCount,
                IsWatchLater = video.VideoStatus.IsWatchLater,
                LastPlayed = video.VideoStatus.LastPlayed
            }
        };
    }
}