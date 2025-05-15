FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file từ thư mục gốc
COPY ["TodoAppApi1.csproj", "./"]
RUN dotnet restore "TodoAppApi1.csproj"

# Copy toàn bộ source code
COPY . .
WORKDIR "/src"
RUN dotnet build "TodoAppApi1.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TodoAppApi1.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoAppApi1.dll"]