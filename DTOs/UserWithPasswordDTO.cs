namespace DTOs
{
    public record UserWithPasswordDTO(
        int UserId,
        string UserEmail,
        string UserFirstName,
        string UserLastName,
        string UserPassword,
        string? City,
        string? Address,
        string? Phon,
        string Role = "User");
}
