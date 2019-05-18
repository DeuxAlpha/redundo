FROM ubuntu:latest
ENV DEBIAN_FRONTEND=noninteractive
ENV WEBMASTER_MAIL=${WEBMASTER_MAIL}
ENV DOMAINS=${DOMAINS}
ENV TIMEZONE=America/Detroit

RUN apt-get update && \
    apt-get install -y tzdata && \
    ln -fs /usr/share/zoneinfo/America/New_York /etc/localtime && \
    dpkg-reconfigure --frontend noninteractive tzdata && \
    apt-get install apache2 -y && \
    apt-get install software-properties-common -y && \
    add-apt-repository universe -y && \
    add-apt-repository ppa:certbot/certbot -y && \
    apt-get install python-certbot-apache -y

WORKDIR /etc/apache2
RUN rm ./sites-enabled/000-default.conf
COPY ./conf/apache2.conf .
COPY ./conf/init_apache.sh .
COPY ./conf/sites-available ./sites-available
COPY ./conf/sites-enabled ./sites-enabled
COPY ./conf/mods-enabled ./mods-enabled

ENTRYPOINT[ "bash", "/etc/apache2/init_apache.sh" ]