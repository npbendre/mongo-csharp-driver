﻿/* Copyright 2010-2013 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Net.Sockets;

namespace MongoDB.Driver.Core.Connections
{
    /// <summary>
    /// Wraps a socket exception.
    /// </summary>
    [Serializable]
    public class MongoSocketException : MongoDriverException
    {
        /// <summary>
        /// Gets the socket error code.
        /// </summary>
        public SocketError SocketErrorCode
        {
            get { return ((SocketException)InnerException).SocketErrorCode; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoSocketException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        public MongoSocketException(string message, SocketException inner)
            : base(message, inner)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoSocketException" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected MongoSocketException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}