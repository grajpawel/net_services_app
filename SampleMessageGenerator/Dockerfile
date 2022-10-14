FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["net_services_app/SampleMessageGenerator/SampleMessageGenerator.csproj", "SampleMessageGenerator/"]
RUN dotnet restore "SampleMessageGenerator/SampleMessageGenerator.csproj"
COPY . .
WORKDIR "/src/SampleMessageGenerator"
RUN dotnet build "SampleMessageGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleMessageGenerator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleMessageGenerator.dll"]
