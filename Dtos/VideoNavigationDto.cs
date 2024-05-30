using R3vids.Models;

namespace R3vids.Dtos;

public class VideoNavigationDto
{
    public Guid Id { get; set; }
    public Video? CurrentVideo { get; set; }
    public Video? PreviousVideo { get; set; }
}