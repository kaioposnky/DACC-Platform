namespace DaccApi.Enum.Produtos;

/// <summary>
/// Contém enums relacionados a produtos.
/// </summary>
public class ProdutosEnum
{
    
    /// <summary>
    /// Enum para obter o Tamanho do produto em forma de String
    /// </summary>
    public enum ProdutosTamanho
    {
        /// <summary>
        /// Tamanho não especificado.
        /// </summary>
        Nenhum = 0,
        /// <summary>
        /// Tamanho extra extra pequeno.
        /// </summary>
        PP,
        /// <summary>
        /// Tamanho pequeno.
        /// </summary>
        P,
        /// <summary>
        /// Tamanho médio.
        /// </summary>
        M,
        /// <summary>
        /// Tamanho grande.
        /// </summary>
        G,
        /// <summary>
        /// Tamanho extra grande.
        /// </summary>
        GG,
        /// <summary>
        /// Tamanho extra extra grande.
        /// </summary>
        XG,
        /// <summary>
        /// Variação de tamanho 'Pequeno'.
        /// </summary>
        Pequeno,
        /// <summary>
        /// Variação de tamanho 'Médio'.
        /// </summary>
        Medio,
        /// <summary>
        /// Variação de tamanho 'Grande'.
        /// </summary>
        Grande,
    }
}