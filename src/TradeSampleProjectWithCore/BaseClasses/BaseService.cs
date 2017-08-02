using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradeSampleProjectWithCore.Models;

namespace TradeSampleProjectWithCore.BaseClasses
{
    public class BaseService
    {
        public readonly TradeSampleContext DbContext;

        public BaseService(TradeSampleContext context)
        {
            this.DbContext = context;
        }
    }
}
