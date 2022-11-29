using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Customers.Profiles
{
    public class CustomerProfiles: AutoMapper.Profile
    {
        public CustomerProfiles()
        {
            CreateMap<Db.Customer, Models.Customer>();
        }
    }
}
