using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Serializer.Runtime
{
    /// <summary>
    /// Global registry for RNS packet field number to name mappings.
    /// Used for diagnostics and validation of packet fields.
    /// </summary>
    public static class RnsKeyRegistry
    {
        // Type -> (fieldNumber -> fieldName)
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<int, string>> _byType = new();

        /// <summary>
        /// Registers field mappings for a packet type.
        /// </summary>
        /// <typeparam name="T">The packet type</typeparam>
        /// <param name="map">Map of field numbers to field names</param>
        public static void Register<T>(IReadOnlyDictionary<int, string> map)
            => _byType[typeof(T)] = map;

        /// <summary>
        /// Attempts to get the field name for a given type and field number.
        /// </summary>
        /// <param name="t">The packet type</param>
        /// <param name="fieldNumber">The field number</param>
        /// <param name="name">The field name if found</param>
        /// <returns>True if the field name was found, false otherwise</returns>
        public static bool TryGet(Type t, int fieldNumber, out string? name)
        {
            name = null;
            return _byType.TryGetValue(t, out var m) && m.TryGetValue(fieldNumber, out name);
        }
    }
}