using System;
using AvalonAssets.DataStructure.Heap;

namespace AvalonAssets.DataStructure.Queue
{
    /// <summary>
    ///     Implementation of priority queue with <see cref="IHeap{T}" />.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueue<T>
    {
        private readonly IHeap<IPriority<T>> _heap;

        /// <summary>
        ///     Create a <see cref="PriorityQueue{T}" /> with given <see cref="IHeap{T}" />.
        ///     If no <see cref="IHeap{T}" /> is given, <see cref="BinaryHeap{T}" /> and <see cref="PriorityComparer{T}" /> will be
        ///     used.
        /// </summary>
        /// <param name="heap">Initial Heap.</param>
        public PriorityQueue(IHeap<IPriority<T>> heap = null)
        {
            if (heap == null)
                heap = Heaps.Default(new PriorityComparer<T>());
            _heap = heap;
        }

        /// <summary>
        ///     Returns is the queue empty.
        /// </summary>
        /// <returns>Is queue empty.</returns>
        public bool IsEmpty()
        {
            return _heap.IsEmpty();
        }

        /// <summary>
        ///     Removes and returns the object at the beginning of the queue.
        /// </summary>
        /// <returns>Object at the beginning of the queue.</returns>
        public T Dequeue()
        {
            if (_heap.IsEmpty())
                throw new InvalidOperationException("PriorityObject Queue is empty");
            return _heap.ExtractMin().Value.Object;
        }

        /// <summary>
        ///     Returns the object at the beginning of the queue without removing it.
        /// </summary>
        /// <returns>Object at the beginning of the queue.</returns>
        public T Peek()
        {
            if (_heap.IsEmpty())
                throw new InvalidOperationException("PriorityObject Queue is empty");
            return _heap.GetMin().Value.Object;
        }

        /// <summary>
        ///     Adds an prioritized object to the end of the queue.
        /// </summary>
        /// <param name="prioritizedObject">Prioritized object.</param>
        public void Enqueue(IPriority<T> prioritizedObject)
        {
            _heap.Insert(prioritizedObject);
        }

        /// <summary>
        ///     Adds an object to the end of the queue with given priority.
        /// </summary>
        /// <param name="priority">Priority of the object.</param>
        /// <param name="object">Object</param>
        public void Enqueue(int priority, T @object)
        {
            _heap.Insert(new PriorityObject<T>(priority, @object));
        }
    }
}