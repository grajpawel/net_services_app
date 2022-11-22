FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MessageGenerator.csproj", "MessageGenerator/"]
RUN dotnet restore "MessageGenerator/MessageGenerator.csproj"
WORKDIR "/src/MessageGenerator"
COPY . .
RUN dotnet build "MessageGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MessageGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MessageGenerator.dll"]
