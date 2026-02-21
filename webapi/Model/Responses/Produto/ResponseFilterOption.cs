namespace DaccApi.Model.Responses.Produto
{
    /// <summary>
    /// Representa uma opção de filtro (tamanho, cor, etc) para o frontend.
    /// </summary>
    public class ResponseFilterOption
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public ResponseFilterOption(string value)
        {
            Label = value;
            Value = value;
        }

        public ResponseFilterOption()
        {
            Label = string.Empty;
            Value = string.Empty;
        }
    }
}
