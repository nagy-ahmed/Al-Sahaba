namespace Al_Sahaba.Web.Core.Models
{
    [Index(nameof(Title), nameof(AuthorId), IsUnique = true)]
    public class Book : BaseModel
    {
        public int Id { get; set; }

        [MaxLength(500)]
        public string Title { get; set; } = null!;

        [MaxLength(200)]
        public string Description { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public DateTime PublishingDate { get; set; }

        public string? ImageUrl { get; set; }

        public string? Hall { get; set; }

        public bool IsAvailableForRent { get; set; }

        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();

    }
}
