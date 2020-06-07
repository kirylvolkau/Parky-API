using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.DTO
{
    public class TrailDto
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }
        
        [Required]
        public int NationalParkId { get; set; }
        
        public NationalParkDto NationalPark { get; set; }
    }
}