using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public class TestClaims 
    {
        public static List<Calculations> Claims = new()
    {
           new Calculations { claimid=1, Lecturer ="Alice Lecturer", HoursWorked=10, HourlyRate=200, TotalAmount=2000, ClaimStatus="Pending" },
           new Calculations { claimid=2, Lecturer="Alice Lecturer", HoursWorked=5, HourlyRate=200, TotalAmount=1000, ClaimStatus="Approved" },
           new Calculations { claimid=3 , Lecturer="Mathabatha Gerald" , HoursWorked=9 , HourlyRate=300 , TotalAmount=2700 ,ClaimStatus="Pending"},
           new Calculations { claimid=4 , Lecturer="Donalid Polisha" , HoursWorked=9 , HourlyRate=35 , TotalAmount=315 ,ClaimStatus="Verified"},
           new Calculations { claimid=5 , Lecturer="Polinda pendonia" , HoursWorked=5 , HourlyRate=600 , TotalAmount=3000 ,ClaimStatus="Approved"},
           new Calculations { claimid=6 , Lecturer="Mathabatha Gerald" , HoursWorked=9 , HourlyRate=300 , TotalAmount=2700 ,ClaimStatus="Pending"},
           new Calculations { claimid=7 , Lecturer="Masemola Nkunzi" , HoursWorked=10 , HourlyRate=300 , TotalAmount=3000 ,ClaimStatus="Rejected"},
           new Calculations { claimid=8 , Lecturer="Lamolela Mbatha" , HoursWorked=9 , HourlyRate=300 , TotalAmount=2700 ,ClaimStatus="Rejected"},
           new Calculations { claimid=9 , Lecturer="Thabo doe" , HoursWorked=2 , HourlyRate=300 , TotalAmount=600 ,ClaimStatus="Pending"}
                };
    }
}
