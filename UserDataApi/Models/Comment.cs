using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserDataApi.Models {
    public class Comment : CommentDto {
        public int UserId { get; set; }
        public int Id { get; set; }
        public int BookId { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
