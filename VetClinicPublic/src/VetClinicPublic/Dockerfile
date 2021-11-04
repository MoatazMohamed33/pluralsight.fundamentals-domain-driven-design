ARG BUILD_DIR="/build"
ARG OUT_DIR="out/"

# Build
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build
ARG BUILD_DIR
ARG OUT_DIR

WORKDIR ${BUILD_DIR}
COPY [ "*.csproj", "nuget.config", "${BUILD_DIR}/" ]
RUN dotnet restore --configfile nuget.config

COPY . ${BUILD_DIR}
RUN dotnet publish --configuration Release -o ${OUT_DIR}

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:5.0
ARG APP_DIR="/app"
ARG BUILD_DIR
ARG OUT_DIR

WORKDIR ${APP_DIR}
COPY --from=build ${BUILD_DIR}/${OUT_DIR} ${APP_DIR}
ENTRYPOINT [ "dotnet", "VetClinicPublic.dll" ]
