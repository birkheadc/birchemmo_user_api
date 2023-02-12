FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal AS base
WORKDIR /app
EXPOSE 5000

ARG ENVIRONMENT

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=${ENVIRONMENT}

ARG CONNECTION_STRING

ARG EMAILCONFIG_USERNAME
ARG EMAILCONFIG_PASSWORD

ARG JWTCONFIG_KEY

ENV ASPNETCORE_EMAILCONFIG_USERNAME=${EMAILCONFIG_USERNAME}
ENV ASPNETCORE_EMAILCONFIG_PASSWORD=${EMAILCONFIG_PASSWORD}

ENV ASPNETCORE_JWTCONFIG_KEY=${JWTCONFIG_KEY}

ENV ASPNETCORE_DBCONFIG_CONNECTION_STRING=${CONNECTION_STRING}

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
# RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
# USER appuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["birchemmo_user_api.csproj", "./"]
RUN dotnet restore "birchemmo_user_api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "birchemmo_user_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "birchemmo_user_api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY /assets ./assets


ENTRYPOINT ["dotnet", "birchemmo_user_api.dll"]