using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserDataApi.Models {
    public class Entry : EntryDto {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime LastEdited { get; set; }
    }
}
