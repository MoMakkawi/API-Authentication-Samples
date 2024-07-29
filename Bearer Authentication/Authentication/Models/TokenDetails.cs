namespace Bearer_Authentication.Authentication.Models;

public sealed record TokenDetails(string Token, DateTime ExpireAt);