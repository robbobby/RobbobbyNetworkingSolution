using System;
using System.Diagnostics.CodeAnalysis;

namespace Serializer.Abstractions
{
    /// <summary>
    /// Extension methods for <see cref="IRnsBinaryWritable"/> to provide non-throwing alternatives.
    /// </summary>
    public static class IRnsBinaryWritableExtensions
    {
        /// <summary>
        /// Attempts to write the current instance to the specified buffer without throwing exceptions.
        /// </summary>
        /// <param name="source">The source instance to write.</param>
        /// <param name="destination">The destination buffer to write to.</param>
        /// <param name="bytesWritten">The number of bytes written, or 0 if the operation failed.</param>
        /// <returns>
        /// <see langword="true"/> if the write operation succeeded; otherwise, <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// This method provides a non-throwing alternative to <see cref="IRnsBinaryWritable.Write(Span{byte})"/>
        /// for performance-sensitive code paths where exceptions should be avoided.
        /// </remarks>
        [SuppressMessage("Design", "CA1510:Use ArgumentNullException.ThrowIfNull", Justification = "netstandard2.1 compatibility")]
        public static bool TryWrite(this IRnsBinaryWritable source, Span<byte> destination, out int bytesWritten)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var size = source.GetSerializedSize();
            if (destination.Length < size)
            {
                bytesWritten = 0;
                return false;
            }

            bytesWritten = source.Write(destination);
            return bytesWritten == size;
        }
    }
}
