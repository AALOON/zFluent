using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace zFluent.Extensions.Tests
{
    public class ExtensionsTests
    {
        private int RetryCount { get; set; }

        [Fact]
        public void SimpleTest_ThrowsException()
        {
            // assign
            var httpClient = new HttpClient();
            RetryCount = 3;

            // act, assert
            httpClient
                .Invoking(client => client
                    .WithRetryFunc<HttpClient, HttpResponseMessage>(RetryIfException)
                    .Retry(c => c.GetAsync("http://example120.com/").Result))
                .Should().Throw<Exception>();
            RetryCount.Should().Be(0);
        }

        [Fact]
        public void SimpleTest_Succeed()
        {
            // assign
            var httpClient = new HttpClient();
            RetryCount = 3;

            // act
            var response = httpClient
                .WithRetryFunc<HttpClient, HttpResponseMessage>(RetryIfException)
                .Retry(c => c.GetAsync("http://example.com/").Result);

            // assert
            RetryCount.Should().Be(3);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private HttpResponseMessage RetryIfException(HttpClient client,
            Func<HttpClient, HttpResponseMessage> func)
        {
            while (true)
            {
                try
                {
                    return func(client);
                }
                catch (Exception e) when (RetryCount > 0)
                {
                    Console.WriteLine($"{e.Message}. Retry left: {RetryCount}");
                    RetryCount--;
                }
            }
        }

        [Fact]
        public async Task SimpleTest_ThrowsExceptionAsync()
        {
            // assign
            var httpClient = new HttpClient();
            RetryCount = 3;

            // act, assert
            Func<Task> act = () =>
                httpClient
                    .WithRetryFuncAsync<HttpClient, HttpResponseMessage>(RetryIfExceptionAsync)
                    .RetryAsync(c => c.GetAsync("http://example120.com/"));

            await act.Should().ThrowAsync<Exception>();
            RetryCount.Should().Be(0);
        }

        [Fact]
        public async Task SimpleTest_SucceedAsync()
        {
            // assign
            var httpClient = new HttpClient();
            RetryCount = 3;

            // act
            var response = await httpClient
                .WithRetryFuncAsync<HttpClient, HttpResponseMessage>(RetryIfExceptionAsync)
                .RetryAsync(c => c.GetAsync("http://example.com/"));

            // assert
            RetryCount.Should().Be(3);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task<HttpResponseMessage> RetryIfExceptionAsync(HttpClient client,
            Func<HttpClient, Task<HttpResponseMessage>> func)
        {
            while (true)
            {
                try
                {
                    return await func(client);
                }
                catch (Exception e) when (RetryCount > 0)
                {
                    Console.WriteLine($"{e.Message}. Retry left: {RetryCount}");
                    RetryCount--;
                }
            }
        }
    }
}
