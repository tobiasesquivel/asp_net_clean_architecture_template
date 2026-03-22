namespace Infrastructure.Auth.Options;


public class JwtOptions
{
    public const string SectionName = "Jwt";
    public string SectretKey { get; set; } = String.Empty;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
}
