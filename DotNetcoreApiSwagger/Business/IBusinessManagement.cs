using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Business
{
    public interface IBusinessManagement
    {
        string CalculateNumberSeries();

        bool LineNotifyMessage(string message);
    }
}
