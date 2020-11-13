using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.Entities;
using Project.Data.Exceptions;
using Project.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repositories {
    public class CategoryRepository : ICategoryRepository {

        private ApplicationContext _context;

        public CategoryRepository(ApplicationContext context) {
            _context = context ?? throw new NullDbContextException(nameof(context));
        }
        public async Task AddAsync(Category entity) {
            await _context.Category.AddAsync(entity);
        }

        public void Dispose() {
            _context.Dispose();
        }

        public async Task<IEnumerable<Category>> GetAllAsync() {
            return await _context.Category.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(int id) {
            return await _context.Category.FindAsync(id);
        }

        public void Remove(Category entity) {
            DetachLocalEntity(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            _context.Category.Remove(entity);
        }

        public void Update(Category entity) {
            DetachLocalEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private void DetachLocalEntity(Category entity) {
            var local = _context.Set<Category>()
                        .Local
                        .FirstOrDefault(x => x.CategoryId == entity.CategoryId);
            if (local != null) {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Category.Attach(entity);
        }
    }
}
