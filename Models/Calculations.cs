using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public class Calculations 
    {
        [Key]
       public int claimid { get; set; }
        [Required]
        public int HoursWorked { get; set; }


        [Required]
        public int HourlyRate { get; set; }


        public int TotalAmount { get; set; }

        //expended to include the datetime documents claimstatus
        public DateTime ClaimDate { get; set; } = DateTime.Now;
        public string? DocumentsUploaded { get; set; }
        public string? ClaimStatus { get; set; } = "Pending";

        [Required]
        public string? Lecturer { get; set; }//makeed it nullable.
    }
    
}
