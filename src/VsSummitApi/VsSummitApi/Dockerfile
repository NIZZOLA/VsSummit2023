#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:7.0-jammy as base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VsSummitApi/VsSummitApi.csproj", "VsSummitApi/"]
RUN dotnet restore "VsSummitApi/VsSummitApi.csproj"
COPY . .
WORKDIR "/src/VsSummitApi"
RUN dotnet build "VsSummitApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VsSummitApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VsSummitApi.dll"]