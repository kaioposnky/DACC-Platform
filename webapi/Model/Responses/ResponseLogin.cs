namespace DaccApi.Model.Responses;

public class ResponseLogin
{
    public required string AcessToken { get; set;  }
    public required string RefreshToken { get; set;  }
    public required long ExpiresIn { get; set; }
    public required ResponseUsuario User { get; set;  }
}