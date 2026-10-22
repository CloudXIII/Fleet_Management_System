FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Fleet Management System/Fleet Management System.csproj", "Fleet Management System/"]
RUN dotnet restore "Fleet Management System/Fleet Management System.csproj"

COPY ["Fleet Management System/", "Fleet Management System/"]
WORKDIR "/src/Fleet Management System"
RUN dotnet publish "Fleet Management System.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Fleet Management System.dll"]
