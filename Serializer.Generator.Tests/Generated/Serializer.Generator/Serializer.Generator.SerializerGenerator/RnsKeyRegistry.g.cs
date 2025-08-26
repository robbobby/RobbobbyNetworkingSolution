// Generated RnsKeyRegistry runtime helper
#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Serializer.Generator.Runtime
{
    public static class RnsKeyRegistry
    {
        private static readonly ConcurrentDictionary<Type, IReadOnlyDictionary<ushort, string>> _byType = new();

        public static void Register<T>(IReadOnlyDictionary<ushort, string> map) => _byType[typeof(T)] = map;

        public static bool TryGet(Type t, ushort key, out string? name)
        {
            name = null;
            return _byType.TryGetValue(t, out var m) && m.TryGetValue(key, out name);
        }
    }
}