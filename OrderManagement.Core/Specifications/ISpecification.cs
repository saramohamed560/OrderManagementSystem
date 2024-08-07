﻿using OrderManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Specifications
{
    public  interface ISpecification<T> where T:BaseEntity
    {
        public Expression<Func<T,bool>>? Critria { get; set; }
        public List<Expression<Func<T,object>>> Includes { get; set; }
    }
}
