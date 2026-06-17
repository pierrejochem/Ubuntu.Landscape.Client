using System;
using Ubuntu.Landscape;
using Ubuntu.Landscape.Exceptions;
using Xunit;

namespace Ubuntu.Landscape.Tests
{
    public class ClientTests
    {
        private static Client NewValidClient()
        {
            return new Client
            {
                hostname = "landscape.example.com",
                accessKey = "ACCESS123",
                secretKey = "SECRET456",
                actionString = "GetAlerts"
            };
        }

        // ---- defaults ----

        [Fact]
        public void Defaults_SslEnabledTrue_IgnoreCertsFalse()
        {
            var client = new Client();
            Assert.True(client.sslEnabled);
            Assert.False(client.ignoreInvalidCerts);
        }

        [Fact]
        public void Defaults_PropertiesAreEmptyStrings()
        {
            var client = new Client();
            Assert.Equal("", client.hostname);
            Assert.Equal("", client.accessKey);
            Assert.Equal("", client.secretKey);
            Assert.Equal("", client.actionString);
        }

        // ---- requestUrl structure ----

        [Fact]
        public void RequestUrl_StartsWithAccessKeyId()
        {
            var url = NewValidClient().requestUrl;
            Assert.StartsWith("access_key_id=ACCESS123", url);
        }

        [Theory]
        [InlineData("action=GetAlerts")]
        [InlineData("signature_method=HmacSHA256")]
        [InlineData("signature_version=2")]
        [InlineData("version=2011-08-01")]
        [InlineData("timestamp=")]
        [InlineData("signature=")]
        public void RequestUrl_ContainsExpectedParam(string fragment)
        {
            var url = NewValidClient().requestUrl;
            Assert.Contains(fragment, url);
        }

        [Fact]
        public void RequestUrl_DifferentSecretKey_ProducesDifferentSignature()
        {
            var a = NewValidClient();
            var b = NewValidClient();
            b.secretKey = "DIFFERENT";

            Assert.NotEqual(Signature(a.requestUrl), Signature(b.requestUrl));
        }

        private static string Signature(string url)
        {
            const string marker = "&signature=";
            var i = url.IndexOf(marker, StringComparison.Ordinal);
            Assert.True(i >= 0, "url missing signature param");
            return url.Substring(i + marker.Length);
        }

        // ---- input validation (throws before any network call) ----

        [Fact]
        public void GetResult_MissingHostname_Throws()
        {
            var client = NewValidClient();
            client.hostname = "";
            var ex = Assert.Throws<InputException>(() => client.getResult());
            Assert.Equal("Missing host name!", ex.Message);
        }

        [Fact]
        public void GetResult_MissingAccessKey_Throws()
        {
            var client = NewValidClient();
            client.accessKey = "   ";
            var ex = Assert.Throws<InputException>(() => client.getResult());
            Assert.Equal("Missing access key!", ex.Message);
        }

        [Fact]
        public void GetResult_MissingSecretKey_Throws()
        {
            var client = NewValidClient();
            client.secretKey = "";
            var ex = Assert.Throws<InputException>(() => client.getResult());
            Assert.Equal("Missing secret key!", ex.Message);
        }

        [Fact]
        public void GetResult_MissingActionString_Throws()
        {
            var client = NewValidClient();
            client.actionString = "";
            var ex = Assert.Throws<InputException>(() => client.getResult());
            Assert.Equal("Missing action string!", ex.Message);
        }
    }
}
