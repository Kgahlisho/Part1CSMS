using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public class Calculations 
    {
        [Key]
       public int claimid { get; set; }
        [Required]
            [Column(TypeName = "decimal(18,2)")]
        public decimal HoursWorked { get; set; }


        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal HourlyRate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        //expended to include the datetime documents claimstatus
        public DateTime ClaimDate { get; set; } = DateTime.Now;
        public string? DocumentsUploaded { get; set; }
        public string? ClaimStatus { get; set; } = "Pending";

        [Required]
        public string? Lecturer { get; set; }//makeed it nullable.

        public string? VerifiedBy { get; set; } 
        public string? DeniedBy { get; set; }
                //public string? ApprovedByManager { get; set; }
    }
    
}
