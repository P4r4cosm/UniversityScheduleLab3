using System.ComponentModel.DataAnnotations;

namespace University_Schedule_Lab1;

public class Lecture
{
    public int Id {get; set;}
    
    [Required]public string Name {get; set;}
    
    public bool Requirments {get; set;}
    
    public int CourceId {get; set;}
    public Course Course {get; set;}
}