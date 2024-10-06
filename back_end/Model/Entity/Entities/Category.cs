using System.ComponentModel.DataAnnotations.Schema;

namespace Entity.Entities
{
    [Table("categories")]
    public class Category : BaseEntity<int>
    {
        public string? Name { get; set; }
    }
}
