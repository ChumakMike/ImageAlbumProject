using Microsoft.AspNetCore.Mvc.Formatters;
using Project.Data.Context;
using Project.Data.Exceptions;
using Project.Data.Interfaces;
using Project.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Unit_Of_Work {
    public class UnitOfWork : IUnitOfWork {

        private readonly ApplicationContext _context;
        private CategoryRepository categoryRepository;
        private ImageRepository imageRepository;
        private RatingRepository ratingRepository;

        public UnitOfWork(ApplicationContext context) {
            _context = context ?? throw new NullDbContextException(nameof(context));
        }

        public ICategoryRepository CategoryRepository => categoryRepository ??= new CategoryRepository(_context);

        public IImageRepository ImageRepository => imageRepository ??= new ImageRepository(_context);

        public IRatingRepository RatingRepository => ratingRepository ??= new RatingRepository(_context);

        private bool disposed = false;

        private void Dispose(bool disposingValue) {
           if(!disposed) {
                if (disposingValue) {
                    categoryRepository.Dispose();
                    imageRepository.Dispose();
                    ratingRepository.Dispose();
                }
                disposed = true;
            }
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveAsync() {
            return await _context.SaveChangesAsync();
        }
    }
}
