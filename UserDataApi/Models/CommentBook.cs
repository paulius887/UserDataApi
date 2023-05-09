namespace UserDataApi.Models {
    public class CommentBook {
        public Comment comment { get; set; }
        public Book book { get; set; }
        public CommentBook(Comment comment, Book book) {
            this.comment = comment;
            this.book = book;
        }
    }
}
