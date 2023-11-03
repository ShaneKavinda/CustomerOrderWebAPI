using System.ComponentModel.DataAnnotations;

namespace testApi.Models
{
    public class Customer
    {
        public Guid UserID { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public DateTime CreatedOn { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
