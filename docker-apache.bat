docker container rm redundo-apache-app -f
docker build -t redundo-apache --build-arg SPA_PORT=3000 ./Apache
docker run --name redundo-apache-app -p 8080:80 redundo-apache
