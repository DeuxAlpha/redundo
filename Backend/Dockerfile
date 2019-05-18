FROM mcr.microsoft.com/dotnet/core/sdk:2.2.204-alpine as build
WORKDIR /usr/local/app
COPY . .
RUN dotnet restore
RUN cd API && dotnet publish --no-restore -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.5-alpine
WORKDIR /usr/local/app
COPY --from=build /usr/local/app/API/out .
ENV TIMEZONE=America/Detroit
RUN apk add tzdata && \
    cp /usr/share/zoneinfo/${TIMEZONE} /etc/localtime && \
    echo "${TIMEZONE}" > /etc/timezone && \
    apk del tzdata
ENTRYPOINT ["dotnet", "API.dll"]