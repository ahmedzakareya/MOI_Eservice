using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
	public class RequestCountStatistics:Specification<MoiEserviceLicensesRequest>
	{
        public RequestCountStatistics(int ServiceId)
        {
                
        }
    }
}
