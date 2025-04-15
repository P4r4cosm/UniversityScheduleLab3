using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace University_Schedule_Lab1.Contracts;

public record FindBadStudentsRequest
{
    [Required]
    [DefaultValue("Особенности")]
    public string? SearchText { get; set; }

    [Required]
    [DefaultValue("2021-09-01T12:00:00")]
    public DateTime StartDate { get; set; }
    
    [Required]
    [DefaultValue("2022-09-01T12:00:00")]
    public DateTime EndDate { get; set; }
}