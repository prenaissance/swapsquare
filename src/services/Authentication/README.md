# SwapSquare Authentication Service

This service is a standalone web app that represents an OAuth2 provider.
Routes related to authentication for the main app are gatewayed.

## Development

To generate secrets for authentication run the followings commands:

1. (Already done) Initialize dotnet user-secrets for this service and for the gateway:

```bash
dotnet user-secrets --project=src/services/Authentication/Authentication.Api/ init &&\
dotnet user-secrets --project=src/Gateways/SwapSquare.ApiGateway init
```

2. Generate a private key for signing JWTs:

```bash
openssl genrsa -out ~/private-rsa.pem 2048
```

3. Set the private key as a user-secret in the authentication service:

```bash
dotnet user-secrets --project=src/services/Authentication/Authentication.Api/ set "Jwt:PrivateKey" "$(cat ~/private-rsa.pem)"
```

4. Set the public key as a user-secret in the authentication service:

```bash
dotnet user-secrets --project=src/services/Authentication/Authentication.Api/ set "Jwt:PublicKey" "$(openssl rsa -in ~/private-rsa.pem -pubout -outform PEM)"
```

5. Set the public key as a user-secret in the gateway:

```bash
dotnet user-secrets --project=src/Gateways/SwapSquare.ApiGateway set "Jwt:PublicKey" "$(openssl rsa -in ~/private-rsa.pem -pubout -outform PEM)"
```

6. Remove the private key file:

```bash
rm ~/private-rsa.pem
```
