﻿/* Copyright 2010-2012 10gen Inc.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using NUnit.Framework;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace MongoDB.DriverUnitTests.Jira.CSharp130
{
    [TestFixture]
    public class CSharp130Tests
    {
#pragma warning disable 649 // never assigned to
        private class C
        {
            public ObjectId Id;
            public IList<int> List;
        }
#pragma warning restore

        private MongoServer _server;
        private MongoDatabase _database;
        private MongoCollection _collection;

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            var clientSettings = Configuration.TestClient.Settings.Clone();
            clientSettings.WriteConcern = WriteConcern.None;
            var client = new MongoClient(clientSettings); // FireAndForget=true
            _server = client.GetServer();
            _database = _server.GetDatabase(Configuration.TestDatabase.Name);
            _collection = _database.GetCollection<C>(Configuration.TestCollection.Name);
        }

        [Test]
        public void TestLastErrorMessage()
        {
            using (_server.RequestStart(_database))
            {
                var c = new C { List = new List<int>() };

                // insert it once
                _collection.Insert(c);
                var lastError = _server.GetLastError();
                Assert.AreEqual(0, lastError.DocumentsAffected);
                Assert.IsFalse(lastError.HasLastErrorMessage);
                Assert.IsNull(lastError.LastErrorMessage);
                Assert.IsFalse(lastError.UpdatedExisting);

                // insert it again (expect duplicate key error)
                _collection.Insert(c);
                lastError = _server.GetLastError();
                Assert.AreEqual(0, lastError.DocumentsAffected);
                Assert.IsTrue(lastError.HasLastErrorMessage);
                Assert.IsNotNull(lastError.LastErrorMessage);
                Assert.IsFalse(lastError.UpdatedExisting);
            }
        }
    }
}