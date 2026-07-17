using System.Collections.Generic;

namespace RestoFlow.Dtos.Responses
{
    public class MenuCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public IEnumerable<MenuItemSimpleDto> Items { get; set; } = new List<MenuItemSimpleDto>();
    }
}
