#### First Download Docker app: https://hub.docker.com/?overlay=onboarding

###### Docker Compose simplifies running more than one Docker container in conjunction. For Nakama, we’ll need two containers: one for Nakama itself and one for the database it relies on, CockroachDB.

###### You can choose to configure the Nakama and CockroachDB containers without Docker Compose but we do not recommend it when you’re starting out.

###### Docker Compose uses YAML configuration files to declare which containers to use and how they should work together.


### Running Nakama with docker-compose



1. Let’s start by creating the Nakama Docker-Compose file:
Create a file called docker-compose.yml and edit it in your favourite text editor:



docker-compose.yml
```
version: '3'
services:
  cockroachdb:
    container_name: cockroachdb
    image: cockroachdb/cockroach:v19.1.5
    command: start --insecure --store=attrs=ssd,path=/var/lib/cockroach/
    restart: always
    volumes:
      - data:/var/lib/cockroach
    expose:
      - "8080"
      - "26257"
    ports:
      - "26257:26257"
      - "8080:8080"
  nakama:
    container_name: nakama
    image: heroiclabs/nakama:2.7.0
    entrypoint:
      - "/bin/sh"
      - "-ecx"
      - >
          /nakama/nakama migrate up --database.address root@cockroachdb:26257 &&
          exec /nakama/nakama --name nakama1 --database.address root@cockroachdb:26257
    restart: always
    links:
      - "cockroachdb:db"
    depends_on:
      - cockroachdb
    volumes:
      - ./:/nakama/data
    expose:
      - "7349"
      - "7350"
      - "7351"
    ports:
      - "7349:7349"
      - "7350:7350"
      - "7351:7351"
    healthcheck:
      test: ["CMD", "curl", "-f", "http://localhost:7350/"]
      interval: 10s
      timeout: 5s
      retries: 5
volumes:
  data:
```

Windows users

If you are trying to run Nakama via Docker-Compose on Windows, you'll need to make a small change to the downloaded docker-compose.yml file. Follow this instruction to bind the correct path.

If logging output does not immediately appear in stdout add tty: true to the nakama service in your docker-compose.yml file.

2. Next, we’ll ask Docker Compose to follow the instructions in the file we just downloaded:

Shell


```
docker-compose -f docker-compose.yml up
```

Docker Compose will download the latest CockroachDB and Nakama images published on Docker Hub.

3. You now have both CockroachDB and Nakama running on your machine, available at 127.0.0.1:26257 and 127.0.0.1:7350 respectively.



### Setup Unity3D Project

Create a new project of Unity3D. Seach the nakama package in Unity asset store. Download and import the nakama pakage into your project.After deploy nakama in docker app, you could just connect nakama server.

```
//Use the connection credentials to build a client object.
// using Nakama;
const string scheme = "http";
const string host = "127.0.0.1";
const int port = 7350;
const string serverKey = "defaultkey";
var client = new Client(scheme, host, port, serverKey);

//Authenticate
var deviceId = SystemInfo.deviceUniqueIdentifier;
var session = await client.AuthenticateDeviceAsync(deviceId);
Debug.Log(session);
```

