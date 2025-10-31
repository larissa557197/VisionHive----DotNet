namespace VisionHive.Application.DTO.Request;

/// <summary>
/// Modelo simples para login (email + password).
/// </summary>
public class LoginRequest
{
    
    /// <summary>Nome de usuário (ex: admin)</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Senha do usuário (ex: 1234)</summary>
    public string Password { get; set; } = string.Empty;
}