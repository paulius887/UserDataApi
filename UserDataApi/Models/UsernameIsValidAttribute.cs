using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace UserDataApi.Models {
    public class UsernameIsValidAttribute : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var _context = (UserContext)validationContext.GetService(typeof(UserContext));
            var entity = _context.Users.SingleOrDefault(e => e.Username == value.ToString());
            if (entity != null) {
                return new ValidationResult(GetNotUniqueErrorMessage(value.ToString()));
            }
            else if (Regex.Match(value.ToString(), @"^[a-zA-Z0-9_.]{6,30}$").Success) {
                return ValidationResult.Success;
            }
            else {
                return new ValidationResult(GetWrongFormatErrorMessage(value.ToString()));
            }
        }
        public string GetNotUniqueErrorMessage(string username) {
            return $"Username '{username}' is already in use.";
        }
        public string GetWrongFormatErrorMessage(string username) {
            return $"Username '{username}' is invalid. Username must be 6-30 characters long, and can only use alphanumeric characters, and special characters _ and .";
        }
    }
}