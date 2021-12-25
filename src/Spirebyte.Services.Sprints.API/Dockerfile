#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1-focal AS build
WORKDIR /src
COPY ["src/Sprint-Service/src/Spirebyte.Services.Sprints.API/Spirebyte.Services.Sprints.API.csproj", "src/Sprint-Service/src/Spirebyte.Services.Sprints.API/"]
COPY ["src/Sprint-Service/src/Spirebyte.Services.Sprints.Infrastructure/Spirebyte.Services.Sprints.Infrastructure.csproj", "src/Sprint-Service/src/Spirebyte.Services.Sprints.Infrastructure/"]
COPY ["src/Sprint-Service/src/Spirebyte.Services.Sprints.Application/Spirebyte.Services.Sprints.Application.csproj", "src/Sprint-Service/src/Spirebyte.Services.Sprints.Application/"]
COPY ["src/Sprint-Service/src/Spirebyte.Services.Sprints.Core/Spirebyte.Services.Sprints.Core.csproj", "src/Sprint-Service/src/Spirebyte.Services.Sprints.Core/"]
RUN dotnet restore "src/Sprint-Service/src/Spirebyte.Services.Sprints.API/Spirebyte.Services.Sprints.API.csproj"
COPY . .
WORKDIR "/src/src/Sprint-Service/src/Spirebyte.Services.Sprints.API"
RUN dotnet build "Spirebyte.Services.Sprints.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Spirebyte.Services.Sprints.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Spirebyte.Services.Sprints.API.dll"]
