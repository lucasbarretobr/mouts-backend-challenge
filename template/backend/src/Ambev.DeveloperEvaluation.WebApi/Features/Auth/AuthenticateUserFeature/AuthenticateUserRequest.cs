using System.ComponentModel;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;

/// <summary>
/// Represents the authentication request model for user login.
/// </summary>
public class AuthenticateUserRequest
{
    /// <summary>
    /// Gets or sets the user's email address for authentication.
    /// Must be a valid email format.
    /// </summary>
    [DefaultValue("admin@ambev.com")]
    public string Email { get; set; } = "admin@ambev.com";

    /// <summary>
    /// Gets or sets the user's password for authentication.
    /// Must match the stored password after hashing.
    /// </summary>
    [DefaultValue("Admin@123")]
    public string Password { get; set; } = "Admin@123";
}
