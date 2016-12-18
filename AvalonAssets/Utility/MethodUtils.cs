using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AvalonAssets.Utility
{
    /// <summary>
    ///     Gets MethodInfo of a generic method via Reflection.
    ///     http://blog.functionalfun.net/2009/10/getting-methodinfo-of-generic-method.html
    /// </summary>
    public static class MethodUtils
    {
        /// <summary>
        ///     Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetMethodInfo(Expression<Action> expression)
        {
            return GetMethodInfo((LambdaExpression) expression);
        }

        /// <summary>
        ///     Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T">Paramter type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetMethodInfo<T>(Expression<Action<T>> expression)
        {
            return GetMethodInfo((LambdaExpression) expression);
        }

        /// <summary>
        ///     Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <typeparam name="T">Paramter type.</typeparam>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            return GetMethodInfo((LambdaExpression) expression);
        }

        /// <summary>
        ///     Given a lambda expression that calls a method, returns the method info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo GetMethodInfo(LambdaExpression expression)
        {
            var outermostExpression = expression.Body as MethodCallExpression;
            if (outermostExpression == null)
                throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
            return outermostExpression.Method;
        }

        /// <summary>
        ///     Returns a construtor info of type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">Object type.</typeparam>
        /// <param name="types">Parameter type of the constructor.</param>
        /// <returns>ConstructorInfo.</returns>
        public static ConstructorInfo GetConstructor<T>(params Type[] types)
        {
            if (types == null)
                types = new Type[0];
            return typeof(T).GetConstructor(types);
        }
    }
}