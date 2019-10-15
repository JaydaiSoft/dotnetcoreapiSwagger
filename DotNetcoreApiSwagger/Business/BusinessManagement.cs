using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Business
{
    public class BusinessManagement : IBusinessManagement
    {
        public BusinessManagement()
        {

        }

        public string CalculateNumberSeries()
        {
            int[] numberSeries = new int[7];
            numberSeries = RegonitionPattern(ref numberSeries);
            return string.Format("X, 5, 9, 15, 23, Y, Z Result : X = {0}, Y = {1}, Z = {2}"
                ,numberSeries[0], numberSeries[5], numberSeries[6]);
        }

        private int[] RegonitionPattern(ref int[] numberSeries)
        {
            for (int i = 0; i <= numberSeries.Length - 1; i++)
            {
                if (i == 0)
                {
                    numberSeries[0] = 3;
                }
                else
                {
                    int n = i + 1;
                    numberSeries[i] = numberSeries[i - 1] + 2 * (n - 1);
                }
            }
            return numberSeries;
        }
    }
}
