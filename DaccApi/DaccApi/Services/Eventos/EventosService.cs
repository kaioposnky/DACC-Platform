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
                return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR + ex.Message, ex);
            }
        }
        
        
        
        
        public async Task<IActionResult> CreateEvento(RequestEvento evento)
            {
                try
                {
                    if (
                        String.IsNullOrWhiteSpace(evento.Titulo) ||
                        String.IsNullOrWhiteSpace(evento.Descricao) ||
                        String.IsNullOrWhiteSpace(evento.TipoEvento) ||
                        evento.AutorId == null ||
                        String.IsNullOrWhiteSpace(evento.TextoAcao)||
                        String.IsNullOrWhiteSpace(evento.LinkAcao)
                        )
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                    }
                
                    await _eventosRepository.CreateEvento(evento);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.CREATED);
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.StackTrace);
                }
            }

            public async Task<IActionResult> DeleteEvento(int id)
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
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR);
                }
            }


            public async Task<IActionResult> GetEventoById(int id)
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
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR,ex.StackTrace);
                }
            }

            public async Task<IActionResult> UpdateEvento(int id, RequestEvento evento)
            {
                try
                {
                    var eventoQuery = await _eventosRepository.GetEventoById(id);
                    if (eventoQuery == null)
                    {
                        return ResponseHelper.CreateErrorResponse(ResponseError.BAD_REQUEST);
                    }

                   await _eventosRepository.UpdateEvento(id, evento);

                    return ResponseHelper.CreateSuccessResponse(ResponseSuccess.OK.WithData(new { evento = evento }));
                }
                catch (Exception ex)
                {
                    return ResponseHelper.CreateErrorResponse(ResponseError.INTERNAL_SERVER_ERROR, ex.ToString());
                }
            }
    }
} 

