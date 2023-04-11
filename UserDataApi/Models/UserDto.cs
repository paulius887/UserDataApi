namespace UserDataApi.Models {

    public class UserDto {
        [UsernameIsValid]
        public string Username { get; set; }

        [EmailIsValid]
        public string Email { get; set; }
    }
}
