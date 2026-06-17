# Ubuntu.Landscape.Client

[![CI](https://github.com/pierrejochem/Ubuntu.Landscape.Client/actions/workflows/ci.yml/badge.svg)](https://github.com/pierrejochem/Ubuntu.Landscape.Client/actions/workflows/ci.yml)

The Ubuntu.Landscape.Client .NET library is doing API calls to the Landscape Server. It is written in C# and targets **.NET 10**.

### requirements

- [.NET SDK 10.0](https://dotnet.microsoft.com/download) or later

### build

```sh
dotnet build -c Release
```

### pack (NuGet)

```sh
dotnet pack -c Release
```

Produces `Ubuntu.Landscape.Client/bin/Release/Ubuntu.Landscape.Client.1.0.0.nupkg`.

### usage

```c#
var client = new Ubuntu.Landscape.Client();
client.hostname = "hostname";
client.secretKey = "secret key";
client.accessKey = "access key";
client.ignoreInvalidCerts = true;
client.actionString = "GetAlerts";

var JsonOutput = client.getResult();
```

For possible action strings please have a look here:

[Landscape API’s documentation](https://landscape.canonical.com/static/doc/api/#getting-started-with-the-api)

Addional contributors are welcome!
