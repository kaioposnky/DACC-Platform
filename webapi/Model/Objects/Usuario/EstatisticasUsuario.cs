namespace DaccApi.Model;

public class EstatisticasUsuario
{
    public long Orders { get; set; }
    public long Reviews { get; set; }
    public decimal AverageRating { get; set; }
    public string RegistryDate { get; set; }
}