using System.Collections.Generic;
using AvalonAssets.Algorithm.Injection.Exception;

namespace AvalonAssets.Algorithm.Injection.Constructor
{
    /// <summary>
    ///     Constructors used for injection.
    /// </summary>
    /// <seealso cref="Constructors" />
    public interface IConstructor
    {
        /// <summary>
        ///     Creates a new instance of object.
        /// </summary>
        /// <param name="container">Container owner.</param>
        /// <param name="parameters">Avaiable arguements.</param>
        /// <returns>New instance.</returns>
        /// <exception cref="TypeNotRegisteredException">
        ///     One of the parameter is not registered and defined in
        ///     <paramref name="parameters" />.
        /// </exception>
        object NewInstance(IContainer container, IDictionary<string, object> parameters);
    }
}