using System;
using System.Reflection;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;


namespace ZJB.Core.Injection
{
    public class Container
    {
        private IUnityContainer unityContainer;

        private static Container container = new Container();

        private Container()
        {
            unityContainer = new UnityContainer();
           unityContainer.AddNewExtension<Interception>();
        }

        public static Container Instance
        {
            get { return container; }
        }

        public void RegisterType<T>()
        {
            unityContainer.RegisterType<T>();
        }

        public void RegisterType<T>(LifetimeManager lifetimeManager)
        {
            unityContainer.RegisterType<T>(lifetimeManager);
        }


        public void RegisterType<TInterface, TImplement>()
        {
            unityContainer.RegisterType(typeof(TInterface), typeof(TImplement));
        }

        public void RegisterType<TInterface, TImplement>(string name)
        {
            unityContainer.RegisterType(typeof(TInterface), typeof(TImplement), name);
        }


        public void RegisterInstance<T>(T instance) where T : class
        {
            unityContainer.RegisterInstance(typeof(T), instance);
        }

        public void RegisterInstance<T>(T instance, string name) where T : class
        {
            unityContainer.RegisterInstance(typeof(T), name, instance);
        }

        public T Resolve<T>()
        {
            return unityContainer.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return unityContainer.Resolve(type);
        }

        public bool IsRegistered<T>(string name)
        {
            return unityContainer.IsRegistered<T>(name);
        }

        public T Resolve<T>(string name)
        {
            return unityContainer.Resolve<T>(name);
        }

        public void AddInterception<T>()
        {
            unityContainer.Configure<Interception>()
                .SetInterceptorFor<T>(new VirtualMethodInterceptor());
        }

        public void RegisterAssemblyTypes(params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
                RegisterAssemblyTypeWithPerformance(assembly);
            }
        }


        private InjectionMember[] GetInjectionMembers(IInterceptionBehavior[] interceptors)
        {
            InjectionMember[] injectionMember = null;

            if (interceptors != null && interceptors.Length > 0)
            {
                injectionMember = new InjectionMember[interceptors.Length + 1];
                injectionMember[0] = new Interceptor<VirtualMethodInterceptor>();

                for (int i = 0; i < interceptors.Length; i++)
                {
                    injectionMember[i+1] = new InterceptionBehavior(interceptors[i]);
                }
            }

            return injectionMember;
        }


        public void RegisterAssemblyTypeWithPerformance(Assembly assembly)
        {
            RegisterAssemblyType(assembly, new PerformanceInterceptor());
        }

        public void RegisterAssemblyType(Assembly assembly, params IInterceptionBehavior[] interceptors)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && type.IsVisible && !type.IsAbstract)
                {
                    unityContainer.RegisterType(type, new ContainerControlledLifetimeManager(), GetInjectionMembers(interceptors));
                    unityContainer.Configure<Interception>()
                        .SetInterceptorFor(type, new VirtualMethodInterceptor());
                }
            }
        }

        public void RegisterAssemblyTypeWithPerformance(Assembly assembly, Type interfaceType)
        {
            RegisterAssemblyType(assembly,interfaceType, new PerformanceInterceptor());
        }

        public void RegisterAssemblyType(Assembly assembly, Type interfaceType, params IInterceptionBehavior[] interceptors)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && type.IsVisible && !type.IsAbstract && (interfaceType == null || interfaceType.IsAssignableFrom(type)))
                {
                    unityContainer.Configure<Interception>()
                        .SetInterceptorFor(type, new VirtualMethodInterceptor());
                    unityContainer.RegisterType(type, GetInjectionMembers(interceptors));
                }
            }
        }
    }
}
