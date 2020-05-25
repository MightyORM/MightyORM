﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using Mighty.DataContracts;
using Mighty.Mapping;

namespace Mighty.Dynamic.Tests.X
{
    /// <remarks>
    /// While it's arguably tedious to update the cache hits and misses here as the tests change, it's:
    /// a) Important the understand exactly why the numbers change when they do, and to check that each
    ///    change actually makes sense, and it's also
    /// b) Essential to be able to spot quickly whenever the caching gets crashed completely by any
    ///    code change!
    /// 
    /// These cache hits/misses tests rely on NUnit running all the tests in the project one at a time,
    /// in name order (which doesn't apply in XUnit, for instance), but they do what is needed and it's
    /// certainly useful to be able to leverage the whole test suite as a caching test.
    /// 
    /// There doesn't seem any way to indicate to NUnit that it should run something before and after all
    /// tests (https://stackoverflow.com/q/18485622) - and even if there was, I suppose that something
    /// wouldn't be a test itself, anyway (as existing [<see cref="OneTimeTearDownAttribute"/>] code isn't,
    /// for example).
    /// </remarks>
    public partial class DataContract
    {
        [Test]
        public void CacheHits()
        {
#if NETFRAMEWORK
#if !NET40
            Assert.AreEqual(551, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(265, DataContractStore.Instance.CacheHits);
#endif
#elif NETCOREAPP
#if NETCOREAPP3_0 || NETCOREAPP3_1
#if DISABLE_DEVART
            Assert.AreEqual(372, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(466, DataContractStore.Instance.CacheHits);
#endif
#elif NETCOREAPP2_0
#if DISABLE_DEVART
            Assert.AreEqual(341, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(466, DataContractStore.Instance.CacheHits);
#endif
#else
#if DISABLE_DEVART
            Assert.AreEqual(305, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(400, DataContractStore.Instance.CacheHits);
#endif
#endif
#endif
        }

        [Test]
        public void CacheMisses()
        {
            // all the dynamic tests should only ever need one contract now,
            // even though some of the dynamic tests specify columns
#if NETFRAMEWORK
#if !NET40
            Assert.AreEqual(3, DataContractStore.Instance.CacheMisses);
#else
            Assert.AreEqual(2, DataContractStore.Instance.CacheMisses);
#endif
#elif NETCOREAPP
#if !(NETCOREAPP1_0 || NETCOREAPP1_1)
            Assert.AreEqual(3, DataContractStore.Instance.CacheMisses);
#else
            Assert.AreEqual(3, DataContractStore.Instance.CacheMisses);
#endif
#endif
        }
    }

    public class TableMetaData
    {
        [Test]
        public void CacheHits()
        {
#if NETFRAMEWORK
#if !NET40
            Assert.AreEqual(12, TableMetaDataStore.Instance.CacheHits);
#else
            Assert.AreEqual(3, TableMetaDataStore.Instance.CacheHits);
#endif
#elif NETCOREAPP
#if DISABLE_DEVART
            Assert.AreEqual(8, TableMetaDataStore.Instance.CacheHits);
#else
            Assert.AreEqual(12, TableMetaDataStore.Instance.CacheHits);
#endif
#endif
        }

        [Test]
        public void CacheMisses()
        {
#if DISABLE_DEVART
            Assert.AreEqual(4, TableMetaDataStore.Instance.CacheMisses);
#else
            Assert.AreEqual(6, TableMetaDataStore.Instance.CacheMisses);
#endif
        }
    }
}

namespace Mighty.Generic.Tests.X
{
    public class DataContract
    {
        [Test]
        public void CacheHits()
        {
#if NETFRAMEWORK
#if !NET40
            Assert.AreEqual(931, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(462, DataContractStore.Instance.CacheHits);
#endif
#elif NETCOREAPP
#if NETCOREAPP3_0 || NETCOREAPP3_1
#if DISABLE_DEVART
            Assert.AreEqual(627, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(778, DataContractStore.Instance.CacheHits);
#endif
#elif NETCOREAPP2_0
#if DISABLE_DEVART
            Assert.AreEqual(596, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(778, DataContractStore.Instance.CacheHits);
#endif
#else
#if DISABLE_DEVART
            Assert.AreEqual(518, DataContractStore.Instance.CacheHits);
#else
            Assert.AreEqual(674, DataContractStore.Instance.CacheHits);
#endif
#endif
#endif
        }

        [Test]
        public void CacheMisses()
        {
#if NETFRAMEWORK
#if !NET40
            Assert.AreEqual(22, DataContractStore.Instance.CacheMisses);
#else
            Assert.AreEqual(21, DataContractStore.Instance.CacheMisses);
#endif
#elif NETCOREAPP
#if NETCOREAPP2_0 || NETCOREAPP3_0 || NETCOREAPP3_1
            Assert.AreEqual(21, DataContractStore.Instance.CacheMisses);
#else
            Assert.AreEqual(18, DataContractStore.Instance.CacheMisses);
#endif
#endif
        }
    }

    public class TableMetaData
    {
        [Test]
        public void CacheHits()
        {
#if NETFRAMEWORK
#if !NET40
            Assert.AreEqual(70, TableMetaDataStore.Instance.CacheHits);
#else
            Assert.AreEqual(25, TableMetaDataStore.Instance.CacheHits);
#endif
#elif NETCOREAPP
#if NETCOREAPP2_0 || NETCOREAPP3_0 || NETCOREAPP3_1
#if DISABLE_DEVART
            Assert.AreEqual(47, TableMetaDataStore.Instance.CacheHits);
#else
            Assert.AreEqual(68, TableMetaDataStore.Instance.CacheHits);
#endif
#else
#if DISABLE_DEVART
            Assert.AreEqual(45, TableMetaDataStore.Instance.CacheHits);
#else
            Assert.AreEqual(67, TableMetaDataStore.Instance.CacheHits);
#endif
#endif
#endif
        }

        [Test]
        public void CacheMisses()
        {
#if NETFRAMEWORK
            Assert.AreEqual(23, TableMetaDataStore.Instance.CacheMisses);
#elif NETCOREAPP2_0 || NETCOREAPP3_0 || NETCOREAPP3_1
#if DISABLE_DEVART
            Assert.AreEqual(16, TableMetaDataStore.Instance.CacheMisses);
#else
            Assert.AreEqual(21, TableMetaDataStore.Instance.CacheMisses);
#endif
#else
#if DISABLE_DEVART
            Assert.AreEqual(14, TableMetaDataStore.Instance.CacheMisses);
#else
            Assert.AreEqual(20, TableMetaDataStore.Instance.CacheMisses);
#endif
#endif
        }
    }
}