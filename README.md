# SocketMessaging (Yes this is the title)

* The intended audience is anybody, there is no direct targetting. Anybody with a Windows machine can run this.

* The Project is a Client/Server pair which runs my own SocketNetworking library for its TCP transport layer, it is just, a chat platform. You can create channels, send messages (which are saved on the server) and reconnect etc. You can sign up or log in with a username and password. The concept is effectively a discord knockoff, a simple chat app that is not centrialized to a single server. (As people can host servers as they please rather then relying on my servers.) This Program functions using a TCP/IP connection allowing for latent but still fast and reliable communication between server and client without the risk of Packets being missed. I have also implemented a Network Object system allowing management of pieces of code on the server and client which allows the client to have access to the servers list of clients.

* The required features are as follows:
    - The TCP Transport Layer

    - The Chat UI itself (Roman)

    - The Server side functionality (Me), this includes message saving and channels

    - User auth and password hashing functions

* What I would like to add:
    - Image and multimedia uploads

    - Channel permissions and client side user management

    - A sign up proccess that isn't just a username password pair

    - End to End Encryption

* In order to complete the project, I needed an understanding of C#'s reflection API, socket API, and general serialization. This was done prior to the project starting, with my library. Which at the time of starting was untested, which meant that a lot of functionality was not present or otherwise was not functional. In essense, to complete the features I would like to add, I would need to properly clense uploaded files and creates a file cache on the local clients and server. In additon, I would need to implement a better system of sending large amounts of data through my library as there is a fixed 65kb limit per packet, which is not enough for must multimedia uploads. In order to create user management, I would need to create proper authentication and probably add additional flags and settings to both users and channels. In order for me to have completed this project, I would also have to le-earn windows forms as I last used them nearly 2 years ago, and also learn how to use the visual studio debugger, which proved to be extremely usefull. In order to implement Encryption, I would need to first modify my existing library to allow me to ecnrypt packets with both Public-Private key encryption standards as well as just Password based encryption. This would require me to learn how such a handshake works from something like the HTTPS protocol, as writing my own may be insecure.

# Install Instructions

* clone this repo and this one: https://github.com/BTELNYY/SocketNetworking

* You will need to place both of these folders in the same parent directory.

* Install Visual Studio 2022 (Only tested on 2022)

* Make sure .NET Framework 4.8 SDK and Targetting Pack are installed.

* Compile in "Release" Mode

* Run the server (in the bin/Release/ of its subfolder, SocketMessagingServer), then repeat for the client

* You will be prompted with a IP Connection screen. By default, there is no password (leave blank), the localhost IP is 127.0.0.1 and the port is 7777

* You will be prompted with a login screen. Press "Sign-Up", enter your details and **remember them**

* Re-input your credentials into the login screen.

* By default, the server has no channels, you will need to create one by going to the server console and typing in "createchannel <name> <description>", at this point the client will have the channel and you can click on it on the left. 

* You can send messages as you wish and the server will record them all, for reasons of TCP socket limits, I cannot send more then 50 messages per initial sync so only the last 50 messages or so will be shown when you connect for the second or third time with more then 50 messages in the channel already.

* To exit, exit the app. Ignore the error present in Console as this is intended behavior.

* The only errors that are NOT Intended are ones which crash the application, if a warning or error is printed, it is generally safe to ignore it.