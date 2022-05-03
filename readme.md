# B2CAuth

Netstandard library, to generate Azure B2C access token.

## Frameworks

This package is built in **netstandard 2.0**

Currently supports below versions:


| .NET implementation		| Version support					|
| :-------------------------- | :-------------------------------------- |
| .NET and .NET Core		| 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0		|
| .NET Framework 			| 4.6.1 2, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8	|
| Mono				| 5.4, 6.4						|
| Xamarin.iOS			| 10.14, 12.16					|
| Xamarin.Mac			| 3.8, 5.16						|
| Xamarin.Android			| 8.0, 10.0						|
| Universal Windows Platform	| 10.0.16299, TBD					|
| Unity				| 2018.1						|

## Implementation

Implementation can be done in .net core and .net framework both as mentioned above, Please refer below examples

### 1. NetCore 3.1

- Install the nuget package: **B2CAuth**

- Add below settings to appsettings.json

``` C#
"AzureB2CSettings": {
    "TokenUrl": "############",
    "GrantType": "password",
    "ClientId": "##########",
    "ResponseType": "token id_token",
    "Scope": "##########",
    "LoginId": "prashant.kumar@####.com",
    "Password": "#########"
  }
```

- Copy below line in startup class:

``` C#
services.AddB2CTokenGenerator(Configuration);
``` 

- Use Constructor Injection for ITokenGenerator

``` C#
	  private readonly ITokenGenerator tokenGenerator;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ITokenGenerator tokenGenerator)
        {
            _logger = logger;
            this.tokenGenerator = tokenGenerator;
        }
```

- Call token generator method

``` C#
var result = await tokenGenerator.GetTokenAsync();
```


### 2. .Net 4.7.2

- Add appsettings.json file to root folder.

- Right click on appsettings.json file and Set **Copy to Output Directory** to **Copy Always**

- Use Autofac for DI and create DI

``` C#
		var builder = new ContainerBuilder();
            var assembly = typeof(Program).Assembly;
```

- Bind B2C Settings into AzureB2CSettings object

``` C#
		IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
            var sfConfig = configuration.GetSection("AzureB2CSettings");
            AzureB2CSettings azureB2CSettings = new AzureB2CSettings();
            sfConfig.Bind(azureB2CSettings);
```

- Bind Http Client factory

``` C#
var serviceProvider = new ServiceCollection().AddHttpClient().BuildServiceProvider();
```

- Make sure ILogger is already registered in Autofac 

- Register all 3 in DI

``` C#
		builder.RegisterInstance(serviceProvider.GetService<IHttpClientFactory>());
            builder.RegisterInstance(Options.Create<AzureB2CSettings>(azureB2CSettings));
            builder.RegisterInstance(logger);
```

- Get Values of IHttpClientFactory, IOptions<AzureB2CSettings> and ILogger from constructor injection.

- Create an instance of Token generator and call token generator method

``` C#
		ITokenGenerator obj = new TokenGenerator(som, log, _httpClientFactory);
            var res = await obj.GetTokenAsync();
```

- OR, Register TokenGenerator as ITokenGenerator and use object from constructor injection.

``` C#
builder.RegisterType<TokenGenerator>.As<ITokenGenerator>();
```

## License
Neighborly®

## Changelog
- 29-03-2022 Added initial commit


