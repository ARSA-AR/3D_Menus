# 1. Aşama: Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Proje dosyasını kopyala ve restore et
COPY ["ArModelMenu.csproj", "./"]
RUN dotnet restore "ArModelMenu.csproj"

# Diğer dosyaları kopyala ve yayınla (Publish)
COPY . .
RUN dotnet publish "ArModelMenu.csproj" -c Release -o /app/publish

# 2. Aşama: Çalıştırma (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Render dinamik port kullanır, bu yüzden portu değişkenden alıyoruz veya 8080'i açıyoruz
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "ArModelMenu.dll"]
