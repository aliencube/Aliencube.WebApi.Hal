# Aliencube.WebApi.Hal #

## THIS IS NO LONGER DEVELOPED NOR MANAGED ##

----

**Aliencube.WebApi.Hal** provides REST responses with [HAL specification](https://tools.ietf.org/html/draft-kelly-json-hal) in Web API apps.


## Package Status ##

| NuGet Package | `master` | `dev` |
|:-------------:|:--------:|:-----:|
| [![](https://img.shields.io/nuget/v/Aliencube.WebApi.Hal.svg)](https://www.nuget.org/packages/Aliencube.WebApi.Hal/) [![](https://img.shields.io/nuget/dt/Aliencube.WebApi.Hal.svg)](https://www.nuget.org/packages/Aliencube.WebApi.Hal/) | [![Build status](https://ci.appveyor.com/api/projects/status/0ea0abx3i71m4y03/branch/master?svg=true)](https://ci.appveyor.com/project/justinyoo/aliencube-webapi-hal/branch/master) | [![Build status](https://ci.appveyor.com/api/projects/status/0ea0abx3i71m4y03/branch/dev?svg=true)](https://ci.appveyor.com/project/justinyoo/aliencube-webapi-hal/branch/dev) |


## Getting Started ##

HAL basically provides metadata with REST responses. It works differently in JSON response and XML response. In order to apply HAL specifiation to your REST response with **Aliencube.WebApi.Hal**, follow the steps below.


### Configure Web API ###

In order to run a Web API app, either `Startup.cs` or `Global.asax.cs`, the `WebApiConfig.Configure(HttpConfiguration)` method should be called like:

```csharp
// OWIN pipeline
public class Startup
{
  public void Configuration(IAppBuilder appBuilder)
  {
    ...
  
    WebApiConfig.Configure(GlobalConfiguration.Configuration);

    ...
  }
}

// ASP.NET pipeline
public class WebApiApplication : System.Web.HttpApplication
{
  protected void Application_Start(object sender, EventArgs e)
  {
    WebApiConfig.Configure(GlobalConfiguration.Configuration);
  }
}
```

Hence, in the `WebApiConfig.Configure()` method, just call the extension method, `ConfigHalFormatter()` for setup.

```csharp
public static class WebApiConfig
{
  public static void Configure(HttpConfiguration config)
  {
    ...

    config.ConfigHalFormatter();

    ...
  }
}
```

If you need more control over the `ConfigHallFormatter()` method, you can do like:

```csharp
var settings = new JsonSerializerSettings()
                   {
                       ContractResolver = new CamelCasePropertyNamesContractResolver(),
                       MissingMemberHandling = MissingMemberHandling.Ignore,
                   };

var jsonFormatter = new HalJsonMediaTypeFormatter()
                        {
                            SerializerSettings = settings,
                        };

var xmlFormatter = new HalXmlMediaTypeFormatter()
                       {
                           Namespace = "http://schema.aliencube.org/xml/2015/08/hal",
                       };

config.Formatters.Remove(config.Formatters.JsonFormatter);
config.Formatters.Insert(0, jsonFormatter);
config.Formatters.Insert(1, xmlFormatter);
```

### Implement `LinkedResource` or `LinkedResourceCollection` ###

If the response of your API contains a single resource, you can implement `LinkedResource` for your model like:

```csharp
public class Product : LinkedResource
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
```

If the response is in charge of a collection of resources, you can implement `LinkedResourceCollection` for your model like:

```charp
public class Products : LinkedResourceCollection<Product>
{
  public Products() : base()
  {
  }

  public Products(List<Product> items) : base(items)
  {
  }
}
```


### Implement API Controllers ###

This is a basic example to set HAL metadata. This can be achieved in a different way by developer experiences not being disturbed from manual insertions like below:

```csharp
[RoutePrefix("product")]
public class ProductController : ApiController
{
  ...
  
  [Route("{productId}")]
  public Product Get(int productId)
  {
    var product = GetProduct(productId);
	
	// Sets the HAL metadata. This can be done in a different way.
	product.Href = string.Format("/product/{0}", productId);
	
	product.AddLink(new Link() { Rel = "collection", Href = "/products" });
	product.AddLink(new Link() { Rel = "template", Href = "/product/{productId}" });

    return product;
  }
  
  ...
}

[RoutePrefix("products")]
public class ProductsController : ApiController
{
  ...
  
  [Route("")]
  public Products Get()
  {
    var products = GetProducts();
	
	// Sets the HAL metadata. This can be done in a different way.
	products.Href = "/products";
	
	products.AddLink(new Link() { Rel = "next", Href = "/products?p=2" });
	products.AddLink(new Link() { Rel = "template", Href = "/product/{productId}" });

    return products;
  }
  
  ...
}
```


### Results ###

After the development, run the API app and send a request like, using Fiddler or Postman:

```
GET http://localhost/product/1
```

Depending on the request header, `application/json` OR `text/json` OR `application/hal+json`, the result returned looks like:

```json
{
  "_links": {
    "self": {
      "href": "/product/1"
    },
    "collection": {
      "href": "/products"
    },
    "templated": {
      "href": "/product/{productId}",
      "templated": true
    }
  },
  "productId": 1,
  "name": "ABC",
  "description": "Product ABC"
}
```

If the request header is `application/xml` OR `text/xml` OR `application/hal+xml`, the result returned looks like:

```xml
<?xml version="1.0" encoding="utf-8"?>
<resource xmlns="http://schema.aliencube.org/xml/2015/08/hal">
  <links>
    <link>
      <rel>self</rel>
      <href>/product/1</href>
    </link>
    <link>
      <rel>collection</rel>
      <href>/products</href>
    </link>
    <link>
      <rel>template</rel>
      <href>/product/{productId}</href>
      <templated>true</templated>
    </link>
  </links>
  <productId>1</productId>
  <name>ABC</name>
  <description>Product ABC</description>
</resource>
```

Now, let's send another request returning a collection of results.

```
GET http://localhost/products
```

Depending on the request header, `application/json` OR `text/json` OR `application/hal+json`, the result returned looks like:

```json
{
  "_links": {
    "self": {
      "href": "/products"
    },
    "next": {
      "href": "/products?p=2"
    },
    "templated": {
      "href": "/product/{productId}",
      "templated": true
    }
  },
  "_embedded": [
    {
      "_links": {
        "self": {
          "href": "/product/1"
        },
        "collection": {
          "href": "/products"
        },
        "template": {
          "href": "/product/{productId}",
          "templated": true
        }
      },
      "productId": 1,
      "name": "ABC",
      "description": "Product ABC"
    },
    {
      "_links": {
        "self": {
          "href": "/product/2"
        },
        "collection": {
          "href": "/products"
        },
        "template": {
          "href": "/product/{productId}",
          "templated": true
        }
      },
      "productId": 2,
      "name": "XYZ",
      "description": "Product XYZ"
    }
  ]
}
```

If the request header is `application/xml` OR `text/xml` OR `application/hal+xml`, the result returned looks like:

```xml
<?xml version="1.0" encoding="utf-8"?>
<resource xmlns="http://schema.aliencube.org/xml/2015/08/hal">
  <links>
    <link>
      <rel>self</rel>
      <href>/products</href>
    </link>
    <link>
      <rel>next</rel>
      <href>/products?p=2</href>
    </link>
    <link>
      <rel>templated</rel>
      <href>/product/{productId}</href>
    </link>
  </links>
  <resources>
    <resource>
      <links>
        <link>
          <rel>self</rel>
          <href>/product/1</href>
        </link>
        <link>
          <rel>collection</rel>
          <href>/products</href>
        </link>
        <link>
          <rel>template</rel>
          <href>/product/{productId}</href>
          <templated>true</templated>
        </link>
      </links>
      <productId>1</productId>
      <name>ABC</name>
      <description>Product ABC</description>
    </resource>
    <resource>
      <links>
        <link>
          <rel>self</rel>
          <href>/product/2</href>
        </link>
        <link>
          <rel>collection</rel>
          <href>/products</href>
        </link>
        <link>
          <rel>template</rel>
          <href>/product/{productId}</href>
          <templated>true</templated>
        </link>
      </links>
      <productId>2</productId>
      <name>XYZ</name>
      <description>Product XYZ</description>
    </resource>
  </resources>
</resource>
```

## Contribution ##

Your contributions are always welcome! All your work should be done in your forked repository. Once you finish your work, please send us a pull request onto our `dev` branch for review.


## License ##

**Aliencube.WebApi.Hal** is released under [MIT License](http://opensource.org/licenses/MIT)

> The MIT License (MIT)
>
> Copyright (c) 2015 [aliencube.org](http://aliencube.org)
> 
> Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
> 
> The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
> 
> THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
