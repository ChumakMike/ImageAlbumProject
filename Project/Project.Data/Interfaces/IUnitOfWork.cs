using Project.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repositories {
    public interface IUnitOfWork : IDisposable {
        ICategoryRepository CategoryRepository { get; }
        IImageRepository ImageRepository { get; }
        IRatingRepository RatingRepository { get; }

        Task<int> SaveAsync();
    }
}
