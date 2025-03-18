# ใช้ .NET SDK สำหรับการ Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# คัดลอกไฟล์ทั้งหมดและ Restore Dependencies
COPY *.csproj .
RUN dotnet restore

# คัดลอกไฟล์ทั้งหมดและ Build
COPY . .
RUN dotnet publish -c Release -o /out

# ใช้ .NET Runtime สำหรับรันโปรแกรม
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out .

# เปิดพอร์ต 80 สำหรับ Web Application
EXPOSE 8080

# คำสั่งเริ่มต้น
ENTRYPOINT ["dotnet", "DemoSAP.dll"]
