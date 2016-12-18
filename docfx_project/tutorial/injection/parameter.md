# Parameter
To give exact parameter for the constructor, there are two ways to do it. First, it is the simple way. You create a `Dictionary<string, object>` and add parameter by using the parameter name as key. Then, pass the `Dictionary<string, object>` to `IContainer.Resolve<CustomClass>`. 

> To use default value, you can use Type.Missing

```csharp
var parameters = new Dictionary<string, object>();
parameters.Add("num", 1);
// Constructor must have a parameter call num to take effect
var obj = container.Resolve<ICustomInterface>(parameters);
```

Another way is to use `IParameter`. To create `IParameter`, it is recommend to use `Parameters`. `Parameters` provides `Resolve` and `Value`.

## Resolve Parameter
Resolve parameter tells the container how to resolve it. You can also using naming for the parameter.

```csharp
var parameter = Parameters.Resolve<CustomClass>("Field");
// Using naming type instead of default
var parameter2 = Parameters.Resolve<CustomClass>("Field", "ResolveName");
var obj = container.Resolve<ICustomInterface>(parameter, parameter2);
```

## Value Parameter
Value parameter is simple. Just given it the field name and the value.

```csharp
var parameter = Parameters.Value(1, "Field");
var obj = container.Resolve<ICustomInterface>(parameter);
```