#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["src/LetsGo.UI/LetsGo.UI.csproj", "src/LetsGo.UI/"]
COPY ["src/LetsGo.DAL/LetsGo.DAL.csproj", "src/LetsGo.DAL/"]
COPY ["src/LetsGo.Core/LetsGo.Core.csproj", "src/LetsGo.Core/"]
RUN dotnet restore "src/LetsGo.UI/LetsGo.UI.csproj"
COPY . .
WORKDIR "/src/src/LetsGo.UI"
RUN dotnet build "LetsGo.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LetsGo.UI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LetsGo.UI.dll"]