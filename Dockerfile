FROM mcr.microsoft.com/dotnet/core/sdk
WORKDIR /app
EXPOSE 80
EXPOSE 443
WORKDIR /src
COPY . .
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet tool install -g dotnet-ef --version 3.1.1
# Solution by : https://stackoverflow.com/a/59814147/5774018
RUN dotnet publish SoundcheFinal/SoundcheFinal.csproj -c Release -o /app/publish
ENTRYPOINT ["dotnet", "/app/publish/SoundcheFinal.dll"]


#FOr some reason it starts in an interactive shell if I do docker run soundche
