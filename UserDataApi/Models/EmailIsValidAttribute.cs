using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserDataApi.Models {
    public class EmailIsValidAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var _context = (UserContext)validationContext.GetService(typeof(UserContext));
            var entity = _context.Users.SingleOrDefault(e => e.Email == value.ToString());
            if (entity != null) {
                return new ValidationResult(GetNotUniqueErrorMessage(value.ToString()));
            }
            else if (Regex.Match(value.ToString(), @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$").Success) {
                return ValidationResult.Success;
            }
            else {
                return new ValidationResult(GetWrongFormatErrorMessage(value.ToString()));
            }
        }
        public string GetNotUniqueErrorMessage(string email) {
            return $"Email '{email}' is already in use.";
        }
        public string GetWrongFormatErrorMessage(string email) {
            return $"Email '{email}' is invalid.";
        }
    }
}