using System;
using System.Reflection;

namespace Collector
{
    public class ReflectorProperty<T, V>
    {
        private readonly Func<T, V> getter;
        private readonly Action<T, V> setter;

        public ReflectorProperty(PropertyInfo property)
        {
            this.getter = (Func<T, V>)Delegate.CreateDelegate(typeof(Func<T, V>), property.GetGetMethod());
            this.setter = (Action<T, V>)Delegate.CreateDelegate(typeof(Action<T, V>), property.GetSetMethod());
        }

        public ReflectorProperty(Func<T, V> getter, Action<T, V> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public bool IsNull(T item)
        {
            return getter.Invoke(item) == null;
        }

        public void SetNull(T item)
        {
            setter.Invoke(item, default(V));
        }

        public V GetValue(T item)
        {
            return getter.Invoke(item);
        }

        public void SetValue(T item, V value)
        {
            setter.Invoke(item, value);
        }

        public ReflectorProperty<T, U> Cast<U>(Func<V, U> from, Func<U, V> to)
        {
            U toGetter(T instance) => from(getter(instance));
            void toSetter(T instance, U value) => setter(instance, to(value));

            return new ReflectorProperty<T, U>(toGetter, toSetter);
        }
    }
}