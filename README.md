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


## Using the project
When you start the application, you will be taken to the main page with products. **At first launch you will need to wait for a while** because of data being parsed.

 - Groats is parsed from three stores: ATB, Fozzy, Novus.
 - There is a filter on the main page. With the help of it, you can sort groats by price or name.
 - You can view specific store products by switching between them with the help of the sidebar menu. To open it press the "bars" icon that is placed at the left in the navbar.

## P.S.
Sample project is running at: https://grechaf0x.southcentralus.azurecontainer.io
