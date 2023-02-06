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

dotnet user-secrets set JwtConfig:Key {a random, secret string}
```

### Production

Same as above, but the variable names are slightly different

```
ASPNETCORE_EMAILCONFIG_USERNAME
ASPNETCORE_EMAILCONFIG_PASSWORD

ASPNETCORE_JWTCONFIG_KEY
```

# Tests

With this project I have finally gotten serious about testing, both Unit tests and Integration tests.

This application's tests are in separate projects but the same solution. Unit tests and Integration tests are each their own project.

Not much to say in this section except that I have finally seen the light of the usefulness of good Tests. Even the simple ones I've written so far save me a lot of time manually testing the project as retesting old features as it grows.

Testing has also ~~forced~~ led me to learn a lot about the ASP.NET framework that I had been taking for granted. It's hard to test an ActionFilter without knowing how an ActionContext is used, for example.

I'll continue putting more emphasis on testing going forward.