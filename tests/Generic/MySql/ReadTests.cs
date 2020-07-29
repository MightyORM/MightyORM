﻿using System;
using System.Linq;

using NUnit.Framework;

using Mighty.Generic.Tests.MySql.TableClasses;

namespace Mighty.Generic.Tests.MySql
{
    [TestFixture("MySql.Data.MySqlClient")]
#if !DISABLE_DEVART
    [TestFixture("Devart.Data.MySql")]
#endif
    public class ReadTests
    {
        private readonly string ProviderName;

        /// <summary>
        /// Initialise tests for given provider
        /// </summary>
        /// <param name="providerName">Provider name</param>
        public ReadTests(string providerName)
        {
            ProviderName = providerName;
        }


        [Test]
        public void Max_SingleArg()
        {
            var soh = new Films(ProviderName);
            var result = soh.Max(columns: "film_id", where: "rental_duration > @0", args: 6);
            Assert.AreEqual(988, result);
        }


        [Test]
        public void Max_TwoArgs()
        {
            var soh = new Films(ProviderName);
            var result = soh.Max(columns: "film_id", where: "rental_duration > @0 AND rental_duration < @1", args: new object[] { 6, 100 });
            Assert.AreEqual(988, result);
        }


        [Test]
        public void Max_NameValuePair()
        {
            var films = new Films(ProviderName);
            var result = films.Max(columns: "film_id", whereParams: new { rental_duration = 6 });
            Assert.AreEqual(998, result);
        }


        [Test]
        public void EmptyElement_ProtoType()
        {
            var films = new Films(ProviderName);
            var defaults = films.New();
            Assert.IsTrue(defaults.last_update > DateTime.MinValue);
        }


        [Test]
        public void SchemaTableMetaDataRetrieval()
        {
            var films = new Films(ProviderName);
            var metaData = films.TableMetaData;
            Assert.IsNotNull(metaData);
            Assert.AreEqual(13, metaData.Count());
            Assert.IsTrue(metaData.All(v => v.TABLE_NAME == films.BareTableName));
        }


        [Test]
        public void All_NoParameters()
        {
            var films = new Films(ProviderName);
            var allRows = films.All().ToList();
            Assert.AreEqual(1000, allRows.Count);
        }


        [Test]
        public void All_NoParameters_Streaming()
        {
            var films = new Films(ProviderName);
            var allRows = films.All();
            var count = 0;
            foreach(var r in allRows)
            {
                count++;
                Assert.Greater(r.film_id, 0);
                Assert.Greater(r.last_update, DateTime.MinValue);
                Assert.Greater(r.language_id, 0);
                Assert.Greater(r.rental_duration, 0);
                Assert.AreNotEqual(r.description, "");
            }
            Assert.AreEqual(1000, count);
        }


        [Test]
        public void All_LimitSpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(limit: 10).ToList();
            Assert.AreEqual(10, allRows.Count);
        }


        [Test]
        public void All_ColumnSpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(columns: "film_id, description, language_id").ToList();
            Assert.AreEqual(1000, allRows.Count);
            var firstRow = allRows[0];
            Assert.Greater(firstRow.film_id, 0);
            Assert.AreNotEqual("", firstRow.description);
            Assert.Greater(firstRow.language_id, 0);
        }


        [Test]
        public void All_OrderBySpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(orderBy: "rental_duration DESC").ToList();
            Assert.AreEqual(1000, allRows.Count);
            int previous = int.MaxValue;
            foreach(var r in allRows)
            {
                int current = r.rental_duration;
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }


        [Test]
        public void All_WhereSpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(where: "WHERE rental_duration=@0", args: 5).ToList();
            Assert.AreEqual(191, allRows.Count);
        }


        [Test]
        public void All_WhereSpecification_OrderBySpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(orderBy: "film_id DESC", where: "WHERE rental_duration=@0", args: 5).ToList();
            Assert.AreEqual(191, allRows.Count);
            int previous = int.MaxValue;
            foreach(var r in allRows)
            {
                int current = r.film_id;
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }


        [Test]
        public void All_WhereSpecification_ColumnsSpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(columns: "film_id, description, language_id", where: "WHERE rental_duration=@0", args: 5).ToList();
            Assert.AreEqual(191, allRows.Count);
            var firstRow = allRows[0];
            Assert.Greater(firstRow.film_id, 0);
            Assert.AreNotEqual(firstRow.description, "");
            Assert.Greater(firstRow.language_id, 0);
        }


        [Test]
        public void All_WhereSpecification_ColumnsSpecification_LimitSpecification()
        {
            var films = new Films(ProviderName);
            var allRows = films.All(limit: 2, columns: "film_id, description, language_id", where: "WHERE rental_duration=@0", args: 5).ToList();
            Assert.AreEqual(2, allRows.Count);
            var firstRow = allRows[0];
            Assert.Greater(firstRow.film_id, 0);
            Assert.AreNotEqual(firstRow.description, "");
            Assert.Greater(firstRow.language_id, 0);
        }


#if DYNAMIC_METHODS
        [Test]
        public void Find_AllColumns()
        {
            dynamic films = new Films(ProviderName);
            var singleInstance = films.Find(film_id: 43);
            Assert.AreEqual(43, singleInstance.film_id);
        }


        [Test]
        public void Find_OneColumn()
        {
            dynamic films = new Films(ProviderName);
            var singleInstance = films.Find(film_id: 43, columns: "film_id");
            Assert.AreEqual(43, singleInstance.film_id);
            Assert.AreEqual(DateTime.MinValue, singleInstance.last_update);
        }


        [Test]
        public void Get_AllColumns()
        {
            dynamic films = new Films(ProviderName);
            var singleInstance = films.Get(film_id: 43);
            Assert.AreEqual(43, singleInstance.film_id);
            Assert.Greater(singleInstance.last_update, DateTime.MinValue);
        }


        [Test]
        public void First_AllColumns()
        {
            dynamic films = new Films(ProviderName);
            var singleInstance = films.First(film_id: 43);
            Assert.AreEqual(43, singleInstance.film_id);
        }
