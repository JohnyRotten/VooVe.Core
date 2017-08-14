using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VooVe.Core
{
    public class IoC
    {

        private delegate object ResolveDelegate(TypeStack stack = null);

        private readonly Dictionary<Type, ResolveDelegate> _container = new Dictionary<Type, ResolveDelegate>();

        public void Bind<TInterface, TImplementation>() where TImplementation : TInterface where TInterface : class
        {
            _container[typeof(TInterface)] = stack => ConstructImplementation(typeof(TImplementation), stack);
        }

        public void BindInstance<TInterface, TImplementation>() where TImplementation : TInterface where TInterface : class
        {
            var impl = (TInterface)ConstructImplementation(typeof(TImplementation));
            _container[typeof(TInterface)] = _ => impl;
        }

        public void BindSingleton<TInterface, TImplementation>() where TImplementation : TInterface where TInterface : class
        {
            TInterface impl = null;
            _container[typeof(TInterface)] = _ => impl ?? (impl = (TInterface)ConstructImplementation(typeof(TImplementation)));
        }

        private object ConstructImplementation(Type type, TypeStack stack = null)
        {
            stack = stack ?? new TypeStack(type);
            foreach (var contructor in type.GetConstructors())
            {
                try
                {
                    var parameters = contructor
                        .GetParameters()
                        .Select(p => ResolveType(p.ParameterType, stack.Push(p.ParameterType)))
                        .ToArray();
                    return contructor.Invoke(parameters);
                }
                catch (TargetInvocationException e) when (e.InnerException is CyclingTypingException)
                {
                    throw new CyclingTypingException(type, e.InnerException);
                }
                catch (Exception e) when(!(e is CyclingTypingException))
                {
                }
            }
            throw new UnconstructedException(type);
        }

        public void Bind<T>() where T : class => Bind<T, T>();

        public T Resolve<T>() => (T)_container[typeof(T)]();

        public void BindInstance<TInterface, TImplementation>(TImplementation impl) where TImplementation : TInterface where TInterface : class
        {
            _container[typeof(TInterface)] = _ => impl;
        }

        public void Bind<TInterface, TImplementation>(Func<TImplementation> getter) where TImplementation : TInterface where TInterface : class
        {
            _container[typeof(TInterface)] = _ => getter();
        }

        public void Bind<T>(Func<T> getter) where T : class => Bind<T, T>(getter);

        public void BindInstance<T>(T impl) where T : class => BindInstance<T, T>(impl);

        private T ResolveWithStack<T>(TypeStack stack) => (T) _container[typeof(T)](stack);
        
        private object ResolveType(Type type, TypeStack stack)
        {
            var method = typeof(IoC).GetMethod(nameof(ResolveWithStack), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(type);
            return method.Invoke(this, new object[] { stack });
        }

        protected class TypeStack
        {
            private readonly HashSet<Type> _types = new HashSet<Type>();

            public TypeStack(Type type)
            {
                _types.Add(type);
            }

            public TypeStack Push(Type type)
            {
                if (_types.Contains(type))
                {
                    throw new CyclingTypingException(type);
                }
                _types.Add(type);
                return this;
            }
        }

    }

    public class CyclingTypingException : Exception
    {
        private readonly Type _type;

        public CyclingTypingException(Type type, Exception exception = null) : base("", exception)
        {
            _type = type;
        }

        public override string Message => $"Cycling reference on '{_type.FullName}'";

    }

    public class UnconstructedException : Exception
    {
        private readonly Type _type;

        public UnconstructedException(Type type)
        {
            _type = type;
        }

        public override string Message => $"Type '{_type.FullName}' unconstructed.";

    }
}
