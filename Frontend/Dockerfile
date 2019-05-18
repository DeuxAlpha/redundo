FROM node:alpine
WORKDIR /usr/local/app
ENV TIMEZONE=America/Detroit
RUN apk add tzdata && \
    cp /usr/share/zoneinfo/${TIMEZONE} /etc/localtime && \
    echo "${TIMEZONE}" > /etc/timezone && \
    apk del tzdata
ARG API_BROWSER_URL
ENV API_BROWSER_URL=${API_BROWSER_URL}
COPY ./package.json ./
RUN npm install --production
RUN npm audit fix
COPY . .
RUN npm run build
ENV HOST=0.0.0.0
ENTRYPOINT ["npm", "start"]