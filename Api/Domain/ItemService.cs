using Api.Models;

namespace Api.Domain
{
    public class ItemService : IItemsService
    {
        private readonly BaseService _baseService;

        public ItemService(BaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<IEnumerable<ItemDto>> GetItems(string[]? itemIds = null)
        {
            if (itemIds == null || itemIds.Count() == 0)
                return await _baseService.GetAllAsync<ItemDto>("api/fetch");

            // PostAsync(arr ids)
            return await _baseService.PostAsync<string[], IEnumerable<ItemDto>>("api/fetch", itemIds);
        }
    }
}
