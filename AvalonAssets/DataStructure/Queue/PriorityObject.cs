namespace AvalonAssets.DataStructure.Queue
{
    /// <summary>
    ///     Simple implementation of <see cref="IPriority{T}" />
    /// </summary>
    /// <typeparam name="T">Type of the Object.</typeparam>
    public class PriorityObject<T> : IPriority<T>
    {
        private readonly T _object;
        private readonly int _priority;

        /// <summary>
        ///     Create a new <see cref="PriorityObject{T}" />.
        /// </summary>
        /// <param name="priority">Priority of the Object. The lower value, the higher priority.</param>
        /// <param name="object">Object.</param>
        public PriorityObject(int priority, T @object)
        {
            _priority = priority;
            _object = @object;
        }

        /// <summary>
        ///     Priority of the object. The lower value, the higher priority.
        /// </summary>
        public int Priority
        {
            get { return _priority; }
        }

        /// <summary>
        ///     Object.
        /// </summary>
        public T Object
        {
            get { return _object; }
        }
    }
}