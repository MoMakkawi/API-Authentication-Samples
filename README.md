# ASP .NET Core Web API Security Implementation Samples :
Note: There a lot of way to implement those ideas but i will show you mine.
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
