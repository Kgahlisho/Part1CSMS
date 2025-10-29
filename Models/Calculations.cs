using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public class Calculations 
    {
        [Key]
       public int claimid { get; set; }
        [Required]
        public decimal HoursWorked { get; set; }


        [Required]
        public decimal HourlyRate { get; set; }


        public decimal TotalAmount { get; set; }

        //expended to include the datetime documents claimstatus
        public DateTime ClaimDate { get; set; } = DateTime.Now;
        public string? DocumentsUploaded { get; set; }
        public string? ClaimStatus { get; set; } = "Pending";

        [Required]
        public string? Lecturer { get; set; }//makeed it nullable.
    }
    
}
