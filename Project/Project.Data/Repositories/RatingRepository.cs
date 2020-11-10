using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.Entities;
using Project.Data.Exceptions;
using Project.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repositories {
    public class RatingRepository : IRatingRepository {

        private ApplicationContext _context;

        public RatingRepository(ApplicationContext context) {
            _context = context ?? throw new NullDbContextException(nameof(context));
        }
        public async Task AddAsync(Rating entity) {
            await _context.Ratings.AddAsync(entity);
        }

        public void Dispose() {
            _context.Dispose();
        }

        public async Task<IEnumerable<Rating>> GetAllAsync() {
            return await _context.Ratings.ToListAsync();
        }

        public async Task<Rating> GetByIdAsync(int id) {
            return await _context.Ratings.FindAsync(id);
        }

        public void Remove(Rating entity) {
            _context.Ratings.Remove(entity);
        }

        public void Update(Rating entity) {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}