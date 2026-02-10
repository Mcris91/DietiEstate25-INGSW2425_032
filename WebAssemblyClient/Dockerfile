FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["WebAssemblyClient/WebAssemblyClient.csproj", "WebAssemblyClient/"]
RUN dotnet restore "WebAssemblyClient/WebAssemblyClient.csproj"
COPY . .
WORKDIR "/src/WebAssemblyClient"
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
COPY ["WebAssemblyClient/nginx.conf", "/etc/nginx/nginx.conf"]
