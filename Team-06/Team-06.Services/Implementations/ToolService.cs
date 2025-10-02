using Microsoft.EntityFrameworkCore;
using Team_06.Data;
using Team_06.Data.Entities;
using Team_06.DTOs;
using Team_06.Services.Interfaces;

namespace Team_06.Services.Implementations
{
    public class ToolService : IToolService
    {
        private readonly ApplicationDbContext _context;

        public ToolService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ToolDto> RegisterToolAsync(CreateToolDto createToolDto)
        {
            var tool = new Tool
            {
                Id = Guid.NewGuid().ToString(),
                FriendlyName = createToolDto.FriendlyName,
                Description = createToolDto.Description,
                Make = createToolDto.Make,
                Model = createToolDto.Model,
                Category = createToolDto.Category,
                Supplier = createToolDto.Supplier,
                PurchaseDate = createToolDto.PurchaseDate,
                UnitCost = createToolDto.UnitCost,
                Currency = createToolDto.Currency,
                QRData = createToolDto.QRData,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Tools.Add(tool);
            await _context.SaveChangesAsync();

            return MapToDto(tool);
        }

        public async Task<List<ToolDto>> GetToolsByNameAsync(string name)
        {
            var tools = await _context.Tools
                .Where(t => t.FriendlyName.Contains(name))
                .OrderBy(t => t.FriendlyName)
                .ToListAsync();

            return tools.Select(MapToDto).ToList();
        }

        public async Task<ToolDto?> GetToolByIdAsync(string id)
        {
            var tool = await _context.Tools.FindAsync(id);
            return tool != null ? MapToDto(tool) : null;
        }

        public async Task<List<ToolDto>> GetAllToolsAsync()
        {
            var tools = await _context.Tools
                .OrderBy(t => t.FriendlyName)
                .ToListAsync();

            return tools.Select(MapToDto).ToList();
        }

        public async Task<ToolDto?> UpdateToolAsync(string id, UpdateToolDto updateToolDto)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
                return null;

            // Update only provided fields
            if (!string.IsNullOrEmpty(updateToolDto.FriendlyName))
                tool.FriendlyName = updateToolDto.FriendlyName;
            
            if (updateToolDto.Description != null)
                tool.Description = updateToolDto.Description;
            
            if (updateToolDto.Make != null)
                tool.Make = updateToolDto.Make;
            
            if (updateToolDto.Model != null)
                tool.Model = updateToolDto.Model;
            
            if (updateToolDto.Category != null)
                tool.Category = updateToolDto.Category;
            
            if (updateToolDto.Supplier != null)
                tool.Supplier = updateToolDto.Supplier;
            
            if (updateToolDto.PurchaseDate.HasValue)
                tool.PurchaseDate = updateToolDto.PurchaseDate;
            
            if (updateToolDto.UnitCost.HasValue)
                tool.UnitCost = updateToolDto.UnitCost;
            
            if (updateToolDto.Currency != null)
                tool.Currency = updateToolDto.Currency;
            
            if (!string.IsNullOrEmpty(updateToolDto.QRData))
                tool.QRData = updateToolDto.QRData;

            tool.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return MapToDto(tool);
        }

        public async Task<bool> DeleteToolAsync(string id)
        {
            var tool = await _context.Tools.FindAsync(id);
            if (tool == null)
                return false;

            _context.Tools.Remove(tool);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ToolDto>> SearchToolsAsync(string searchTerm)
        {
            var tools = await _context.Tools
                .Where(t => t.FriendlyName.Contains(searchTerm) ||
                           (t.Description != null && t.Description.Contains(searchTerm)) ||
                           (t.Make != null && t.Make.Contains(searchTerm)) ||
                           (t.Model != null && t.Model.Contains(searchTerm)) ||
                           (t.Category != null && t.Category.Contains(searchTerm)) ||
                           (t.Supplier != null && t.Supplier.Contains(searchTerm)))
                .OrderBy(t => t.FriendlyName)
                .ToListAsync();

            return tools.Select(MapToDto).ToList();
        }

        private static ToolDto MapToDto(Tool tool)
        {
            return new ToolDto
            {
                Id = tool.Id,
                FriendlyName = tool.FriendlyName,
                Description = tool.Description,
                Make = tool.Make,
                Model = tool.Model,
                Category = tool.Category,
                Supplier = tool.Supplier,
                PurchaseDate = tool.PurchaseDate,
                UnitCost = tool.UnitCost,
                Currency = tool.Currency,
                QRData = tool.QRData,
                CreatedAt = tool.CreatedAt,
                UpdatedAt = tool.UpdatedAt
            };
        }
    }
}