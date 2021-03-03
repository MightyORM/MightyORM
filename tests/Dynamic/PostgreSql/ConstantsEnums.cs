﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mighty.Dynamic.Tests.PostgreSql
{
    public static class TestConstants
    {
        public static readonly string ProviderName = "Npgsql";

#if NETCOREAPP
        public static readonly string ReadWriteTestConnection = "Database=northwind;Server=windows2008r2.sd.local;Port=5432;User Id=postgres;Password=123;providerName={0}";
#else
        public static readonly string ReadWriteTestConnection = "Northwind.ConnectionString.PostgreSql ({0})";
#endif
    }
}
