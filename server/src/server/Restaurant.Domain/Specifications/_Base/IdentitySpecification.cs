using System;
using System.Linq.Expressions;

namespace Restaurant.Domain.Specifications._Base
{
    internal sealed class IdentitySpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> ToExpression()
        {
            return x => true;
        }
    }
}