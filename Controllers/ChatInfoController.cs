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
    /// Verifica se o chat possui algum atendente atribuído.
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
    /// Obtém as informações completas do chat na API do Talk.
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
    /// Obtém o histórico de pesquisas do banco de dados.
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
}
