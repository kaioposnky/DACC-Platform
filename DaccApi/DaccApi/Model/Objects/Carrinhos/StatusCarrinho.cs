namespace DaccApi.Model
{
    public class StatusCarrinho
    {
        private string Value { get; }

        private StatusCarrinho(string value)
        {
            this.Value = value;
        }

        public static readonly StatusCarrinho Ativo = new StatusCarrinho("ativo");
        public static readonly StatusCarrinho Finalizado = new StatusCarrinho("finalizado");
        public static readonly StatusCarrinho Abandonado = new StatusCarrinho("abandonado");

        public override string ToString()
        {
            return Value;
        }
    }
}

