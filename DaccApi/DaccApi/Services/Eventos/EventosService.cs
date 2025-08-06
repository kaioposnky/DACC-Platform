using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Eventos;
using DaccApi.Helpers;

using Helpers.Response;
using Microsoft.AspNetCore.Mvc;


namespace DaccApi.Services.Eventos

{
    public class EventosService : IEventosService
    {
        private readonly IEventosRepository _eventosRepository;

        public EventosService(IEventosRepository eventosRepository)
        {
            _eventosRepository = eventosRepository;
        }

        public async Task<IActionResult> GetAllEventos()
        {
            try
            {
                var eventos = await _eventosRepository.GetAllEventos();

                if (eventos.Count() == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { eventos = eventos}));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR + ex.Message);
            }
        }
        
        public async Task<IActionResult> CreateEvento(Guid autorId, RequestEvento request)
            {
                try
                {
                    if (
                        String.IsNullOrWhiteSpace(request.Titulo) ||
                        String.IsNullOrWhiteSpace(request.Descricao) ||
                        String.IsNullOrWhiteSpace(request.TipoEvento) ||
                        String.IsNullOrWhiteSpace(request.TextoAcao)||
                        String.IsNullOrWhiteSpace(request.LinkAcao)
                        )
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                    }

                    var evento = new Evento()
                    {
                        Titulo = request.Titulo,
                        AutorId = autorId,
                        Descricao = request.Descricao,
                        LinkAcao = request.LinkAcao,
                        TextoAcao = request.TextoAcao,
                        TipoEvento = request.TipoEvento
                    };
                    await _eventosRepository.CreateEvento(evento);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
                }
            }

            public async Task<IActionResult> DeleteEvento(Guid id)
            {

                try
                {
                    var evento = await _eventosRepository.GetEventoById(id);
                
                    if (evento == null)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                    }
                    _eventosRepository.DeleteEvento(id);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
                }
            }


            public async Task<IActionResult> GetEventoById(Guid id)
            {
                try
                {
                    var evento = await _eventosRepository.GetEventoById(id);

                    
                    if (evento == null) 
                        return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { evento = evento}));
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
                }
            }

            // TODO: Criar RequestUpdateEvento ao invés de usar RequestEvento e colocar tudo menos AutorId
            public async Task<IActionResult> UpdateEvento(Guid id, RequestEvento request)
            {
                try
                {
                    var eventoQuery = await _eventosRepository.GetEventoById(id);
                    if (eventoQuery == null)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                    }

                    var evento = new Evento()
                    {
                        Titulo = request.Titulo,
                        TextoAcao = request.TextoAcao,
                        Descricao = request.Descricao,
                        LinkAcao = request.LinkAcao,
                        TipoEvento = request.TipoEvento,
                        Data = request.Data,
                    };
                    
                    await _eventosRepository.UpdateEvento(id, evento);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { evento = request }));
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
                }
            }
    }
} 

