using System;
using System.Collections.Generic;
using System.Text;

namespace mdaWar.Http
{
    public class WarOperations
    {
        private readonly Context dbContext;

        public WarOperations(Context dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
