﻿using System;
using System.Runtime.Serialization;

namespace LibraryAccounting.DAL.Repositories
{
    [Serializable]
    /// Класс-исключение
    internal class EntityException : Exception
    {
        public EntityException()
        {
        }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EntityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}