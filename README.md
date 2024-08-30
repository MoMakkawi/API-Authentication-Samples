# ASP .NET Core Web API Security Implementation Samples :
Note: There a lot of way to implement those ideas but i will show you mine.
## Basic Authentication:
Most important things:
  -  Create BasicAuthenticationHandler class That is inherent AuthenticationHandler class,
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

