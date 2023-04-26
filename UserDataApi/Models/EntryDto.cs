using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace UserDataApi.Models {
    public class EntryDto {
        public string EntryText { get; set; }
        public int BookId { get; set; }
    }
}
