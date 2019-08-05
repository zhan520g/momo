FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 8079
WORKDIR /app
COPY . /app
ENTRYPOINT ["dotnet", "momo.dll"]