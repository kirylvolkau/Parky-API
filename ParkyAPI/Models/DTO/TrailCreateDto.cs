using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.DTO
{
    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }
        
        [Required]
        public int NationalParkId { get; set; }
    }
}