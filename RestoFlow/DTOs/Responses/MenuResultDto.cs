using System.Collections.Generic;

namespace RestoFlow.Dtos.Responses
{
    public class MenuResultDto
    {
        required public SettingResponseDto Settings { get; set; }
        public IEnumerable<MenuCategoryDto> Menu { get; set; } = new List<MenuCategoryDto>();
    }
}
