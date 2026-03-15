# Build and publish API
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ProjectManagementERP.sln ./
COPY src/ ./src/
COPY tests/ ./tests/
RUN dotnet restore ProjectManagementERP.sln
RUN dotnet publish src/API/ProjectManagementERP.API.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProjectManagementERP.API.dll"]
