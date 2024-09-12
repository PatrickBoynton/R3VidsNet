using Microsoft.EntityFrameworkCore;
using R3vids.Data;
using R3vids.Models;

namespace R3vids.Dtos;

public class VideoNavigationDto
{
    public Guid Id { get; set; }
    public VideoDto? CurrentVideo { get; set; }
    public VideoDto? PreviousVideo { get; set; }

    public static VideoNavigationDto MapToDto(VideoNavigation videoNavigation, VideoDbContext context)
    {
        //     if (!videoNavigation.CurrentVideo.HasValue) throw new Exception("Current video is null.");
        //
        var id = videoNavigation?.CurrentVideo.Value;

        if (id == null) throw new Exception("Current video id is null.");
        ;
        var video = context.Videos.Include(v => v.VideoStatus).FirstOrDefault(v => v.Id == id);


        if (video == null) throw new Exception("Video not found.");

        return new VideoNavigationDto
        {
            Id = videoNavigation.Id,
            CurrentVideo = video != null ? VideoDto.MapToDto(video) : null,
            PreviousVideo = null
        };
    }
}