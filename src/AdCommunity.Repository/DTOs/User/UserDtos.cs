namespace AdCommunity.Repository.DTOs.User;

public class UserBaseDto
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }
    public string? HashedPassword { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public string? Website { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? Github { get; set; }

    public string? Medium { get; set; }
}

public class UserDto : UserBaseDto
{
    public int Id { get; set; }
}

public class UserCreateDto : UserBaseDto
{
    public DateTime? CreatedOn { get; set; }
}

public class UserUpdateDto : UserBaseDto
{
    public int Id { get; set; }
}

public class UserLoginDto
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}