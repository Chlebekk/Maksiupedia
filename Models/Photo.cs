using System.ComponentModel.DataAnnotations;

namespace Maksiupedia.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
