using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppServiceCoverage.Infrastructure
{
    public class TeslaStock
    {
        public int Predict(int currentValue)
        {
            int newValue = Convert.ToInt32(currentValue + (.5 * currentValue));
            return newValue;
        }
    }
}
