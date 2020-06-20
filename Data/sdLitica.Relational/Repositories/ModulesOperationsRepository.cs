using sdLitica.Analytics;
using sdLitica.Relational.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sdLitica.Relational.Repositories
{
    public class ModulesOperationsRepository: RepositoryBase<ModulesOperations>
    {
        public ModulesOperationsRepository(MySqlDbContext context)
            : base(context)
        {
        }
        public IList<ModulesOperations> GetAll()
        {
            return Entity.ToList();
        }
    }
}
