using System;
using System.Linq.Expressions;

namespace Restaurant.Domain.Specifications._Base
{
    /// <summary>
    /// Specification pattern is a pattern that allows us
    /// to encapsulate some piece of domain knowledge
    /// into a single unit – specification – and reuse it
    /// in different parts of the code base.
    /// </summary>
    /// <typeparam name="T">Entity type</typeparam>
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();

        public static readonly Specification<T> All = new IdentitySpecification<T>();

        public bool IsSatisfiedBy(T entity)
        {
            var predicate = ToExpression().Compile();
            return predicate(entity);
        }

        public Specification<T> And(Specification<T> specification)
        {
            if (this == All)
            {
                return specification;

            }

            return specification == All ? this : new AndSpecification<T>(this, specification);
        }

        public Specification<T> Or(Specification<T> specification)
        {
            if (this == All || specification == All)
            {
                return All;
            }

            return new OrSpecification<T>(this, specification);
        }

        public Specification<T> Not()
        {
            return new NotSpecification<T>(this);
        }
    }
}