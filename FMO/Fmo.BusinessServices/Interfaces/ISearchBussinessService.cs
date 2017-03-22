using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DataServices.Entities;


namespace Fmo.BusinessServices.Interfaces
{

    public interface ISearchBussinessService
    {

        List<AdvanceSearch> FetchAdvanceSearchDetails();
    }
}
