FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

# set up network
ENV ASPNETCORE_URLS http://+:80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["FastNet.TaskManager/FastNet.TaskManager.csproj", "./"]
#COPY ["FastNet.TaskManager/nuget.config", "./"]
RUN dotnet restore "FastNet.TaskManager.csproj" -s https://api.nuget.org/v3/index.json
WORKDIR "/src/FastNet.TaskManager"
COPY . .
RUN dotnet build "FastNet.TaskManager/FastNet.TaskManager.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FastNet.TaskManager/FastNet.TaskManager.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FastNet.TaskManager.dll"]