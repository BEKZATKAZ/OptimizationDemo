using GenerationDemo.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace GenerationDemo.Data.Pools
{
    public class MonoBehaviourPool<T> : IObjectPool<T> where T : MonoBehaviour
    {
        private Queue<T> _elements;

        public void Initialize(int amount, T prefab)
        {
            _elements = new Queue<T>(amount);

            for (int i = 0; i < amount; i++)
            {
                T chunk = Object.Instantiate(prefab);
                Return(chunk);
            }
        }

        public T Get()
        {
            T chunk = _elements.Dequeue();
            chunk.gameObject.SetActive(true);
            return chunk;
        }

        public void Return(T element)
        {
            _elements.Enqueue(element);
            element.gameObject.SetActive(false);
        }

        public void Dispose()
        {
            while (_elements.TryDequeue(out T chunk))
            {
                if (chunk) Object.Destroy(chunk.gameObject);
            }
        }
    }
}
