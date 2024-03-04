FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY HahnDeliveryBack/DeliveryAutomationSim/*.csproj ./HahnDeliveryBack
RUN dotnet restore HahnDeliveryBack

COPY ./HahnDeliveryBack/DeliveryAutomationSim/. ./HahnDeliveryBack
RUN dotnet publish HahnDeliveryBack -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/HahnDeliveryBack/out .

ENV ASPNETCORE_ENVIRONMENT=Production

# Ejecuta tu aplicación
ENTRYPOINT ["dotnet", "HahnDeliveryBack.dll"]
