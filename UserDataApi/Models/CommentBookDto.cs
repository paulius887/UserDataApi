namespace UserDataApi.Models {
    public class CommentBookDto {
        public CommentDto commentDto { get; set; }
        public BookDto bookDto { get; set; }
        public CommentBookDto(CommentDto commentDto, BookDto bookDto) {
            this.commentDto = commentDto;
            this.bookDto = bookDto;
        }
    }
}
