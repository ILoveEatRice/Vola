# Constructor
`IConstructor` allows you to select which constructor to use in `IContainer`. To select constructor, you need `IConstructor`. It is recommend to use `Constructors` to initialize them.

## Instance
This is exactly the same to `IContainer.RegisterInstance`. Please use that instead.

## Injection
This is used for selecting which constructor to use. You need `ConstructorInfo` to achieve that. If you do not know to get `ConstructorInfo`, you can use `MethodUtils.GetConstructor<T>` to help.

```csharp
// Gets the constructor of CustomClass which is CustomClass(int, string)
var constructor = MethodUtils.GetConstructor<CustomClass>(typeOf(int), typeOf(string));
var iConstructor = Constructors.Injection(constructor);
container.RegisterType<ICustomInterface>(iConstructor);
```

## Type
This is exactly the same to `IContainer.RegisterType`. Please use that instead.

## Factory
If your class can only created through factory, you should use this instead. You need to get the `MethodInfo` to achieve that. If you do not know to get `MethodInfo`, you can use `MethodUtils.GetMethodInfo` to help.

```csharp
var factoryMethod = MethodUtils.GetMethodInfo(() => factory.Create());
var iConstructor = Constructors.Factory(factoryMethod, factory);
container.RegisterType<ICustomInterface>(iConstructor);
```

## Static Factory
If you use factory method instead of factory,you can use this. You need to get the `MethodInfo` to achieve that. If you do not know to get `MethodInfo`, you can use `MethodUtils.GetMethodInfo` to help.

```csharp
var factoryMethod = MethodUtils.GetMethodInfo(() => Factory.Create());
var iConstructor = Constructors.StaticFactory(factoryMethod);
container.RegisterType<ICustomInterface>(iConstructor);
```