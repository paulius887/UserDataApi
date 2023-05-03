namespace UserDataApi.Models {
    public class EntryBookDto {
        public EntryDto entryDto { get; set; }
        public BookDto bookDto { get; set; }
        public EntryBookDto(EntryDto entryDto, BookDto bookDto) {
            this.entryDto = entryDto;
            this.bookDto = bookDto;
        }
    }
}
