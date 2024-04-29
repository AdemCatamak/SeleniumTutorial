#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR ./

COPY ["/SeleniumTutorial/SeleniumTutorial.csproj", "SeleniumTutorial/"]
RUN dotnet restore "SeleniumTutorial/SeleniumTutorial.csproj"
COPY . .
WORKDIR "/SeleniumTutorial"
RUN dotnet build "SeleniumTutorial.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeleniumTutorial.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeleniumTutorial.dll"]