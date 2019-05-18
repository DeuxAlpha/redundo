#!/usr/bin/env bash
cd /srv/app && docker-compose down
cd /srv/app && docker-compose build
cd /srv/app && docker-compose up -d