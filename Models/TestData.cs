using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{
    public static class TestData
    {
        // Hardcoded test users
        public static List<(string Email, string Password, string Role, string Name)> Users =
             new()
             {
                ("lecturer@test.com", "123", "Lecturer", "Alice Lecturer"),
                ("coord@test.com", "123", "Program Coordinator", "Bob Coordinator"),
                ("manager@test.com", "123", "Program Manager", "Mary Manager")
             };
    }
}
