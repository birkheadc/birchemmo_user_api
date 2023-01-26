# Birche Online User API

This is the API that manages Users and Login Sessions for the online game / website I am creating.

# The Code

## Environment Variables

The following environment variables are needed to run the application.

In development, these can be created as dotnet user-secrets via `dotnet user-secrets init` followed by `dotnet user-secrets set {key} {value}`

In production, I generally set the values through Docker. This may change if I decide not to use Docker for this application.

### Development

```
dotnet user-secrets set EmailConfigNoReply:Name no-reply
dotnet user-secrets set EmailConfigNoReply:Address {your email address here}
dotnet user-secrets set EmailConfigNoReply:SmtpServer {your email servers smtp server}
dotnet user-secrets set EmailConfigNoReply:Port {most likely 465 or 587}
dotnet user-secrets set EmailConfigNoReply:Username {your email account's username}
dotnet user-secrets set EmailConfigNoReply:Password {your email accounts password}
```

### Production

Same as above, but the variable names are slightly different

```
ASPNETCORE_EMAILCONFIG_NOREPLY_NAME
ASPNETCORE_EMAILCONFIG_NOREPLY_ADDRESS
ASPNETCORE_EMAILCONFIG_NOREPLY_SMTPSERVER
ASPNETCORE_EMAILCONFIG_NOREPLY_PORT
ASPNETCORE_EMAILCONFIG_NOREPLY_USERNAME
ASPNETCORE_EMAILCONFIG_NOREPLY_PASSWORD
```
