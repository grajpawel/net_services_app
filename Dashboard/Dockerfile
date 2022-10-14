FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

ENV ASPNETCORE\_URLS http://\*:5000

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Dashboard.csproj", "Dashboard/"]
RUN dotnet restore "Dashboard/Dashboard.csproj"
WORKDIR "/src/Dashboard"
COPY . .
RUN dotnet build "Dashboard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dashboard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dashboard.dll"]
