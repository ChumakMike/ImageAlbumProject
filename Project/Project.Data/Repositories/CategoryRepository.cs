using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.Entities;
using Project.Data.Exceptions;
using Project.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repositories {
    public class CategoryRepository : ICategoryRepository {

        private ApplicationContext _context;

        public CategoryRepository(ApplicationContext context) {
            _context = context ?? throw new NullDbContextException(nameof(context));
        }
        public async Task AddAsync(Category entity) {
            await _context.Categories.AddAsync(entity);
        }

        public void Dispose() {
            _context.Dispose();
        }

        public async Task<IEnumerable<Category>> GetAllAsync() {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id) {
            return await _context.Categories.FindAsync(id);
        }

        public void Remove(Category entity) {
            _context.Categories.Remove(entity);
        }

        public void Update(Category entity) {
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
