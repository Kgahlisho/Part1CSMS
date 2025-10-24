using System.ComponentModel.DataAnnotations;
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

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role {  get; set; }

    
    }

}
