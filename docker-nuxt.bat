docker container rm redundo-nuxt-app -f
docker build -t redundo-nuxt ./Frotnend
docker run --name redundo-nuxt-app -p 3000:3000 redundo-nuxt