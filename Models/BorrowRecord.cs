using System.ComponentModel.DataAnnotations;

namespace LibraryCoreWeb.Models
{
    public class BorrowRecord
    {
        [Key]
        public int BorrowId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned { get; set; }
    }

}
