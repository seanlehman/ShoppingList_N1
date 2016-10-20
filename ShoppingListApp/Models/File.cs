using System.ComponentModel.DataAnnotations;

namespace ShoppingListApp.Models
{
    public class File
    {
        public int FileId { get; set; }
        [StringLength(255)]
        public string FileName { get; set; }
        [StringLength(100)]
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public FileType Filetype { get; set; }
        public int ShoppingListId { get; set; }
        public virtual ShoppingList ShoppingList { get; set; }
    }
}
