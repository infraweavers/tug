
using Xunit;
using Amazon.Lambda.TestUtilities;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Reflection;

namespace Tug.Server.FaaS.AwsLambda
{
    public class FunctionTest
    {
        // [Fact]
        // public void TetGetMethod()
        // {
        //     TestLambdaContext context;
        //     APIGatewayProxyRequest request;
        //     APIGatewayProxyResponse response;

        //     FunctionMain functions = new FunctionMain();

        //     request = new APIGatewayProxyRequest();
        //     context = new TestLambdaContext();

        //     response = functions.Get(request, context);

        //     Assert.Equal(200, response.StatusCode);
        //     Assert.Equal("Hello AWS Serverless", response.Body);
        // }

        [Fact]
        public async void TestGetVersion()
        {
            var f = new FunctionMain();
            var context = new TestLambdaContext();
            var request = GetRequest("/version");
            var response = await f.FunctionHandlerAsync(request, context);

            var version = typeof(FunctionMain).GetTypeInfo().Assembly.GetName().Version.ToString();
            var expected = $@"{{""version"":""{version}""}}";

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
        }

        [Fact]
        public async void TestRegisterDscAgent()
        {
            var f = new FunctionMain();
            var context = new TestLambdaContext();
            var request = GetRequest(sample:  "./SampleRequests/Put-RegisterDscAgent.json");
            var response = await f.FunctionHandlerAsync(request, context);

            var version = typeof(FunctionMain).GetTypeInfo().Assembly.GetName().Version.ToString();
            var expected = $@"{{""version"":""{version}""}}";

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(expected, response.Body);
        }
        public static APIGatewayProxyRequest GetRequest(string path = null, string method = null, string sample = "./SampleRequests/Get-Base.json")
        {
            if (path != null && !path.StartsWith("/"))
                throw new ArgumentException("path must start with '/'", nameof(path));

            var requestSer = File.ReadAllText(sample);
            var request = JsonConvert.DeserializeObject<APIGatewayProxyRequest>(requestSer);

            if (path != null)
            {
                request.Path = path;
            }

            if (method != null)
            {
                request.HttpMethod = method;
                request.RequestContext.HttpMethod = method;
            }
            return request;
        }
    }
}
