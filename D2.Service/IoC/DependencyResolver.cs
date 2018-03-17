using D2.Service.Controller;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Linq;
using System.Reflection;

namespace D2.Service.IoC
{
    public interface IServices
    {
        IServices AddSingleton<TInterface>(TInterface instance);
        IServices Add<TInterface, TImplementation>()
                  where TImplementation : TInterface;
        IServices AddMethod<TInterface>(Func<IServices, TInterface> factory);
        IServices AddControllers();
        T ResolveNamed<T>(string name);
        object ResolveNamed(Type clazz, string name);
        T Resolve<T>();
        object Resolve(Type clazz);
    }

    public class DependencyResolver : IServices
    {
        public DependencyResolver()
        {
            Kernel = new StandardKernel();
        }

        internal IKernel Kernel
        {
            get;
            private set;
        }

        public void RegisterApplicationComponents(Assembly bindingSource, params Assembly[] bindingSources)
        {
            var sources = new Assembly[] { bindingSource }.Concat(bindingSources);

            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .InheritedFrom(typeof(BaseController))
                      .WithoutAttribute<TopicAttribute>()
                      .BindBase()
                      .Configure((b, c) => {
                          var name = c.Name;
                          if (name.EndsWith("Controller")) name = name.Remove(name.Length - "Controller".Length);
                          b.Named(name);
                      })
            );

            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .InheritedFrom(typeof(BaseController))
                      .WithAttribute<TopicAttribute>()
                      .BindBase()
                      .Configure((b, c) => {
                          var name = c.GetCustomAttribute<TopicAttribute>().Topic;
                          b.Named(name);
                      })
            );


            // This is where our bindings are configurated
            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .WithAttribute<SingletonAttribute>()
                      .BindAllInterfaces()
                      .Configure(
                           b => b.InSingletonScope()
                       )
            );
            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .WithAttribute<RequestScopeAttribute>()
                      .BindAllInterfaces()
                      .Configure(
                           b => b.InScope(Scope.RequestScope)
                       )
            );
            Kernel.Bind(
                x => x.From(sources)
                      .SelectAllClasses()
                      .WithoutAttribute<TransientAttribute>()
                      .BindAllInterfaces()
            );
        }

        public T Resolve<T>()
        {
            return Kernel.Get<T>();
        }

        public T ResolveNamed<T>(string name)
        {
            return Kernel.Get<T>(name);
        }

        public object Resolve(Type clazz)
        {
            return Kernel.Get(clazz);
        }

        public object ResolveNamed(Type clazz, string name)
        {
            return Kernel.Get(clazz, name);
        }

        public IServices Add<TInterface, TImplementation>()
                         where TImplementation : TInterface
        {
            Kernel.Bind<TInterface>().To<TImplementation>();
            return this;
        }

        public IServices AddSingleton<TInterface>(TInterface instance)
        {
            Kernel.Bind<TInterface>().ToConstant(instance);
            return this;
        }

        public IServices AddSingleton<TInterface, TImplementation>()
                         where TImplementation : TInterface
        {
            Kernel.Bind<TInterface>()
                .To<TImplementation>()
                .InSingletonScope();
            return this;
        }

        public IServices AddMethod<TInterface>(Func<IServices, TInterface> factory)
        {
            Kernel.Bind<TInterface>().ToMethod(context => factory(this));
            return this;
        }

        public IServices AddControllers()
        {
            RegisterApplicationComponents(Assembly.GetCallingAssembly());
            return this;
        }
    }
}
