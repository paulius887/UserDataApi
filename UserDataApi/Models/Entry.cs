using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserDataApi.Models {
    public class Entry : EntryDto {
        public int UserId { get; set; }
        public int Id { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
