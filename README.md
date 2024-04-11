# AI-Dev-Reloaded
Providing solutions for the AI DEVS 2 Reloaded course.

# Utilizing Qdrant for Task search
To perform the task search, you should install Qdrant. It is recommended to install this software using Docker.
Visit the Qdrant website [here](https://qdrant.tech/).

To pull Qdrant with Docker, use:
```
docker pull qdrant/qdrant
```

Here are some helpful links to guide you through this task:
- [Setting https](https://systenics.ai/blog/2024-01-01-setting-up-qdrant-with-qdrant-dotnet/)
- [Creating a collection](https://qdrant.tech/documentation/concepts/collections/)
- [Implementing similarity search](https://qdrant.tech/documentation/concepts/search/)
- [Inserting embeddings](https://qdrant.tech/documentation/concepts/payload/)

To complete this task, I used the [qdrant-dotnet library](https://github.com/qdrant/qdrant-dotnet).

I used ngrok to solve the task OwnAPI is in the AI.Devs.OwnAPi project.

[ngrok](https://ngrok.com/)

1. Download and install the current version of ngrok.
2. Sign up and retrieve your token from [this page](https://dashboard.ngrok.com/auth).
3. Launch ngrok and set up the token you got from the website using the command: `ngrok authtoken YOUR_AUTHTOKEN`.
4. Create a tunnel and execute the command: `ngrok http --host-header=localhost https://localhost:7221`.