#Ubuntu.Landscape.Client

The Ubuntu.Landscape.Client .Net library is doing API calls to the Landscape Server. It is written in C#.

###usage

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
