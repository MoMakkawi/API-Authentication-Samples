namespace Bearer_Authentication.Authentication.Models;

public sealed record LoginRequest(string UserName, string Password);