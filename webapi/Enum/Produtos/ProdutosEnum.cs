namespace DaccApi.Enum.Produtos;

public class ProdutosEnum
{
    
    /// <summary>
    /// Enum para obter o Tamanho do produto em forma de String
    /// Valores a seguir:
    /// 0 - Nenhum
    /// 1 - PP
    /// 2 - P
    /// 3 - M
    /// 4 - G
    /// 5 - GG
    /// 6 - XG
    /// 7 - Pequeno
    /// 8 - Medio
    /// 9 - Grande
    /// </summary>
    public enum ProdutosTamanho
    {
        Nenhum = 0,
        PP,
        P,
        M,
        G,
        GG,
        XG,
        Pequeno,
        Medio,
        Grande,
    }
}