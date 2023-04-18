using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using UserDataApi.Data;
using UserDataApi.Models;

namespace UserDataApi.Validation
{
    public class EmailIsValid
    {
        public static Boolean IsUnique(object value, UserContext _context) {
            foreach (User user in _context.Users) {
                if (user.Email.ToString() == value.ToString()) {
                    return false;
                }
            }
            return true;
        }
        public static Boolean IsUnique(object value, int id, UserContext _context) {
            foreach (User user in _context.Users) {
                if (user.Email.ToString() == value.ToString() && user.Id != id) {
                    return false;
                }
            }
            return true;
        }
        public static Boolean HasCorrectFormat(object value) {
            return Regex.Match(value.ToString(), @"^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$").Success;
        }
        public static String IsValid(object value, int id, UserContext _context) {
            if (!IsUnique(value, id, _context)) {
                return $"Email '{value}' is already in use.";
            }
            else if (!HasCorrectFormat(value)) {
                return $"Email '{value}' is invalid.";
            }
            return "";
        }
        public static String IsValid(object value, UserContext _context) {
            if (!IsUnique(value, _context)) {
                return $"Email '{value}' is already in use.";
            }
            else if (!HasCorrectFormat(value)) {
                return $"Email '{value}' is invalid.";
            }
            return "";
        }
    }
}