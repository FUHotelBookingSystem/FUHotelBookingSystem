using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.Repositories
{
    public class RepoGeneric<T> : IRepoGeneric<T> where T : class
    {
        private readonly FuhotelBookingSystemContext _context;
        private readonly DbSet<T> _dbSet;

        public RepoGeneric(FuhotelBookingSystemContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }
        public T Add1(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
            return entity;
        }
        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
            _context.SaveChanges();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetbyId(int id)
        {
            return _dbSet.Find(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }


    }
}
