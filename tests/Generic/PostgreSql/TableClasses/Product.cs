﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mighty.Generic.Tests.PostgreSql.TableClasses
{
    public class Product
    {
        public int productid { get; set; }
        public string productname { get; set; }
    }

    public class Products : MightyOrm<Product>
    {
        public Products()
            : this(includeSchema: true)
        {
        }


        public Products(bool includeSchema) :
            base(string.Format(TestConstants.ReadWriteTestConnection, TestConstants.ProviderName), includeSchema ? "public.products" : "products", "productid",
#if KEY_VALUES
                string.Empty,
#endif
                "products_productid_seq")
        {
        }
    }
}
