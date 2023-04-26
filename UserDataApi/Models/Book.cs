namespace UserDataApi.Models {
    public class Book {
        public int id { get; set; }
        public string title { get; set; }
        public string isbn { get; set; }
        public string createdDate { get; set; }
        public Author author { get; set; }
        public string description { get; set; }
        public Boolean isAvailable { get; set; }
        public DateTime unavailableUntil { get; set; }
    }
}
