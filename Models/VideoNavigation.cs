using System.ComponentModel.DataAnnotations.Schema;

namespace R3vids.Models;

[Table("VideoNavigation")]
public class VideoNavigation
{
    public Guid Id { get; set; }
    public Guid? CurrentVideo { get; set; }
    public Guid? PreviousVideo { get; set; }
}