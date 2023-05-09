namespace UserDataApi.Models {
    public class CommentBookNoBookInfo {
        public Comment comment { get; set; }
        public string noBookData { get; set; }
        public CommentBookNoBookInfo (Comment comment) {
            this.comment = comment;
            this.noBookData = "Book data currently unavailable";
        }
    }
}
