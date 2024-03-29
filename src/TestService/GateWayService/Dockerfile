#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["GateWayService/GateWayService.csproj", "GateWayService/"]
COPY ["Repository/Repository.csproj", "Repository/"]
COPY ["IRepository/IRepository.csproj", "IRepository/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["IServices/IServices.csproj", "IServices/"]
RUN dotnet restore "GateWayService/GateWayService.csproj"
COPY . .
WORKDIR "/src/GateWayService"
RUN dotnet build "GateWayService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GateWayService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GateWayService.dll"]