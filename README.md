# Birche Online User API

This is the API that manages Users and Login Sessions for the online game / website I am creating.

# The Code

## Environment Variables

The following environment variables are needed to run the application.

Some of these variables are not sensitive, and so are read in via `appsettings.json`. Sensitive variables, like the JwtConfig:Key and email login credentials, should be treated with more care.

In development, these can be created as dotnet user-secrets via `dotnet user-secrets init` followed by `dotnet user-secrets set {key} {value}`

In production, I generally set the values through Docker, via a docker-compose file that is not commited to version control. This may change if I decide not to use Docker for this application.

### Development

```
dotnet user-secrets set EmailConfigNoReply:Username {your email account's username}
dotnet user-secrets set EmailConfigNoReply:Password {your email accounts password}

dotnet user-secrets set JwtConfig:Key {a random string of at least n length, keep secret (I can't remember what n is...)}
```

### Production

Same as above, but the variable names are slightly different

```
ASPNETCORE_EMAILCONFIG_USERNAME
ASPNETCORE_EMAILCONFIG_PASSWORD

ASPNETCORE_JWTCONFIG_KEY
```
