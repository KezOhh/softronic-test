using Api.Models;

namespace Api.Domain
{
    public interface IItemsService
    {
        public Task<IEnumerable<ItemDto>> GetItems(string[]? itemIds = null);
    }
}
