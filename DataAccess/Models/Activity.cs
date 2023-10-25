using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models;
public class Activity
{
    public int Id { get; set; }
    [Required]
    public DateTime ActivityDate { get; set; }
    [Required]
    [Range(1, 24)]
    public double TotalHours { get; set; }
    [Required]
    [MaxLength(200)]
    public string? Description { get; set; }
}