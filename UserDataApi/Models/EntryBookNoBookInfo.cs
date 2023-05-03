namespace UserDataApi.Models {
    public class EntryBookNoBookInfo {
        public Entry entry { get; set; }
        public string noBookData { get; set; }
        public EntryBookNoBookInfo (Entry entry) {
            this.entry = entry;
            noBookData = "Book data currently unavailable";
        }
    }
}
