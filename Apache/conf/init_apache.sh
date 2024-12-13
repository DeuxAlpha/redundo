#!/bin/bash
# Check if letsencrypt certificates already exist
if [ ! -d /etc/letsencrypt/live ] || [ ! "$(ls -A /etc/letsencrypt)" ]; then
    certbot --apache --agree-tos -m ${WEBMASTER_MAIL} -d ${DOMAINS} --no-redirect --non-interactive
fi

# Check if letsencrypt certificates still don't exist (TODO: implement functions)
if [ -d /etc/letsencrypt/live ] && [ "$(ls -A /etc/letsencrypt)" ]; then
    rm /etc/apache2/sites-enabled/proxy-no-ssl*.conf
    a2ensite proxy-rewrite proxy-ssl
fi

service apache2 stop

echo "Starting apache server."
apachectl -D FOREGROUND