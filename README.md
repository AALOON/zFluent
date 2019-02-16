# zFluent

The project which contains some fluent help functions
https://github.com/AALOON/zFluent

## Getting Started

https://www.nuget.org/packages/zFluent.Extensions/
```
Install-Package zFluent.Extensions -Version 1.0.2
```
https://www.nuget.org/packages/zFluent.Extensions.Flurl/
```
Install-Package zFluent.Extensions.Flurl -Version 1.0.0
```

### Example

Simple example how to use extensions
```
public void SimpleExample()
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
```

```
public class FlurlTests : IDisposable
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

    private async Task<string> RetryFuncAsync(IFlurlRequest request, Func<IFlurlRequest, Task<string>> func)
    {
        return await _policy.ExecuteAsync(() => func(request));
    }

    public void Dispose()
    {
        _httpTest?.Dispose();
    }
}
```

## Authors

* **Albert Zabirov** - [AALOON](https://github.com/AALOON)

## License

[MIT License](https://github.com/AALOON/zFluent/blob/master/LICENSE.txt)
