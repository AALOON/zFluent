# zFluent

The project which contains some fluent help functions
https://github.com/AALOON/zFluent

## Getting Started

https://www.nuget.org/packages/zFluent.Extensions/

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

## Authors

* **Albert Zabirov** - [AALOON](https://github.com/AALOON)

## License

[GNU GENERAL PUBLIC LICENSE](https://github.com/AALOON/zFluent/blob/master/LICENSE.txt)
