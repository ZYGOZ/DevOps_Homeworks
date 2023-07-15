#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Sevice2.csproj", "."]
RUN dotnet restore "./Sevice2.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "Sevice2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sevice2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sevice2.dll"]