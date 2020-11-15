FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

COPY . .
RUN dotnet publish ./TwReplay/ -c Release -o ./prod

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/prod .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet TwReplay.dll