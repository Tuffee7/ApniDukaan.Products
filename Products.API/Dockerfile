# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
# APP_UID user name is created in the base image and is used to run the application.
USER $APP_UID
# The working directory is set to /app, which is where the application will be copied to and run from app named folder. Not to confuse with APP_UID, which is the user that runs the application.
WORKDIR /app
# The application will listen on port 8080, which is the default port for ASP.NET Core applications. This port is exposed to the host machine so that the application can be accessed from outside the container.
EXPOSE 8080 
# The application will also listen on port 8081, which is used for HTTPS traffic. This port is also exposed to the host machine so that the application can be accessed securely from outside the container. 
EXPOSE 8081 


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
# ARG is used to create variable which is BUILD_CONFIGURATION and set to Release. This variable is used to specify the build configuration for the dotnet build command. The default value is Release, but it can be overridden by passing a different value when building the Docker image.
ARG BUILD_CONFIGURATION=Release 
WORKDIR /src 
# The Products.API.csproj file is copied to the /src/Products.API directory in the container. This is done to ensure that the project file is available for the dotnet restore command, which will restore the NuGet packages required by the project.
COPY ["Products.API/Products.API.csproj", "Products.API/"] 
# The dotnet restore command is run to restore the NuGet packages required by the Products.API project. This command reads the project file and downloads the necessary packages from NuGet.org or other configured package sources. The restored packages are stored in a global package cache, which is shared across all projects on the machine. This allows for faster builds and reduces the amount of disk space used by the container.
RUN dotnet restore "./Products.API/Products.API.csproj" 
# The entire contents of the current directory (.) on the host machine are copied to the /src directory in the container. This includes all source code files, configuration files, and other assets required by the application. This is done to ensure that all necessary files are available for the dotnet build command, which will compile the application and generate the output files.
COPY . . 
# switching back to the Products.API directory in the container, which is where the dotnet build command will be run. This is done to ensure that the build output files are generated in the correct location and can be easily copied to the final stage of the Dockerfile.
WORKDIR "/src/Products.API" 
# The dotnet build command is run to compile the Products.API project and generate the output files (DLL Files). The -c option specifies the build configuration (Release or Debug), which is set to the value of the BUILD_CONFIGURATION argument. The -o option specifies the output directory for the build files, which is set to /app/build in the container. This ensures that the build output files are generated in a known location that can be easily copied to the final stage of the Dockerfile.
RUN dotnet build "./Products.API.csproj" -c $BUILD_CONFIGURATION -o /app/build 

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish 
ARG BUILD_CONFIGURATION=Release
# The dotnet publish command is run to compile the Products.API project and generate the output files (DLL Files) for deployment. The -c option specifies the build configuration (Release or Debug), which is set to the value of the BUILD_CONFIGURATION argument. The -o option specifies the output directory for the published files, which is set to /app/publish in the container. The /p:UseAppHost=false option is used to disable the generation of an executable host file, which is not needed when running the application in a container.
RUN dotnet publish "./Products.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false 

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
ENV MSSQL_HOST=localhost
ENV MSSQL_PORT=1433
ENV MSSQL_DATABASE=ApniDukaanProducts
ENV MSSQL_USER=sa
ENV MSSQL_PASSWORD=admin
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Products.API.dll"]