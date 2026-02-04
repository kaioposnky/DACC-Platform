using DaccApi.Model.Objects;

namespace DaccApi.Model.Responses
{
    /// <summary>
    /// Representa a resposta de uma diretoria.
    /// </summary>
    public class ResponseDiretoria
    {
        /// <summary>
        /// Obtém ou define o ID da diretoria.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Obtém ou define o nome da diretoria.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Obtém ou define a descrição da diretoria.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Construtor para mapear de uma entidade Diretoria.
        /// </summary>
        /// <param name="diretoria">A entidade Diretoria de origem.</param>
        public ResponseDiretoria(Diretoria diretoria)
        {
            Id = diretoria.Id;
            Name = diretoria.Nome;
            Description = diretoria.Descricao;
        }

        /// <summary>
        /// Construtor sem parâmetros para deserialização
        /// </summary>
        public ResponseDiretoria() { }
    }
}
