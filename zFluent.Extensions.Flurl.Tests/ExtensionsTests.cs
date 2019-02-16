using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using Polly;
using Xunit;

namespace zFluent.Extensions.Flurl.Tests
{
    public class ExtensionsTests : IDisposable
    {
        private const int RetryCount = 2;
        private readonly HttpTest _httpTest;
        private readonly AsyncPolicy _policy;

        public ExtensionsTests()
        {
            _httpTest = new HttpTest();

            _policy = Policy
                .Handle<FlurlHttpException>()
                .Or<FlurlHttpTimeoutException>()
                .WaitAndRetryAsync(RetryCount, i => TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task SimpleTest_SucceedAsync()
        {
            // assign
            _httpTest.SimulateTimeout();
            _httpTest.RespondWith(JsonConvert.SerializeObject("Bad"), 503);
            _httpTest.RespondWith(JsonConvert.SerializeObject("Ok"), 200);

            var flurlClient = new FlurlClient("http://example120.com/");

            var response = await flurlClient.Request("api", "controller")
                .AllowHttpStatus(HttpStatusCode.OK)
                .WithRetryFuncAsync<string>(RetryFuncAsync)
                .RetryAsync(p => p.GetJsonAsync<string>());

            response.Should().Be("Ok");
            _httpTest.CallLog.Count.Should().Be(3);
        }

        [Fact]
        public async Task SimpleTest_ThrowsExceptionAsync()
        {
            // assign
            _httpTest.SimulateTimeout();
            _httpTest.RespondWith(JsonConvert.SerializeObject("Bad"), 503);
            _httpTest.RespondWith(JsonConvert.SerializeObject("Bad"), 503);

            var flurlClient = new FlurlClient("http://example120.com/");

            Func<Task> act = () =>
                flurlClient.Request("api", "controller")
                    .AllowHttpStatus(HttpStatusCode.OK)
                    .WithRetryFuncAsync<string>(RetryFuncAsync)
                    .RetryAsync(p => p.GetJsonAsync<string>());

            await act.Should().ThrowAsync<FlurlHttpException>();

            _httpTest.CallLog.Count.Should().Be(3);
        }

        [Fact]
        public async Task SimpleActionTest_SucceedAsync()
        {
            // assign
            _httpTest.SimulateTimeout();
            _httpTest.RespondWith(JsonConvert.SerializeObject("Bad"), 503);
            _httpTest.RespondWith(JsonConvert.SerializeObject("Ok"), 200);

            var flurlClient = new FlurlClient("http://example120.com/");

            await flurlClient.Request("api", "controller")
                .AllowHttpStatus(HttpStatusCode.OK)
                .WithRetryFuncAsync(RetryActionAsync)
                .RetryAsync(p => p
                    .PostUrlEncodedAsync(JsonConvert.SerializeObject("data")));
            
            _httpTest.CallLog.Count.Should().Be(3);
        }

        [Fact]
        public async Task SimpleActionTest_ThrowsExceptionAsync()
        {
            // assign
            _httpTest.SimulateTimeout();
            _httpTest.RespondWith(JsonConvert.SerializeObject("Bad"), 503);
            _httpTest.RespondWith(JsonConvert.SerializeObject("Bad"), 503);

            var flurlClient = new FlurlClient("http://example120.com/");

            Func<Task> act = () =>
                flurlClient.Request("api", "controller")
                    .AllowHttpStatus(HttpStatusCode.OK)
                    .WithRetryFuncAsync(RetryActionAsync)
                    .RetryAsync(p => p
                        .PostUrlEncodedAsync(JsonConvert.SerializeObject("data"))); 

            await act.Should().ThrowAsync<FlurlHttpException>();

            _httpTest.CallLog.Count.Should().Be(3);
        }

        private async Task<string> RetryFuncAsync(IFlurlRequest request, Func<IFlurlRequest, Task<string>> func)
        {
            return await _policy.ExecuteAsync(() => func(request));
        }

        private async Task RetryActionAsync(IFlurlRequest request, Func<IFlurlRequest, Task> func)
        {
            await _policy.ExecuteAsync(() => func(request));
        }

        public void Dispose()
        {
            _httpTest?.Dispose();
        }
    }
}
