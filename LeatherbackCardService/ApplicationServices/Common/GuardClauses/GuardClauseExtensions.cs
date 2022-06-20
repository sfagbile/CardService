#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationServices.Common.Exceptions;
using ApplicationServices.Interfaces;
using Domain.Exceptions;

namespace ApplicationServices.Common.GuardClauses
{
    public static class GuardClauseExtensions
    {
        /// <summary>
        /// Throws an <see cref="EntityNotFoundException" /> if <paramref name="entity" /> is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guardClause"></param>
        /// <param name="entity"></param>
        /// <param name="parameterName"></param>
        public static void DuplicateEntity<T>(this IGuardClause guardClause, T input, string parameterName)
        {
            if (input is not null) throw new DuplicateException($"Duplicate {parameterName} Request");
        }
        
        /// <summary>
        /// Throws an <see cref="ProviderNotFoundException" /> if <paramref name="entity" /> is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guardClause"></param>
        /// <param name="entity"></param>
        /// <param name="parameterName"></param>
        public static void NullProvider(this IGuardClause guardClause, bool isProviderAvailable) 
        {
            if (!isProviderAvailable) 
                throw new ProviderNotFoundException("No provider has been configured to process this mobile money transaction");
        }

        /// <summary>
        /// Throws an <see cref="EntityNotFoundException" /> if <paramref name="entity" /> is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="guardClause"></param>
        /// <param name="entity"></param>
        /// <param name="parameterName"></param>
        public static void EntityNull<T>(this IGuardClause guardClause, T input, string parameterName)
        {
            if (input is null) throw new EntityNotFoundException($"No {parameterName} is found that matches your search");
        }
        
        /// <summary>
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is empty.
        /// </summary>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <exception cref="InvalidInputException"></exception>
        /// <exception cref="InvalidInputException"></exception>
        public static void NullOrEmpty(this IGuardClause guardClause, string? input, string parameterName, string className)
        {
            if (string.IsNullOrEmpty(input)) throw new InvalidInputException($"Required input {parameterName} of class {className} was empty.");
        }
        
        /// <summary>
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is empty.
        /// </summary>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <exception cref="InvalidInputException"></exception>
        /// <exception cref="InvalidInputException"></exception>
        public static void NullOrEmptyOrDefaultString(this IGuardClause guardClause, string? input, string parameterName, string className)
        {
            if (string.IsNullOrEmpty(input) || input == "string") throw new InvalidInputException($"Required input {parameterName} of class {className} was empty.");
        }
        
        /// <summary>
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is empty.
        /// </summary>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <exception cref="InvalidInputException"></exception>
        /// <exception cref="InvalidInputException"></exception>
        public static void NullOrEmpty(this IGuardClause guardClause, int input, string parameterName, string className)
        {
            if (input == default) throw new InvalidInputException($"Required input {parameterName} of class {className} was empty.");
        }
        
        /// <summary>
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is empty.
        /// </summary>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <exception cref="InvalidInputException"></exception>
        /// <exception cref="InvalidInputException"></exception>
        public static void NullOrEmpty(this IGuardClause guardClause, decimal input, string parameterName, string className)
        {
            if (input == default) throw new InvalidInputException($"Required input {parameterName} of class {className} was empty.");
        }
        
        /// <summary>
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is empty.
        /// </summary>ΩΩΩtcty
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <exception cref="InvalidInputException"></exception>
        /// <exception cref="InvalidInputException"></exception>
        public static void NullOrEmpty(this IGuardClause guardClause, double input, string parameterName, string className)
        {
            if (input == default) throw new InvalidInputException($"Required input {parameterName} of class {className} was empty.");
        }
        
        /// <summary>
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="InvalidInputException" /> if <paramref name="input" /> is empty.
        /// </summary>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <exception cref="InvalidInputException"></exception>
        /// <exception cref="InvalidInputException"></exception>
        public static void NullOrEmpty(this IGuardClause guardClause, Guid input, string parameterName, string className)
        {
            if (input == Guid.Empty) throw new InvalidInputException($"Required input {parameterName} of class {className} was empty.");
        }

        ///
        /// <summary>
        /// Throws an <see cref="ArgumentNullException" /> if <paramref name="input" /> is null.
        /// Throws an <see cref="ArgumentException" /> if <paramref name="input" /> is an empty enumerable.
        /// </summary>
        /// <param name="guardClause"></param>
        /// <param name="input"></param>
        /// <param name="parameterName"></param>
        /// <param name="className"></param>
        /// <returns><paramref name="input" /> if the value is not an empty enumerable or null.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void NullOrEmpty<T>(this IGuardClause guardClause, IEnumerable<T>? input, string parameterName, string className)
        {
            if (input is null || !input.Any()) throw new InvalidInputException($"Required input {parameterName} of class {className}  was empty.");
        }
    }
}