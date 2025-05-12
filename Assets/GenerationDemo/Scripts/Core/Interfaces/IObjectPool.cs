using System;

namespace GenerationDemo.Core.Interfaces
{
    public interface IObjectPool<T> : IDisposable
    {
        T Get();
        void Return(T element);
    }
}
