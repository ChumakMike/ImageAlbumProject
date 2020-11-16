using Microsoft.EntityFrameworkCore;
using Project.Data.Context;
using Project.Data.Entities;
using Project.Data.Exceptions;
using Project.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repositories {
    public class ImageRepository : IImageRepository {
        
        private ApplicationContext _context;

        public ImageRepository(ApplicationContext context) {
            _context = context ?? throw new NullDbContextException(nameof(context));
        }
        public async Task AddAsync(Image entity) {
            await _context.Images.AddAsync(entity);
        }

        public void Dispose() {
            _context.Dispose();
        }

        public async Task<IEnumerable<Image>> GetAllAsync() {
            return await _context.Images.ToListAsync();
        }

        public async Task<Image> GetByIdAsync(int id) {
            return await _context.Images.FindAsync(id);
        }

        public void Remove(Image entity) {
            DetachLocalEntity(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            _context.Images.Remove(entity);
        }

        public void Update(Image entity) {
            DetachLocalEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        private void DetachLocalEntity(Image entity) {
            var local = _context.Set<Image>()
                        .Local
                        .FirstOrDefault(x => x.ImageId == entity.ImageId);
            if (local != null) {
                _context.Entry(local).State = EntityState.Detached;
            }
            _context.Images.Attach(entity);
        }
    }
}