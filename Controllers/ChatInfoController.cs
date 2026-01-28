/*
 * ============================================================
 * CONTROLADOR DE INFORMAÇÕES DE CHAT (API REST)
 * ============================================================
 */

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
    private readonly IServicoChat _servicoChat;

    public ChatInfoController(IServicoChat servicoChat)
    {
        _servicoChat = servicoChat;
    }

    /// <summary>
    /// Verifica se o chat tem algum atendente atribuído.
    /// </summary>
    [HttpGet(nameof(IsChatWithSomeone))]
    public async Task<ActionResult<bool>> IsChatWithSomeone(string chatId)
    {
        try
        {
            var chatInfo = await _servicoChat.GetChatInfoAsync(chatId);

            if (chatInfo == null)
                return BadRequest("Não encontramos nenhum chat com o ID fornecido!");

            return Ok(chatInfo.IsAnyAttendantAssigned);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro inesperado: {e.Message}");
        }
    }

    /// <summary>
    /// Retorna as informações completas de um chat.
    /// </summary>
    [HttpGet(nameof(GetChatInfo))]
    public async Task<ActionResult<RegistroDeChat>> GetChatInfo(string chatId)
    {
        try
        {
            var chatInfo = await _servicoChat.GetChatInfoAsync(chatId);

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
    /// Retorna o histórico de pesquisas do banco de dados.
    /// </summary>
    [HttpGet(nameof(GetHistory))]
    public async Task<ActionResult<List<RegistroDeChat>>> GetHistory([FromQuery] int limit = 50)
    {
        try
        {
            var history = await _servicoChat.GetHistoryAsync(limit);
            return Ok(history);
        }
        catch (Exception e)
        {
            return StatusCode(500, $"Erro inesperado: {e.Message}");
        }
    }
}
