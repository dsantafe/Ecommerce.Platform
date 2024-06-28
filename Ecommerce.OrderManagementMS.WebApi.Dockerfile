FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Ecommerce.Application/Ecommerce.Application.csproj", "Ecommerce.Application/"]
COPY ["Ecommerce.Domain/Ecommerce.Domain.csproj", "Ecommerce.Domain/"]
COPY ["Ecommerce.Infrastructure/Ecommerce.Infrastructure.csproj", "Ecommerce.Infrastructure/"]
COPY ["Ecommerce.OrderManagementMS.WebApi/Ecommerce.OrderManagementMS.WebApi.csproj", "Ecommerce.OrderManagementMS.WebApi/"]
RUN dotnet restore "Ecommerce.OrderManagementMS.WebApi/Ecommerce.OrderManagementMS.WebApi.csproj"
COPY . .
WORKDIR "/src/Ecommerce.OrderManagementMS.WebApi"
RUN dotnet build "Ecommerce.OrderManagementMS.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ecommerce.OrderManagementMS.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.OrderManagementMS.WebApi.dll"]