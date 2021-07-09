using System;
using System.Runtime.Serialization;

namespace LibraryAccounting.DAL.Repositories
{
    /// Класс-исключение
    [Serializable]
    internal class EntityException : Exception
    {
        /// <inheritdoc></inheritdoc>>
        public EntityException() { }

        /// <inheritdoc></inheritdoc>>
        public EntityException(string message) : base(message)
        {
        
        }
        /// <inheritdoc></inheritdoc>>
        public EntityException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <inheritdoc></inheritdoc>>
        protected EntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}