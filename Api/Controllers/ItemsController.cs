using Api.Domain;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsService _itemsService;

        public ItemsController(IItemsService itemsService)
        {
            _itemsService = itemsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Get()
        {
            try
            {
                var items = await _itemsService.GetItems();

                if (items.Count() == 0)
                    return BadRequest("No items found. Try again");

                return Ok(items);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<ItemDto>>> Post([FromForm] string[] items)
        {
            try
            {
                var res = await _itemsService.GetItems();

                if (res.Count() == 0)
                    return BadRequest("Not matching items found. Try again");

                // NOTE: Im not sure if the Azure Function is set up to accept POST calls. I only managed to get 404 Not Found when posting objects to it.
                // Therefore i made this workaround where I fetch all items and filter the array based on the input. I realised this after I implemented
                // everything on the backend so I just left it for you to have a look at.

                return Ok(res.Where(res => items.Contains(res.Id)));
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<ItemDto>>> Post([FromForm] string[] items)
        //{
        //    try
        //    {
        //        var res = await _itemsService.GetItems(items);

        //        if (res.Count() == 0)
        //            return BadRequest("No items found. Try again");

        //        return Ok(res);
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
