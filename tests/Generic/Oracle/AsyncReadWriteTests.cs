﻿#if (NETFRAMEWORK && !NET40) || (NETCOREAPP && !(NETCOREAPP1_0 || NETCOREAPP1_1))
using System;
using System.Data;
using Dasync.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mighty.Generic.Tests.Oracle.TableClasses;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Threading;

namespace Mighty.Generic.Tests.Oracle
{
    /// <summary>
    /// Specific tests for code which is specific to Oracle. This means there are fewer tests than for SQL Server, as logic that's covered there already doesn't have to be
    /// retested again here, as the tests are meant to see whether a feature works. Tests are designed to touch the code in Massive.Oracle. 
    /// </summary>
    /// <remarks>These tests run on x64 by default, as by default ODP.NET installs x64 only. If you have x86 ODP.NET installed, change the build directive to AnyCPU
    /// in the project settings.<br/>
    /// These tests use the SCOTT test DB shipped by Oracle. Your values may vary though. </remarks>
    [TestFixture("Oracle.ManagedDataAccess.Client")]
#if !NETCOREAPP
    [TestFixture("Oracle.DataAccess.Client")]
#endif
    public class AsyncReadWriteTests
    {
        private readonly string ProviderName;

        /// <summary>
        /// Initialise tests for given provider
        /// </summary>
        /// <param name="providerName">Provider name</param>
        public AsyncReadWriteTests(string providerName)
        {
            ProviderName = providerName;
        }


        [Test]
        public async Task All_NoParameters()
        {
            var depts = new Departments(ProviderName);
            var allRows = await (await depts.AllAsync()).ToListAsync();
            Assert.AreEqual(60, allRows.Count);
            foreach (var d in allRows)
            {
                MDebug.WriteLine("{0} {1} {2}", d.DEPTNO, d.DNAME, d.LOC);
            }
        }


        [Test]
        public async Task All_NoParameters_RespondsToCancellation()
        {
            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                var depts = new Departments(ProviderName);
                var allRows = await depts.AllAsync(cts.Token);
                int count = 0;
                Assert.ThrowsAsync<TaskCanceledException>(async () => {
                    await allRows.ForEachAsync(d => {
                        MDebug.WriteLine("{0} {1} {2}", d.DEPTNO, d.DNAME, d.LOC);
                        count++;
                        if (count == 14)
                        {
                            cts.Cancel();
                        }
                    });
                });
                Assert.AreEqual(14, count);
            }
        }


        [Test]
        public async Task All_LimitSpecification()
        {
            var depts = new Departments(ProviderName);
            var allRows = await (await depts.AllAsync(limit: 10)).ToListAsync();
            Assert.AreEqual(10, allRows.Count);
        }


        [Test]
        public async Task All_WhereSpecification_OrderBySpecification()
        {
            var depts = new Departments(ProviderName);
            var allRows = await (await depts.AllAsync(orderBy: "DEPTNO DESC", where: "WHERE LOC=:0", args: "Nowhere")).ToListAsync();
            Assert.AreEqual(9, allRows.Count);
            int previous = int.MaxValue;
            foreach(var r in allRows)
            {
                int current = r.DEPTNO;
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }


        [Test]
        public async Task All_WhereSpecification_OrderBySpecification_LimitSpecification()
        {
            var depts = new Departments(ProviderName);
            var allRows = await (await depts.AllAsync(limit: 6, orderBy: "DEPTNO DESC", where: "WHERE LOC=:0", args: "Nowhere")).ToListAsync();
            Assert.AreEqual(6, allRows.Count);
            int previous = int.MaxValue;
            foreach(var r in allRows)
            {
                int current = r.DEPTNO;
                Assert.IsTrue(current <= previous);
                previous = current;
            }
        }


        [Test]
        public async Task Paged_NoSpecification()
        {
            var depts = new Departments(ProviderName);
            // no order by, and paged queries logically must have an order by; this will order on PK
            var page2 = await depts.PagedAsync(currentPage: 2, pageSize: 10);
            var pageItems = page2.Items.ToList();
            Assert.AreEqual(10, pageItems.Count);
            Assert.AreEqual(60, page2.TotalRecords);
        }


        [Test]
        public async Task Paged_WhereSpecification()
        {
            var depts = new Departments(ProviderName);
            var page4 = await depts.PagedAsync(currentPage: 4, where: "LOC = :0", args: "Somewhere");
            var pageItems = page4.Items.ToList();
            Assert.AreEqual(0, pageItems.Count); // also testing being after last page
            Assert.AreEqual(47, page4.TotalRecords);
        }


        [Test]
        public async Task Paged_OrderBySpecification()
        {
            var depts = new Departments(ProviderName);
            var page2 = await depts.PagedAsync(orderBy: "DEPTNO DESC", currentPage: 2, pageSize: 10);
            var pageItems = page2.Items.ToList();
            Assert.AreEqual(10, pageItems.Count);
            Assert.AreEqual(60, page2.TotalRecords);
        }


        [Test]
        public async Task Paged_SqlSpecification()
        {
            var depts = new Departments(ProviderName);
            var page2 = await depts.PagedFromSelectAsync("SCOTT.EMP e INNER JOIN SCOTT.DEPT d ON e.DEPTNO = d.DEPTNO", "EMPNO", "EMPNO, ENAME, DNAME", pageSize: 5, currentPage: 2);
            var pageItems = page2.Items.ToList();
            Assert.AreEqual(5, pageItems.Count);
            Assert.AreEqual(14, page2.TotalRecords);
            Assert.AreEqual(3, page2.TotalPages);
        }

        [Test]
#pragma warning disable CS1998
        public async Task PagedStarWithJoin_ThrowsInvalidOperationException()
        {
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => {
                var depts = new Departments(ProviderName);
                var page2 = await depts.PagedFromSelectAsync("SCOTT.EMP e INNER JOIN SCOTT.DEPT d ON e.DEPTNO = d.DEPTNO", "EMPNO", "*", pageSize: 2, currentPage: 2);
                var pageItems = page2.Items.ToList();
            });
            // Check that it was thrown for the right reason
            Assert.AreEqual("To query from joined tables you have to specify the columns explicitly not with *", ex.Message);
        }
#pragma warning restore CS1998

