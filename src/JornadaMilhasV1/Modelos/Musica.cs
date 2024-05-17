namespace JornadaMilhas.Modelos
{
    public class Musica
    {
        private string? artista;
        private int? anoLancamento;

        public Musica(string nome)
        {
            Nome = nome;
        }

        public string Nome { get; set; }
        public int Id { get; set; }
        
        public string? Artista
        {
            get => artista;
            set
            {
                if (artista == null)
                {
                    artista = "Artista desconhecido";
                }
                else
                {
                    artista = value;
                }
            }
        }

        public int? AnoLancamento
        {
            get => anoLancamento;
            set
            {
                if (value <= 0)
                {
                    anoLancamento = null;
                }
                else
                {
                    anoLancamento = value;
                }
            }
        }

        public void ExibirFichaTecnica()
        {
            Console.WriteLine($"Nome: {Nome}");

        }

        public override string ToString()
        {
            return @$"Id: {Id} Nome: {Nome}";
        }
    }
}