using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace ParkyAPI.Models
{
    public class NationalPark
    {
        [Key] 
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string State { get; set; }
        
        public byte[] Image { get; set; }
        
        public DateTime Created { get; set; }
        
        [AllowNull]
        public DateTime Established { get; set; }
        
    }
}