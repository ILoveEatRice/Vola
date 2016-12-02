# Inversion of Control Container
## Introduction
Inversion of Control (IoC) is a design principle in which custom-written portions of a computer program receive the flow of control from a generic framework.

A software architecture with this design inverts control as compared to traditional procedural programming: in traditional programming, the custom code that expresses the purpose of the program calls into reusable libraries to take care of generic tasks, but with inversion of control, it is the framework that calls into the custom, or task-specific, code.

Inversion of control is used to increase modularity of the program and make it extensible.

## Getting Started
For simple IoC container allows you to configure which concrete class to be used for the required interface. 

First, you have to register it to the container.

> If the concrete class has more than one constructor, container will try all of them until it can resolve it. See [precise registration](#precise-registration) for more information.

```csharp
container.RegisterType<ICustomInterface, CustomConcreteClass>();
```

Then, when you need `ICustomInterface`, you call the container instead of initialize it yourself.

```csharp
var obj = container.Resolve<ICustomInterface>();
```

However, this is not the usage. As you apply dependency injection, the dependency can be deeply nested. But the container can resolve all of them if it has registered or it has a default value.

```csharp
// Before
var svc = new ShippingService(new ProductLocator(), new PricingService(), 
        new InventoryService(), new TrackingRepository(new ConfigProvider()), 
        new Logger(new EmailLogger(new ConfigProvider())));
// After
var svc = container<IShippingService>();
```

There is a the default container in *Vola*

```csharp
var container = Containers.Default();
```

It registers the following interfaces:
* IContainer
* IEventAggregator
* IEventHandlerFactory

## Precise Registration
Although IoC Container resolve the type for you, but you may want control on using which constructor or method, the value of parameters. By default, IoC Container try every constructors before it throws an exception. If the constructor needs parameters, it looks for the given parameters, tries to resolve it, or uses the default value.

There are several registration types. Different registration types allows you to initialize instances more flexible.

### Type
This is the most common way to register. Using `IContainer.RegisterType` to register a concrete type for the interface. To custom how to initialize, you need (constructor)[#constructor].

### Instance
`IContainer.RegisterInstance` allows you to use the same instance every time some objects ask for resolve.

### Name
Naming allows multiple registration to the same request type. And it can also resolve by name. Naming is supported in both type and instance registration.

```csharp
container.RegisterType<ICustomInterface, CustomConcreteClass>();
container.RegisterType<ICustomInterface, CustomConcreteClass2>("2");
var obj1 = container.Resolve<ICustomInterface>(); // CustomConcreteClass
var obj2 = container.Resolve<ICustomInterface>("2"); // CustomConcreteClass2
```

For more details, please read [constructor](constructor.md) and [parameter](parameter.md)