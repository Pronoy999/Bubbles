FROM mcr.microsoft.com/dotnet/sdk:5.0 AS Build
WORKDIR /app
COPY *.sln .
COPY BubblesAPI/*.csproj ./BubblesAPI/
COPY BubblesEngine/*.csproj ./BubblesEngine/
COPY BubblesAPITests/*.csproj ./BubblesAPITests/
COPY BubblesEngineTests/*.csproj ./BubblesEngineTests/

RUN dotnet restore
COPY BubblesAPI/. ./BubblesAPI
COPY BubblesEngine/. ./BubblesEngine

WORKDIR /app
RUN dotnet publish -c Release -o out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime 
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "BubblesAPI.dll"]