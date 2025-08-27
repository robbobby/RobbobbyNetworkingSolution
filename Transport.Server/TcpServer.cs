using System;
using System.Net.Sockets;

namespace Transport.Server
{
    /// <summary>
    /// TCP server for network transport
    /// </summary>
    public class TcpServer : IDisposable
    {
        private readonly Socket _listener;
        private bool _isRunning;
        private bool _disposed;

        /// <summary>
        /// Initializes a new TCP server
        /// </summary>
        public TcpServer()
        {
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _isRunning = false;
            _disposed = false;
        }

        /// <summary>
        /// Starts listening for connections
        /// </summary>
        /// <param name="port">Port to listen on</param>
        public void Start(int port)
        {
            // TODO: Implement actual server logic
            _isRunning = true;
        }

        /// <summary>
        /// Gets the server status
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Disposes the TCP server
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the TCP server
        /// </summary>
        /// <param name="disposing">True if disposing managed resources</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _listener?.Dispose();
                _disposed = true;
            }
        }
    }
}
