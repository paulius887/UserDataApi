using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserDataApi.Models {
    public class BookComments : Book {
        public List<Comment> userComments { get; set; }
        public BookComments(Book book) {
            this.id = book.id;
            this.title = book.title;
            this.isbn = book.isbn;
            this.createdDate = book.createdDate;
            this.author = book.author;
            this.description = book.description;
            this.isAvailable = book.isAvailable;
            this.unavailableUntil = book.unavailableUntil;
            this.userComments = new List<Comment>();
        }
    }
}
