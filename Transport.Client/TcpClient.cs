using System;
using System.Net.Sockets;

namespace Transport.Client
{
    /// <summary>
    /// TCP client for network transport
    /// </summary>
    public class TcpClient : IDisposable
    {
        private readonly Socket _socket;
        private bool _isConnected;
        private bool _disposed;

        /// <summary>
        /// Initializes a new TCP client
        /// </summary>
        public TcpClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _isConnected = false;
            _disposed = false;
        }

        /// <summary>
        /// Connects to a remote endpoint
        /// </summary>
        /// <param name="host">Host to connect to</param>
        /// <param name="port">Port to connect to</param>
        public void Connect(string host, int port)
        {
            // TODO: Implement actual connection logic
            _isConnected = true;
        }

        /// <summary>
        /// Gets the connection status
        /// </summary>
        public bool IsConnected => _isConnected;

        /// <summary>
        /// Disposes the TCP client
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the TCP client
        /// </summary>
        /// <param name="disposing">True if disposing managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _socket?.Dispose();
                _disposed = true;
            }
        }
    }
}
