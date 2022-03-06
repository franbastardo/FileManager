using System.ComponentModel.DataAnnotations;

namespace FileManager.DTOs
{
    public class UpdateFileDTO
    {
        [Required]
        public string NewName { get; set; }
        [Required]
        public string OldName { get; set; }
    }
}
