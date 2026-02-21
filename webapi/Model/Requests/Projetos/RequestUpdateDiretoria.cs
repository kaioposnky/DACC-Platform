using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests.Projetos
{
    /// <summary>
    /// Request para atualizar uma diretoria existente.
    /// </summary>
    public class RequestUpdateDiretoria
    {
        /// <summary>
        /// Nome da diretoria.
        /// </summary>
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string? Name { get; set; }

        /// <summary>
        /// Descrição da diretoria.
        /// </summary>
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Description { get; set; }
    }
}
