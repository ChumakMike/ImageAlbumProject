using Project.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Data.Interfaces {
    public interface ICategoryRepository : IRepository<Category>, IDisposable {

    }
}
