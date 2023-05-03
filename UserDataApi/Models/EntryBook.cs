namespace UserDataApi.Models {
    public class EntryBook {
        public Entry entry { get; set; }
        public Book book { get; set; }
        public EntryBook(Entry entry, Book book) {
            this.entry = entry;
            this.book = book;
        }
    }
}
