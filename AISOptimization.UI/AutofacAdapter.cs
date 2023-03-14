using System;

using Autofac;


namespace AISOptimization;

public class AutofacAdapter : IServiceProvider
{
    public IContainer Container { get; set; }

    public object? GetService(Type serviceType)
    {
        return Container.Resolve(serviceType);
    }
}


