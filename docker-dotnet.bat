docker container rm redundo-dotnet-app -f
docker build -t redundo-dotnet ./Backend
docker run --name redundo-dotnet-app -p 5000:5000 redundo-dotnet