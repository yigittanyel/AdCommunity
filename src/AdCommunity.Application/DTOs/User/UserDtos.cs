namespace AdCommunity.Application.DTOs.User
{
    public record UserBaseDto(
        string? FirstName,
        string? LastName,
        string? Email,
        string? Password,
        string? HashedPassword,
        string? Phone,
        string? Username,
        string? Website,
        string? Facebook,
        string? Twitter,
        string? Instagram,
        string? Github,
        string? Medium
    );

    public record UserDto(int Id, UserBaseDto Data);

    public record UserCreateDto(UserBaseDto Data, DateTime? CreatedOn);

    public record UserUpdateDto(int Id, UserBaseDto Data);

    public record UserLoginDto(string? Username, string? Password);
}
