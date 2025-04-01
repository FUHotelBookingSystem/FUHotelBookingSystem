using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.Repositories
{
    public interface IRepoGeneric<T>
    {
        IEnumerable<T> GetAll();
        T GetbyId(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    }
}
