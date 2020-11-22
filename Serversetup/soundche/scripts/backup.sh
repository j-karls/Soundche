#!/bin/bash

# Backs up the database from within the container
# To get this working, you need the folder structure:
# /soundche
# /soundche/scripts
# /soundche/backup
# /soundche/backup/bak
# /soundche/backup/logs

# Place this file into /soundche/scripts/backup.sh

# Then place the following into your sudo crontab:
# 00 07 * * * /soundche/scripts/backup.sh `cat /soundche/running/container.id` &> /soundche/backup/logs/backup_`date +\%Y\%m\%d\%H\%M`.log

CONTAINERID=$1
DATE=$(date +%Y%m%d%H%M)
echo $DATE

sudo docker cp $1:/app/lite.db /soundche/backup/bak/lite.db.$DATE.bak

# Remove old backup files
find "/soundche/backup/bak" -name "lite.db.*.bak" -mtime +100 -delete

