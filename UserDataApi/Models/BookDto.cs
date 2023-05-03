namespace UserDataApi.Models {
    public class BookDto {
        public string title { get; set; }
        public string isbn { get; set; }
        public DateTime createdDate { get; set; }
        public int authorId { get; set; }
        public string description { get; set; }
        public Boolean isAvailable { get; set; }
        public DateTime unavailableUntil { get; set; }
    }
}
