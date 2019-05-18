FROM postgres:alpine
ENV TIMEZONE=America/Detroit
RUN apk add tzdata && \
    cp /usr/share/zoneinfo/${TIMEZONE} /etc/localtime && \
    echo "${TIMEZONE}" > /etc/timezone && \
    apk del tzdata