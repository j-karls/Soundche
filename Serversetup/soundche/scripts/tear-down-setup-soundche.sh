#!/bin/bash

# Tear down old containers and images
docker rm $(sudo docker ps -aq)
docker rmi $(docker images -q)

# Create folder structure if it doesn't already exist
mkdir /soundche
mkdir /soundche/scripts
mkdir /soundche/running
mkdir /soundche/backup
mkdir /soundche/backup/bak
mkdir /soundche/backup/logs

# Download new image and create container
docker pull boellebanden/soundchefinal
ID=(docker run -p 0.0.0.0:80:80 -d boellebanden/soundchefinal)

# Override the running container name, such that the backup script knows where to get its files
echo $ID > /soundche/running/container.id



# There we go, this almost ought to work right out of the box. 
# All that's missing is to place the backupscript into soundche/scripts and to place the cronjob into sudo crontab -e