        [Test]
#pragma warning disable CS1998
        public async Task PagedNoOrderBy_ThrowsInvalidOperationException()
        {
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => {
                var depts = new Departments(ProviderName);
                var page2 = await depts.PagedFromSelectAsync("SCOTT.EMP e INNER JOIN SCOTT.DEPT d ON e.DEPTNO = d.DEPTNO", null, "EMPNO, ENAME, DNAME", pageSize: 2, currentPage: 2);
                var pageItems = page2.Items.ToList();
            });
            // Check that it was thrown for the right reason
            Assert.AreEqual("Cannot complete paged select operation, you must provide an ORDER BY value", ex.Message);
        }
#pragma warning restore CS1998

        [Test]
        public async Task Insert_SingleRow()
        {
            var depts = new Departments(ProviderName);
            var inserted = await depts.InsertAsync(new { DNAME = "Massive Dep", LOC = "Beach" });
            Assert.IsTrue(inserted.DEPTNO > 0);
            Assert.AreEqual(1, await depts.DeleteAsync(inserted.DEPTNO));
        }


        [Test]
        public async Task Save_SingleRow()
        {
            var depts = new Departments(ProviderName);
            dynamic toSave = new { DNAME = "Massive Dep", LOC = "Beach" }.ToExpando();
            var result = await depts.SaveAsync(toSave);
            Assert.AreEqual(1, result);
            Assert.IsTrue(toSave.DEPTNO > 0);
            Assert.AreEqual(1, await depts.DeleteAsync(toSave.DEPTNO));
        }


        [Test]
        public async Task UpdateUsing_SingleRow()
        {
            var depts = new Departments(ProviderName);
            dynamic toSave = new { DNAME = "Massive Dep", LOC = "Beach" }.ToExpando();
            var saveResult = await depts.SaveAsync(toSave);
            Assert.AreEqual(1, saveResult);
            Assert.IsTrue(toSave.DEPTNO > 0);
            var mightyDep = new { DNAME = "Mighty Dep" };
            Assert.AreEqual(0, (await (await depts.AllAsync(mightyDep)).ToListAsync()).Count);
            Assert.AreEqual(1, await depts.UpdateUsingAsync(mightyDep, toSave.DEPTNO));
            Assert.AreEqual(1, (await (await depts.AllAsync(mightyDep)).ToListAsync()).Count);
            Assert.AreEqual(1, await depts.DeleteAsync(toSave.DEPTNO));
            Assert.AreEqual(0, (await (await depts.AllAsync(mightyDep)).ToListAsync()).Count);
        }


        [Test]
        public async Task Save_MultipleRows()
        {
            var depts = new Departments(ProviderName);
            object[] toSave = new object[]
                                   {
                                       new {DNAME = "Massive Dep", LOC = "Beach"}.ToExpando(),
                                       new {DNAME = "Massive Dep", LOC = "DownTown"}.ToExpando()
                                   };
            var result = await depts.SaveAsync(toSave);
            Assert.AreEqual(2, result);
            foreach(dynamic o in toSave)
            {
                Assert.IsTrue(o.DEPTNO > 0);
            }

            // read them back, update them, save them again, 
            var savedDeps = await (await depts.AllAsync(where: "WHERE DEPTNO=:0 OR DEPTNO=:1", args: new object[] { ((dynamic)toSave[0]).DEPTNO, ((dynamic)toSave[1]).DEPTNO })).ToListAsync();
            Assert.AreEqual(2, savedDeps.Count);
            savedDeps[0].LOC += "C";
            savedDeps[1].LOC += "C";
            result = await depts.SaveAsync(toSave);
            Assert.AreEqual(2, result);
            Assert.AreEqual(1, await depts.DeleteAsync(savedDeps[0].DEPTNO));
            Assert.AreEqual(1, await depts.DeleteAsync(savedDeps[1].DEPTNO));
        }


        [OneTimeTearDown]
        public async Task CleanUp()
        {
            // delete all rows with department name 'Massive Dep'. 
            var depts = new Departments(ProviderName);
            await depts.DeleteAsync("DNAME=:0", "Massive Dep");
        }
    }
}
#endif