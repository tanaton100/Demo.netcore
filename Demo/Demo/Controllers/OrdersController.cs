using System;
using System.Collections.Generic;
using Demo.Models;
using Demo.Models.InputModel;
using Demo.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/Order")]

    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        [Route("getall")]
        public IEnumerable<Orders> GetAll()
        {
            var result = _ordersService.GetAllOrder();
            return result;
        }

        [HttpGet]
        [Route("{id}")]
        public Orders GetById([FromRoute]int id)
        {
            var result = _ordersService.GetByIdOrders(id);
            return result;
        }

        [HttpPost]
        [Route("")]
        public Orders AddOrders([FromBody] OrderInput Input)
        {
            var modelInput = new Orders
            {
               UserId = Input.UserId,
               ProductId = Input.ProductId
            };
            var result = _ordersService.AddOrders(modelInput);
            return result;
        }


        [HttpPut]
        [Route("")]
        public bool UpdateOrders([FromBody] Orders orders)
        {

            var result = _ordersService.Update(orders);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public bool DeleteOrders([FromRoute] int id)
        {

            var result = _ordersService.Delete(id);
            return result;
        }

    }
}
