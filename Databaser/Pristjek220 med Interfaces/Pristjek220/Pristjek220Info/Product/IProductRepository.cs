﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> FindProductStartingWith(string productNameStart);
    }
}