#endif


        [Test]
        public void Single_Where_AllColumns()
        {
            var films = new Films(ProviderName);
            var singleInstance = films.Single(new { film_id = 43 });
            Assert.AreEqual(43, singleInstance.film_id);
        }


        [Test]
        public void Single_Key_AllColumns()
        {
            var films = new Films(ProviderName);
            var singleInstance = films.Single(43);
            Assert.AreEqual(43, singleInstance.film_id);
        }


        [Test]
        public void Query_AllRows()
        {
            var films = new Films(ProviderName);
            var allRows = films.Query("SELECT * FROM sakila.film").ToList();
            Assert.AreEqual(1000, allRows.Count);
        }


        [Test]
        public void Query_Filter()
        {
            var films = new Films(ProviderName);
            var filteredRows = films.Query("SELECT * FROM sakila.film WHERE rental_duration=@0", 5).ToList();
            Assert.AreEqual(191, filteredRows.Count);
        }


        [Test]
        public void Paged_NoSpecification()
        {
            var films = new Films(ProviderName);
            // no order by, and paged queries logically must have an order by; this will order on PK
            var page2 = films.Paged(currentPage: 2, pageSize: 30);
            var pageItems = page2.Items.ToList();
            Assert.AreEqual(30, pageItems.Count);
            Assert.AreEqual(1000, page2.TotalRecords);
        }


        [Test]
        public void Paged_WhereSpecification()
        {
            var films = new Films(ProviderName);
            var page11 = films.Paged(currentPage: 11, where: "description LIKE @0", args: "%the%");
            var pageItems = page11.Items.ToList();
            Assert.AreEqual(1, pageItems.Count); // also testing being on last page
            Assert.AreEqual(201, page11.TotalRecords);
        }


        [Test]
        public void Paged_OrderBySpecification()
        {
            var films = new Films(ProviderName);
            var page2 = films.Paged(orderBy: "rental_duration DESC", currentPage: 2, pageSize: 30);
            var pageItems = page2.Items.ToList();
            Assert.AreEqual(30, pageItems.Count);
            Assert.AreEqual(1000, page2.TotalRecords);

            int previous = int.MaxValue;
            foreach(var r in pageItems)
            {
                int current = r.rental_duration;
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }


        [Test]
        public void Paged_OrderBySpecification_ColumnsSpecification()
        {
            var films = new Films(ProviderName);
            var page2 = films.Paged(columns: "rental_duration, film_id", orderBy: "rental_duration DESC", currentPage: 2, pageSize: 30);
            var pageItems = page2.Items.ToList();
            Assert.AreEqual(30, pageItems.Count);
            Assert.AreEqual(1000, page2.TotalRecords);
            int previous = int.MaxValue;
            foreach(var r in pageItems)
            {
                int current = r.rental_duration;
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }


        [Test]
        public void Count_NoSpecification()
        {
            var films = new Films(ProviderName);
            var total = films.Count();
            Assert.AreEqual(1000, total);
        }


        [Test]
        public void Count_WhereSpecification()
        {
            var films = new Films(ProviderName);
            var total = films.Count(where: "WHERE rental_duration=@0", args: 5);
            Assert.AreEqual(191, total);
        }


        [Test]
        public void DefaultValue()
        {
            var films = new Films(ProviderName, false);
            var value = films.GetColumnDefault("last_update");
            Assert.AreEqual(typeof(DateTime), value.GetType());
        }


        [Test]
        public void IsValid_FilmIDCheck()
        {
            var films = new Films(ProviderName);
            var toValidate = films.Single(new { film_id = 72 });
            // is invalid
            Assert.AreEqual(1, films.IsValid(toValidate).Count);

            toValidate = films.Single(new { film_id = 2 });
            // is valid
            Assert.AreEqual(0, films.IsValid(toValidate).Count);
        }


        [Test]
        public void PrimaryKey_Read_Check()
        {
            var films = new Films(ProviderName);
            var toValidate = films.Single(new { film_id = 45 });

            Assert.IsTrue(films.HasPrimaryKey(toValidate));

            var pkValue = films.GetPrimaryKey(toValidate);
            Assert.AreEqual(45, pkValue);
        }

        public class BitTest
        {
            public int id;
            public bool tinyint_bool; // 0, 1, 2 => false, true, true
            public bool tinyint_one; // 0, 1, 2 => false, true, true
            public bool bit_one; // 0, 1, 0 => false, true, false
        }

        [Test]
        public void BoolTypes()
        {
            var db = new MightyOrm<BitTest>(string.Format(TestConstants.WriteTestConnection, ProviderName));
            var results = db.All();
            foreach (var result in results)
            {
                // check all the bool values, which - note - come out the same on either driver
                Assert.That(result.tinyint_bool, Is.EqualTo(result.id != 1), "tinyint_bool");
                Assert.That(result.tinyint_one, Is.EqualTo(result.id != 1), "tinyint_one");
                Assert.That(result.bit_one, Is.EqualTo(result.id == 2), "bit_one");
            }
        }
    }
}
