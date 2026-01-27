using DaccApi.Model;
using DaccApi.Infrastructure.Repositories.Eventos;
using DaccApi.Helpers;
using DaccApi.Model.Responses;
using DaccApi.Responses;
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
                var eventos = await _eventosRepository.GetAllAsync();

                if (eventos.Count == 0)
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Evento>()));
                var response = eventos.Select(evento => new ResponseEvento(evento));
                return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK, new { events = response }));
            }
            catch (Exception ex)
            {
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, "Erro ao obter eventos! " + ex.Message);
            }
        }
        
        public async Task<IActionResult> CreateEvento(Guid autorId, RequestEvento request)
            {
                try
                {
                    if (
                        String.IsNullOrWhiteSpace(request.Title) ||
                        String.IsNullOrWhiteSpace(request.Description) ||
                        String.IsNullOrWhiteSpace(request.EventType) ||
                        String.IsNullOrWhiteSpace(request.ActionText)||
                        String.IsNullOrWhiteSpace(request.ActionLink)
                        )
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                    }

                    var evento = new Evento()
                    {
                        Id = Guid.NewGuid(),
                        Titulo = request.Title,
                        AutorId = autorId,
                        Descricao = request.Description,
                        LinkAcao = request.ActionLink,
                        TextoAcao = request.ActionText,
                        TipoEvento = request.EventType,
                        Data = request.Date
                    };

                    await _eventosRepository.CreateAsync(evento);
                    
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED.WithData(new ResponseEvento(evento)));
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
                    var evento = await _eventosRepository.GetByIdAsync(id);
                
                    if (evento == null)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.RESOURCE_NOT_FOUND);
                    }
                    await _eventosRepository.DeleteAsync(id);

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
                    var evento = await _eventosRepository.GetByIdAsync(id);

                    
                    if (evento == null) 
                        return ResponseHelper.CreateSuccessResponse(ResponseSuccess.NO_CONTENT.WithData(new List<Evento>()));
                    var response = new ResponseEvento(evento);
                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.WithData(ResponseSuccess.OK,
                        new { @event = response }));
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
                    var eventoQuery = await _eventosRepository.GetByIdAsync(id);
                    if (eventoQuery == null)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                    }

                    eventoQuery.Titulo = request.Title;
                    eventoQuery.TextoAcao = request.ActionText;
                    eventoQuery.Descricao = request.Description;
                    eventoQuery.LinkAcao = request.ActionLink;
                    eventoQuery.TipoEvento = request.EventType;
                    eventoQuery.Data = request.Date;
                    
                    await _eventosRepository.UpdateAsync(id, eventoQuery);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK);
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.Message);
                }
            }
    }
} 
