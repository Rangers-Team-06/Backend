using Team_06.DTOs;

namespace Team_06.Services.Interfaces
{
    public interface IToolService
    {
        Task<ToolDto> RegisterToolAsync(CreateToolDto createToolDto);
        Task<List<ToolDto>> GetToolsByNameAsync(string name);
        Task<ToolDto?> GetToolByIdAsync(string id);
        Task<List<ToolDto>> GetAllToolsAsync();
        Task<ToolDto?> UpdateToolAsync(string id, UpdateToolDto updateToolDto);
        Task<bool> DeleteToolAsync(string id);
        Task<List<ToolDto>> SearchToolsAsync(string searchTerm);
    }
}