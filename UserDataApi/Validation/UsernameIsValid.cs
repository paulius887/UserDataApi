using System.Text.RegularExpressions;
using UserDataApi.Data;
using UserDataApi.Models;

namespace UserDataApi.Validation
{
    public class UsernameIsValid
    {
        public static Boolean IsUnique(object value, UserContext _context) {
            foreach (User user in _context.Users) {
                if (user.Username.ToString() == value.ToString()) {
                    return false;
                }
            }
            return true;
        }
        public static Boolean IsUnique(object value, int id, UserContext _context) {
            foreach (User user in _context.Users) {
                if (user.Username.ToString() == value.ToString() && user.Id != id) {
                    return false;
                }
            }
            return true;
        }
        public static Boolean HasCorrectFormat(object value) {
            return Regex.Match(value.ToString(), @"^[a-zA-Z0-9_.]{6,30}$").Success;
        }
        public static String IsValid(object value, int id, UserContext _context) {
            if (!IsUnique(value, id, _context)) {
               return $"Username '{value}' is already in use.";
            }
            else if (!HasCorrectFormat(value)) {
                return $"Username '{value}' is invalid. Username must be 6-30 characters long, and can only use alphanumeric characters, and special characters _ and .";
            }
            return "";
        }
        public static String IsValid(object value, UserContext _context) {
            if (!IsUnique(value, _context)) {
                return $"Username '{value}' is already in use.";
            }
            else if (!HasCorrectFormat(value)) {
                return $"Username '{value}' is invalid. Username must be 6-30 characters long, and can only use alphanumeric characters, and special characters _ and .";
            }
            return "";
        }
    }
}