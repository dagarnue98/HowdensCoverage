FROM mcr.microsoft.com/dotnet/sdk:6.0 AS installer-env

# Build requires 3.1 SDK
COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet

##Copy content
COPY . .

#RUN echo $(ls -1)
RUN  cd ./PublishSystem.Functions.Orchestrator && \
     mkdir -p ./home/site/wwwroot && \
     dotnet publish *.csproj --output ./home/site/wwwroot

# To enable ssh & remote debugging on app service change the base image to the one below
# FROM mcr.microsoft.com/azure-functions/dotnet:4-appservice
FROM mcr.microsoft.com/azure-functions/dotnet:4

ENV AzureWebJobsScriptRoot=./PublishSystem.Functions.Orchestrator/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true

COPY --from=installer-env ["./PublishSystem.Functions.Orchestrator/home/site/wwwroot", "./PublishSystem.Functions.Orchestrator/home/site/wwwroot"]