using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public class TestClaims 
    {
        public static List<Calculations> Claims = new()
    {
        new Calculations { Lecturer="Alice Lecturer", HoursWorked=10, HourlyRate=200, TotalAmount=2000, ClaimStatus="Pending" },
        new Calculations { Lecturer="Alice Lecturer", HoursWorked=5, HourlyRate=200, TotalAmount=1000, ClaimStatus="Approved" }
    };
    }
}
