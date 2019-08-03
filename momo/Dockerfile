FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
#
#FROM microsoft/dotnet:2.2-sdk AS build
#WORKDIR /src
#COPY ["momo/momo.csproj", "momo/"]
#COPY ["momo.Application/momo.Application.csproj", "momo.Application/"]
#COPY ["momo.Domain/momo.Domain.csproj", "momo.Domain/"]
#COPY ["momo.Entity/momo.Entity.csproj", "momo.Entity/"]
#COPY ["momo.Infrastructure/momo.Infrastructure.csproj", "momo.Infrastructure/"]
#RUN dotnet restore "momo/momo.csproj"
#COPY . .
#WORKDIR "/src/momo"
#RUN dotnet build "momo.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "momo.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
ENTRYPOINT ["dotnet", "momo.dll"]