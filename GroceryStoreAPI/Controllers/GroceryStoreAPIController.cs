using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GroceryStoreAPI.Services;
using GroceryStoreAPI.DomainModels;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/customer")]
    public class GroceryStoreAPIController : Controller
    {
        private readonly ILogger _logger;
        private readonly IGroceryStoreAPIServices _groceryStoreAPIServices;
        public GroceryStoreAPIController(ILogger logger, IGroceryStoreAPIServices groceryStoreAPIServices)
        {
            _logger = logger;
            _groceryStoreAPIServices = groceryStoreAPIServices;
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(IList<DomainModels.Customer>))]
        [SwaggerResponse((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Get()
        {
            var res = await _groceryStoreAPIServices.GetAllCustomers();
            if (res != null && res.Count > 0)
                return Ok(res);
            else
                return NoContent();
        }

        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(DomainModels.Customer))]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var res = await _groceryStoreAPIServices.GetCustomerById(id);
            if (res != null)
                return Ok(res);
            else
                return NotFound("not found");
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, Type = typeof(Guid))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody]GeneralInput custRequest)
        {
            if (custRequest == null)
                return BadRequest("bad request");
            if (string.IsNullOrWhiteSpace(custRequest.Name))
                return BadRequest("bad request");

            var res = await _groceryStoreAPIServices.AddCustomer(custRequest);
            if (res.Status == HttpStatusCode.OK)
                return StatusCode(StatusCodes.Status201Created, $"created customer");
            else
                return BadRequest($"bad request - {res.ErrorMessage}");

        }

        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Put([FromBody]Customer custRequest)
        {
            var updateResult = await _groceryStoreAPIServices.UpdateCustomer(custRequest);
            if (updateResult.Status == HttpStatusCode.OK)
                return Ok(updateResult.ErrorMessage);
            else if (updateResult.Status == HttpStatusCode.BadRequest)
                return BadRequest(updateResult.ErrorMessage);
            else if (updateResult.Status == HttpStatusCode.NotFound)
                return NotFound(updateResult.ErrorMessage);

            return BadRequest("please try again");
        }

        [HttpDelete]
        [SwaggerResponse((int)HttpStatusCode.Created)]
        [SwaggerResponse((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _groceryStoreAPIServices.DeleteCustomer(id);
            if (res)
                return StatusCode(201, "successful");
            else
                return NotFound("not found");
        }
    }
}
