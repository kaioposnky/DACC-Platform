using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;

namespace DaccApi.Infrastructure.Repositories.Anuncio

{
    public class AnuncioRepository : IAnuncioRepository
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public AnuncioRepository(IRepositoryDapper repositoryDapper)
        {
            _repositoryDapper = repositoryDapper;
        }

        public async Task<List<Model.Anuncio>> GetAllAnuncio()
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("GetAllAnuncio");

                var queryResult = await _repositoryDapper.QueryAsync<Model.Anuncio>(sql);

                var anuncios = queryResult.ToList();
                return anuncios;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter todos os anuncios" + ex.Message);
            }
            
        }
        
        
        public async Task<Model.Anuncio> GetAnuncioById(Guid id)
        {
            try
            {
                var sql =  _repositoryDapper.GetQueryNamed("GetAnuncioById");
                
                var param = new { id = id };

                var queryResult = await _repositoryDapper.QueryAsync<Model.Anuncio>(sql,param);

                var anuncio = queryResult.FirstOrDefault();

                return anuncio;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter anuncio." + ex.Message);
            }
        }
        
        
        
        
        
        public async Task CreateAnuncio(RequestAnuncio anuncio)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("CreateAnuncio");

                await _repositoryDapper.ExecuteAsync(sql, anuncio);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao criar anuncio." + ex.Message);
            }
        }
        
        
        public async Task DeleteAnuncio(Guid id)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("DeleteAnuncio");
                var param = new { id = id };
                await _repositoryDapper.ExecuteAsync(sql, param);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar anuncio." + ex.Message);
            }
        }
        
        
        public async Task UpdateAnuncio(Guid id, Model.Anuncio anuncio)
        {
            try
            {
                var sql = _repositoryDapper.GetQueryNamed("UpdateAnuncio");
                var param = new
                {
                    id = id,
                    Titulo = anuncio.Titulo,
                    Conteudo = anuncio.Conteudo,
                    TipoAnuncio = anuncio.TipoAnuncio,
                    ImagemUrl = anuncio.ImagemUrl,
                    ImagemAlt = anuncio.ImagemAlt,
                    Ativo = anuncio.Ativo,
                    AutorId = anuncio.AutorId
                };
                await _repositoryDapper.ExecuteAsync(sql, param);
            
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar anuncio." + ex.Message);
            };
        }
        
        
        
    }
}

