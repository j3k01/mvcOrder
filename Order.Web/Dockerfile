FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM debian

RUN apt-get update && apt-get install -y curl wget netcat

FROM debian

# Instalacja wymaganych pakietów
RUN apt-get update && apt-get install -y curl wget gnupg

# Pobranie i instalacja Microsoft SQL Server
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list
RUN apt-get update && ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools

# Instalacja SQL Server Browser
RUN apt-get install -y sqlbrowser


FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Order.Web/Order.Web.csproj", "Order.Web/"]
RUN dotnet restore "Order.Web/Order.Web.csproj"
COPY . .
WORKDIR "/src/Order.Web"
RUN dotnet build "Order.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Order.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Order.Web.dll"]

# Install SQL Server dependencies
RUN apt-get update && \
    apt-get install -y curl gnupg2 && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y msodbcsql17 mssql-tools && \
    apt-get install -y unixodbc-dev && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
