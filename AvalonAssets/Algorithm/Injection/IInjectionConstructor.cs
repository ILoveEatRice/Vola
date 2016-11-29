using System.Collections.Generic;

namespace AvalonAssets.Algorithm.Injection
{
    /// <summary>
    ///     Constructors used for injection.
    /// </summary>
    public interface IInjectionConstructor
    {
        /// <summary>
        ///     Creates a new instance of object.
        /// </summary>
        /// <param name="container">Container owner.</param>
        /// <param name="arguments">Avaiable arguements.</param>
        /// <returns></returns>
        object NewInstance(IContainer container, IDictionary<string, object> arguments);
    }
}