#!/usr/bin/sudo bash
sudo heroku container:login && sudo heroku container:push web -a tyszkingg && sudo heroku container:release web -a tyszkingg
