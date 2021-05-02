# Test task for HeidelbergCement

A simple Log Proxy API, which receives log messages and forwards them to Airtable.

## Getting Started

Clone the project to your local machine

```
git clone https://github.com/ugunay/log-proxy-api.git
```

### Building

1. Update appsettings.json Airtable->ApplicationId and Airtable->APIKey if you do want to work with your Airtable credentials.

## Running

### Run with Dotnet CLI

1. Run API project

2. You can use Swagger UI or Postman to test the API.

3. Provide basic authentication with:
   username: test
   password: test

### Run on Docker

```
cmd

docker build -t logproxytest:latest -f FULL_PATH_TO_THE_DOCKER_FILE .

docker run -p 8080:80 logproxytest:latest

Try with postman : http://localhost:8080/message
```

## Built With

- [.NET Core 3.1](https://dotnet.microsoft.com/)
- [AirtableApiClient](https://github.com/ngocnicholas/airtable.net)
- [Swagger](https://swagger.io/) - API developer tools for testing and documention
