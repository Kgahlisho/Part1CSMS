using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public class Calculations 
    {
        public int HoursWorked { get; set; }

        public int HourlyRate { get; set; }


        public int TotalAmount { get; set; }

        //expended to include the datetime documents claimstatus
        public DateTime ClaimDate { get; set; }
        public string? DocumentsUploaded { get; set; }
        public string? ClaimStatus { get; set; } = "pending";
        public string Lecturer { get; set; }
    }
    
}
