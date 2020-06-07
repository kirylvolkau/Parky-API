using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.DTO
{
    public class TrailUpdateDto
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }
        
        public int NationalParkId { get; set; }
    }
}