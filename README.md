# ASP .NET Core Web API Security Implementation Samples :
Note: There a lot of way to implement those ideas but i will show you mine.
## Bearer Authentication:
Most important things:
  -   In [appsettings.json](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Bearer%20Authentication/appsettings.json) i added some options for JWT tokens and i injected those values with the [JwtOptions](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Bearer%20Authentication/Options/JwtOptions.cs) class, i inject it in program file like : 
```cs
var jwtOptions = builder.Configuration
    .GetRequiredSection("JWT")
    .Get<JwtOptions>()!;
builder.Services.AddSingleton(jwtOptions);
```
  -  For Access Token and Refresh Token we interested in expiration date so I created models [TokenDetails](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Bearer%20Authentication/Authentication/Models/TokenDetails.cs), [AuthResult](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Bearer%20Authentication/Authentication/Models/AuthResult.cs) have Access and Refresh token details.
  -  An important class is in [JwtAuthenticationService](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Bearer%20Authentication/Authentication/Services/JwtAuthenticationService.cs), you will see the login service in this service the result is [AuthResult](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Bearer%20Authentication/Authentication/Models/AuthResult.cs) object, so we will generate Access and Refresh token and expiration dates, you will see `GenerateAccessToken` function in it create tokenDescriptor object that has Issuer and Audience and Expires, SigningCredentials we will determinate HS256 as encryption algorithm, and in Subject we add Claims, and for `GenerateRefreshToken` we can use any way to generate unique strings like `Guid.NewGuid()` or like we also worked on generated access token. 
  -  To make Endpoint Authorized you can use `.RequireAuthorization()`.
  -  If you are using swagger you must refactor it to make it support Bearer Auth:
```cs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add Bearer token authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = @"Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
        Name = "Bearer Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});
```

## Basic Authentication:
Most important things:
  -  Create [BasicAuthenticationHandler](https://github.com/MoMakkawi/ASP.NET-Core-Web-API-Security-Sample/blob/master/Basic%20Authentication/Authentication/BasicAuthenticationHandler.cs) class That is inherent AuthenticationHandler class,
  to implement the HandleAuthenticateAsync function in implementation,
 I check if there Authorization section in the header and I try to parse it to AuthenticationHeaderValue object,
 then I get the user name from the credentials after I decode it, I add my claims then I create AuthenticationTicket object.
  -  Then in the program file add Authentication & Authorization services and use them:
```cs 
builder.Services.AddAuthentication("Basic")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);
builder.Services.AddAuthorization();

// Some Code ...

app.UseAuthentication();
app.UseAuthorization();
```
  -  To make Endpoint Authorized Use `.RequireAuthorization()` or `[Authorize]`.
  - if you use swagger don't forget to change it to make it support Basic Auth:
```cs
builder.Services.AddSwaggerGen(c =>
{
    // Add basic authentication scheme to Swagger document
    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Basic",
        Description = "Basic authentication header"
    });

    // Add a requirement to use the defined authentication scheme in all operations
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "basic"
                }
            },
            Array.Empty<string>()
        }
    });
});

```
