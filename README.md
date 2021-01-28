# Welcome to Grecha!

## Installation
1. Install Docker Desktop from https://www.docker.com/products/docker-desktop.
2. Launch Docker Desktop application.
3. Download the project from master branch **OR** git clone it from master branch to your desktop.
4. Open the project folder.
5. Open cmd in the project folder and enter command **docker-compose up**
6. Wait for dependencies to download.
7. Open a browser and enter URL **http://localhost:8080/**
**If the port is not available**, change it in *docker-compose.yml* file to another one (ports section where "8080:80" is written. Example: 8080:80 -> 8081:80).
8. Enjoy the project.

:warning: **Problems with Nuget 26.01.2021 - XX.XX.XXXX**: nuget certificate errors in sdk:5.0-buster-slim. To solve the problem change: 
**`mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim`**  ---> **`mcr.microsoft.com/dotnet/aspnet:5.0-focal`** 
**`mcr.microsoft.com/dotnet/sdk:5.0-buster-slim`** --->  **`mcr.microsoft.com/dotnet/sdk:5.0-focal`**  
In the **/Grecha/Dockerfile** 

## Using the project
When you start the application, you will be taken to the main page with products. **At first launch you will need to wait for a while** because of data being parsed.

 - Groats is parsed from three stores: ATB, Fozzy, Novus.
 - There is a filter on the main page. With the help of it, you can sort groats by price.
 - You can view specific store products by switching between them with the help of the sidebar menu. To open it press the "bars" icon that is placed at the left in the navbar.

## P.S.
Sample project is running at: http://grecha.southcentralus.azurecontainer.io/
