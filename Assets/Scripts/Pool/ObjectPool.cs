using System.Collections.Generic;
using UnityEngine;

namespace MyObjectPool
{
    /// <summary>
    /// Generic object pool for GameObjects.
    /// </summary>
    /// <typeparam name="T">Type of the pooled items.</typeparam>
    public class ObjectPool<T>
    {
        private readonly GameObject _poolItemPref;
        private readonly Transform _poolHolder;
        private readonly int _poolStartSize;
        private readonly Queue<T> _pool;

        /// <summary>
        /// Initializes a new instance of the ObjectPool class.
        /// </summary>
        /// <param name="itemPref">The prefab of the object to be pooled.</param>
        /// <param name="poolHolder">The transform to serve as the parent for the pooled objects.</param>
        /// <param name="startSize">The initial size of the object pool.</param>
        public ObjectPool(GameObject itemPref, Transform poolHolder, int startSize)
        {
            _poolItemPref = itemPref;
            _poolHolder = poolHolder;
            _poolStartSize = startSize;
            _pool = new Queue<T>();
            Fill();
        }

        /// <summary>
        /// Fills the object pool to reach the specified start size.
        /// </summary>
        private void Fill()
        {
            for (int i = 0; i < _poolStartSize; i++)
            {
                SpawnNewItem();
            }
        }

        /// <summary>
        /// Spawns a new item from the prefab and adds it to the object pool.
        /// </summary>
        private void SpawnNewItem()
        {
            var item = Object.Instantiate(_poolItemPref, _poolHolder);
            _pool.Enqueue(item.GetComponent<T>());
            item.SetActive(false);
        }

        /// <summary>
        /// You are responsiple for deactivating this item
        /// </summary>
        /// <param name="item">the item needs returning</param>
        public void ReturnToPool(T item)
        {
            _pool.Enqueue(item);
        }

        /// <summary>
        /// Retrieves an item from the object pool, creating a new one if the pool is empty.
        /// The instance is not active by default
        /// </summary>
        /// <returns>The retrieved item from the pool.</returns>
        public T GetAnInstance()
        {
            if (_pool.Count == 0)
                SpawnNewItem();
            T item = _pool.Dequeue();
            return item;
        }
    }
}