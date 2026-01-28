using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REVOPS.DevChallenge.Context.Entities;
using REVOPS.DevChallenge.Services;

namespace REVOPS.DevChallenge.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v1/[controller]")]
public class ChatInfoController : ControllerBase
{
    private readonly IChatInfoService _chatInfoService;

    public ChatInfoController(IChatInfoService chatInfoService)
    {
        _chatInfoService = chatInfoService;
    }

    /// <summary>
    /// This endpoint makes a request to Talk, searching for the chat with the specified <paramref name="chatId"/> and tells us if any attendant is assigned.
    /// </summary>
    [HttpGet(nameof(IsChatWithSomeone))]
    public async Task<ActionResult<bool>> IsChatWithSomeone(string chatId)
    {
        try
        {
            var chatInfo = await _chatInfoService.GetChatInfoAsync(chatId);

            if (chatInfo == null)
                return BadRequest("Não encontramos nenhum chat com o ID fornecido!");

            return Ok(chatInfo.IsAnyAttendantAssigned);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro inesperado ao processar a requisição: {e.Message}");
        }
    }

    /// <summary>
    /// Gets full chat information from Talk API
    /// </summary>
    [HttpGet(nameof(GetChatInfo))]
    public async Task<ActionResult<ChatInfoRecord>> GetChatInfo(string chatId)
    {
        try
        {
            var chatInfo = await _chatInfoService.GetChatInfoAsync(chatId);

            if (chatInfo == null)
                return NotFound("Chat não encontrado");

            return Ok(chatInfo);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro inesperado: {e.Message}");
        }
    }

    /// <summary>
    /// Gets search history from the database
    /// </summary>
    [HttpGet(nameof(GetHistory))]
    public async Task<ActionResult<List<ChatInfoRecord>>> GetHistory([FromQuery] int limit = 50)
    {
        try
        {
            var history = await _chatInfoService.GetHistoryAsync(limit);
            return Ok(history);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro inesperado: {e.Message}");
        }
    }

    /// <summary>
    /// Updates contact information in Talk API
    /// </summary>
    [HttpPatch(nameof(UpdateContact))]
    public async Task<ActionResult<bool>> UpdateContact([FromQuery] string contactId, [FromBody] ContactUpdateModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(contactId))
                return BadRequest("ID do contato é obrigatório.");

            var success = await _chatInfoService.UpdateContactAsync(contactId, model.Name, model.PhoneNumber, model.Email);
            
            if (success)
                return Ok(true);
            
            return StatusCode(500, "Falha ao atualizar contato no Talk API.");
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro inesperado: {e.Message}");
        }
    }

    public class ContactUpdateModel
    {
        public string Name { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string? Email { get; set; }
    }
}
