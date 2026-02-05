using System.ComponentModel.DataAnnotations;

namespace DaccApi.Model.Requests.Projetos
{
    /// <summary>
    /// Request para criar uma nova diretoria.
    /// </summary>
    public class RequestCreateDiretoria
    {
        /// <summary>
        /// Nome da diretoria.
        /// </summary>
        [Required(ErrorMessage = "O nome da diretoria é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da diretoria.
        /// </summary>
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Description { get; set; }
    }
}
