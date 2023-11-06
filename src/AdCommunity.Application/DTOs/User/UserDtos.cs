namespace AdCommunity.Application.DTOs.User
{
    public class UserBaseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public string? Website { get; set; }
        public string? Facebook { get; set; }
        public string? Twitter { get; set; }
        public string? Instagram { get; set; }
        public string? Github { get; set; }
        public string? Medium { get; set; }

        public UserBaseDto()
        {
        }

        public UserBaseDto(
            string firstName,
            string lastName,
            string email,
            string phone,
            string username,
            string? website,
            string? facebook,
            string? twitter,
            string? instagram,
            string? github,
            string? medium
        )
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Username = username;
            Website = website;
            Facebook = facebook;
            Twitter = twitter;
            Instagram = instagram;
            Github = github;
            Medium = medium;
        }
    }

    public class UserDto : UserBaseDto
    {
        public int Id { get; set; }
        public string HashedPassword { get; set; }

        public UserDto(string hashedPassword)
        {
            HashedPassword = hashedPassword;
        }

        public UserDto()
        {
        }
    }

    public class UserCreateDto: UserBaseDto
    {
        public string Password { get; set; }

        public UserCreateDto(string password)
        {
            Password = password;
        }

        public UserCreateDto()
        {
        }

    }

    public class UserUpdateDto : UserBaseDto
    {
        public int Id { get; set; }
        public string Password { get; set; }

        public UserUpdateDto(string password)
        {
            Password = password;
        }

        public UserUpdateDto()
        {
        }
    }

    public class UserLoginDto 
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public UserLoginDto()
        {
        }

        public UserLoginDto(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
