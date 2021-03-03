﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mighty.Dynamic.Tests.Oracle.TableClasses
{
    public class SPTestsDatabase : MightyOrm
    {
        public SPTestsDatabase(string providerName, bool providerNameOnly = false)
            : base(providerNameOnly ? $"ProviderName={providerName}" : string.Format(TestConstants.ReadWriteTestConnection, providerName))
        {
        }
    }
}
