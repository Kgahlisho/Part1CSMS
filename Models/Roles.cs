using Microsoft.AspNetCore.Mvc;

namespace Part1ex.Models
{

    //Use this list just for temporary in-memory before the database
    public static class RolesDown
    {
        public static List<Rol> Roles { get; set; } = new List<Rol>();
    }
    //theses roles are stored so that they can be sued for access control
    public class Rol
    {

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role {  get; set; }

    
    }

}
