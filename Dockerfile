FROM mcr.microsoft.com/dotnet/core/sdk:3.1-focal AS build
WORKDIR /app
COPY . .
RUN dotnet publish src/Spirebyte.Services.Sprints.API -c release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-focal
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT Docker
ENTRYPOINT dotnet Spirebyte.Services.Sprints.API.dll