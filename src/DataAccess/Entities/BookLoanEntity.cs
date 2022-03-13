using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    public class BookLoanEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BookLoanId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public bool ShouldBeReturnedInTwoWeeks => new TimeSpan(DateTime.Now.Ticks) >= (To - new DateTime(0, 0, 14));
        public int TimesExtended { get; set; }

        [Required]
        public CustomerEntity Customer { get; set; } = null!;
        public Guid LoanedTo { get; set; }

        public LibrarianEntity? Librarian { get; set; } = null!;
        public Guid? LoanedBy { get; set; }

        [Required]
        public BookItemEntity BookItem { get; set; } = null!;
    }        
}
