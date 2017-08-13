using System;
using System.Reflection;

namespace Collector
{
    public class ReflectorProperty<T, V>
    {
        private readonly string name;
        private readonly Func<T, V> getter;
        private readonly Action<T, V> setter;
        private readonly Action<Substitute<T>, Func<V>> adder;

        public ReflectorProperty(PropertyInfo property)
        {
            this.name = property.Name;
            this.getter = (Func<T, V>)property.GetGetMethod().CreateDelegate(typeof(Func<T, V>));
            this.setter = (Action<T, V>)property.GetSetMethod().CreateDelegate(typeof(Action<T, V>));
            this.adder = (item, value) => item.Add(name, () => value());
        }

        public ReflectorProperty(string name, Func<T, V> getter, Action<T, V> setter, Action<Substitute<T>, Func<V>> adder)
        {
            this.name = name;
            this.getter = getter;
            this.setter = setter;
            this.adder = adder;
        }

        public string Name
        {
            get { return name; }
        }

        public bool IsNull(T item)
        {
            return getter.Invoke(item) == null;
        }

        public void SetNull(T item)
        {
            setter.Invoke(item, default(V));
        }

        public void SetNull(Substitute<T> item)
        {
            adder.Invoke(item, () => default(V));
        }

        public V GetValue(T item)
        {
            return getter.Invoke(item);
        }

        public void SetValue(T item, V value)
        {
            setter.Invoke(item, value);
        }

        public void SetValue(Substitute<T> item, Func<V> value)
        {
            adder.Invoke(item, value);
        }

        public ReflectorProperty<T, U> Cast<U>(Func<V, U> from, Func<U, V> to)
        {
            U toGetter(T instance) => from(getter(instance));
            void toSetter(T instance, U value) => setter(instance, to(value));
            void toAdder(Substitute<T> instance, Func<U> value) => adder(instance, () => to(value()));

            return new ReflectorProperty<T, U>(name, toGetter, toSetter, toAdder);
        }
    }
}