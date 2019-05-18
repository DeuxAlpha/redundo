#!/usr/bin/env bash
cd /srv/app && git pull https://AaronPasschier:Redundo-Git-Password@purchase-manager.visualstudio.com/Purchase-Manager/_git/Docker
cd /srv/app/Frontend && git pull https://AaronPasschier:Redundo-Git-Password@purchase-manager.visualstudio.com/Purchase-Manager/_git/Frontend
cd /srv/app/Backend && git pull https://AaronPasschier:Redundo-Git-Password@purchase-manager.visualstudio.com/Purchase-Manager/_git/Backend
cd /srv/app/Apache && git pull https://AaronPasschier:Redundo-Git-Password@purchase-manager.visualstudio.com/Purchase-Manager/_git/Apache
