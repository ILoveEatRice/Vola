namespace AvalonAssets.Algorithm.Injection.Parameter
{
    /// <summary>
    ///     Parameters used for injection.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        ///     Field name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Returns value of the parameter.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <returns>Value of the parameter.</returns>
        object Value(IContainer container);
    }
}