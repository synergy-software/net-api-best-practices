using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sample.API.Controllers {
    public class TodoItem
    {
        public long Id { get; set; }

        // TODO: zamień [Required] na non-nullable
        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}