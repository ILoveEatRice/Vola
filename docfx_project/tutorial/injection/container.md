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
var svc = new ShippingService(new ProductLocator(), new PricingService(), new InventoryService(), 
   new TrackingRepository(new ConfigProvider()), new Logger(new EmailLogger(new ConfigProvider())));
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