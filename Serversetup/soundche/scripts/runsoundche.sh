!#/bin/bash

sudo docker run -p 0.0.0.0:80:80 boellebanden/soundchefinal

sudo docker run -p 0.0.0.0:80:80 -d boellebanden/soundchefinal

docker exec -it 71a81d0321ff /bin/bash

sudo docker rm $(sudo docker ps -aq)
sudo docker rmi $(docker images -q)
