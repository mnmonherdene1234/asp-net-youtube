FROM bitnami/dotnet-sdk:7.0.203

WORKDIR /app

COPY . .

CMD [ "dotnet", "published/AspNetYoutube.dll" ]