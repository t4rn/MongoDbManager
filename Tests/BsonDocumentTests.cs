using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class BsonDocumentTests
    {
        public BsonDocumentTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [Test]
        public void EmptyDocument()
        {
            var doc = new BsonDocument();

            Console.WriteLine(doc);
        }

        [Test]
        public void AddElements()
        {
            var person = new BsonDocument()
            {
                { "age", new BsonInt32(54) },
                { "isAlive", true }
            };

            person.Add("firstName", new BsonString("bob"));

            Console.WriteLine(person);
        }

        [Test]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("address",
                new BsonArray(new[] 
                {
                    "101 Some Road",
                    "Unit 501",
                    "Marszałkowska Street 20"
                }));

            Console.WriteLine(person);
        }

        [Test]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument
            {
                { "age", new BsonInt32(11) },
                { "isAlive", false },
                {
                    "contact", new BsonDocument
                    {
                        {"phone", "123-456-7890"},
                        {"email", "whatever@email.com"}
                    }
                }
            };

            Console.WriteLine(person);
        }

        [Test]
        public void BsonValueConversions()
        {
            var person = new BsonDocument
            {
                {"age", 54}
            };

            Console.WriteLine(person["age"].ToDouble() + 10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);

            Console.WriteLine(person["age"].BsonType);
        }

        [Test]
        public void ToBson()
        {
            var person = new BsonDocument
            {
                {"firstName", "bob"}
            };

            var bson = person.ToBson();

            Console.WriteLine(BitConverter.ToString(bson));

            var deserializedPerson = BsonSerializer.Deserialize<BsonDocument>(bson);

            Console.WriteLine(deserializedPerson);
        }
    }
}
