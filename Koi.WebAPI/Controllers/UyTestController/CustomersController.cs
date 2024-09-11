using Koi.Repositories.Models.TestDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Koi.WebAPI.Controllers.UyTestController
{
    public class CustomersController : ODataController
    {
        private static Random random = new Random();

        private static List<CustomerTestDTO> customers = new List<CustomerTestDTO>(
            Enumerable.Range(1, 3).Select(idx => new CustomerTestDTO
            {
                Id = idx,
                Name = $"Customer {idx}",
                Orders = new List<OrderTestDTO>(
                    Enumerable.Range(1, 2).Select(dx => new OrderTestDTO
                    {
                        Id = (idx - 1) * 2 + dx,
                        Amount = random.Next(1, 9) * 10
                    }))
            }));

        [EnableQuery]
        public ActionResult<IEnumerable<CustomerTestDTO>> Get()
        {
            return Ok(customers);
        }

        [EnableQuery]
        public ActionResult<CustomerTestDTO> Get([FromRoute] int key)
        {
            var item = customers.SingleOrDefault(d => d.Id.Equals(key));

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }
    }
}