using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserDataApi.Models {
    public class BookEntries : Book {
        public List<Entry> userEntries { get; set; }
        public BookEntries(Book book) {
            this.id = book.id;
            this.title = book.title;
            this.isbn = book.isbn;
            this.createdDate = book.createdDate;
            this.author = book.author;
            this.description = book.description;
            this.isAvailable = book.isAvailable;
            this.unavailableUntil = book.unavailableUntil;
            this.userEntries = new List<Entry>();
        }
    }
}
