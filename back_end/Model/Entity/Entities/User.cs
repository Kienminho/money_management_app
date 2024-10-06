using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entities
{
    [Table("users")]
    public class User : BaseEntity<Guid>
    {
        public required string UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string? Avatar { get; set; }
        public string? AccesToken { get; set; }
        public string? RefreshToken { get; set; }

    }
}
