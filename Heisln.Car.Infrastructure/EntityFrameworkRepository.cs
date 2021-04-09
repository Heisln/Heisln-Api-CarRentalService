using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heisln.Car.Infrastructure
{
    public abstract class EntityFrameWorkRepository
    {
        protected DatabaseContext dbContext;

        protected EntityFrameWorkRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
