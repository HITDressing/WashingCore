using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HITWashing.Models.DBClass
{
    public class DbInitializer
    {
        public static void Initialize(WashingContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
