using Microsoft.AspNetCore.Mvc;
using Team_06.DTOs;
using Team_06.Services.Interfaces;

namespace Team_06.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ToolsController : ControllerBase
    {
        private readonly IToolService _toolService;
        private readonly ILogger<ToolsController> _logger;

        public ToolsController(IToolService toolService, ILogger<ToolsController> logger)
        {
            _toolService = toolService;
            _logger = logger;
        }

        /// <summary>
        /// Register a new tool
        /// </summary>
        /// <param name="createToolDto">Tool registration data</param>
        /// <returns>The registered tool</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ToolDto>> RegisterTool([FromBody] CreateToolDto createToolDto)
        {
            try
            {
                _logger.LogInformation("Registering new tool: {FriendlyName}", createToolDto.FriendlyName);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var registeredTool = await _toolService.RegisterToolAsync(createToolDto);
                
                _logger.LogInformation("Tool registered successfully with ID: {ToolId}", registeredTool.Id);
                
                return CreatedAtAction(nameof(GetToolById), new { id = registeredTool.Id }, registeredTool);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering tool: {FriendlyName}", createToolDto.FriendlyName);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while registering the tool");
            }
        }

        /// <summary>
        /// Get tools by friendly name (partial match)
        /// </summary>
        /// <param name="name">The name to search for</param>
        /// <returns>List of matching tools</returns>
        [HttpGet("search/name/{name}")]
        [ProducesResponseType(typeof(List<ToolDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ToolDto>>> GetToolsByName(string name)
        {
            try
            {
                _logger.LogInformation("Searching for tools with name: {Name}", name);

                var tools = await _toolService.GetToolsByNameAsync(name);
                
                _logger.LogInformation("Found {Count} tools matching name: {Name}", tools.Count, name);
                
                return Ok(tools);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for tools by name: {Name}", name);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching for tools");
            }
        }

        /// <summary>
        /// Get a specific tool by ID
        /// </summary>
        /// <param name="id">The tool ID</param>
        /// <returns>The tool if found</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ToolDto>> GetToolById(string id)
        {
            try
            {
                _logger.LogInformation("Getting tool by ID: {ToolId}", id);

                var tool = await _toolService.GetToolByIdAsync(id);
                
                if (tool == null)
                {
                    _logger.LogWarning("Tool not found with ID: {ToolId}", id);
                    return NotFound($"Tool with ID {id} not found");
                }

                return Ok(tool);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting tool by ID: {ToolId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the tool");
            }
        }

        /// <summary>
        /// Get all tools
        /// </summary>
        /// <returns>List of all tools</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<ToolDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ToolDto>>> GetAllTools()
        {
            try
            {
                _logger.LogInformation("Getting all tools");

                var tools = await _toolService.GetAllToolsAsync();
                
                _logger.LogInformation("Retrieved {Count} tools", tools.Count);
                
                return Ok(tools);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all tools");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving tools");
            }
        }

        /// <summary>
        /// Search tools by various criteria
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <returns>List of matching tools</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(List<ToolDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ToolDto>>> SearchTools([FromQuery] string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching tools with term: {SearchTerm}", searchTerm);

                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term cannot be empty");
                }

                var tools = await _toolService.SearchToolsAsync(searchTerm);
                
                _logger.LogInformation("Found {Count} tools matching search term: {SearchTerm}", tools.Count, searchTerm);
                
                return Ok(tools);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching tools with term: {SearchTerm}", searchTerm);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while searching for tools");
            }
        }

        /// <summary>
        /// Update a tool
        /// </summary>
        /// <param name="id">The tool ID</param>
        /// <param name="updateToolDto">Updated tool data</param>
        /// <returns>The updated tool</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ToolDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ToolDto>> UpdateTool(string id, [FromBody] UpdateToolDto updateToolDto)
        {
            try
            {
                _logger.LogInformation("Updating tool with ID: {ToolId}", id);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedTool = await _toolService.UpdateToolAsync(id, updateToolDto);
                
                if (updatedTool == null)
                {
                    _logger.LogWarning("Tool not found for update with ID: {ToolId}", id);
                    return NotFound($"Tool with ID {id} not found");
                }

                _logger.LogInformation("Tool updated successfully with ID: {ToolId}", id);
                
                return Ok(updatedTool);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tool with ID: {ToolId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the tool");
            }
        }

        /// <summary>
        /// Delete a tool
        /// </summary>
        /// <param name="id">The tool ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTool(string id)
        {
            try
            {
                _logger.LogInformation("Deleting tool with ID: {ToolId}", id);

                var deleted = await _toolService.DeleteToolAsync(id);
                
                if (!deleted)
                {
                    _logger.LogWarning("Tool not found for deletion with ID: {ToolId}", id);
                    return NotFound($"Tool with ID {id} not found");
                }

                _logger.LogInformation("Tool deleted successfully with ID: {ToolId}", id);
                
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting tool with ID: {ToolId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the tool");
            }
        }
    }
}