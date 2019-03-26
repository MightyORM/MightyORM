﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MightyOrm.Generic.Tests.SqlServer.TableClasses
{
	public class Product
	{
		public int ProductID { get; set; }
		public int CategoryID { get; set; }
	}

	public class Products : MightyOrm<Product>
	{
		public Products() : this(includeSchema:true)
		{
		}


		public Products(bool includeSchema) :
			base(TestConstants.WriteTestConnection, includeSchema ? "dbo.Products" : "Products", "ProductID")
		{
		}
	}
}
