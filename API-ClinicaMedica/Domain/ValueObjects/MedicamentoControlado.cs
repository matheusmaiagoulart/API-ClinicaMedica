namespace API_ClinicaMedica.Domain.ValueObjects
{
    public class MedicamentoControlado
    {
        public string Nome { get; private set; }
        public string Dosagem { get; private set; }
        public string Frequencia { get; private set; }
        public string Observacoes { get; private set; }

        protected MedicamentoControlado() { }

        public MedicamentoControlado(string nome, string dosagem, string frequencia, string observacoes = null)
        {
            Nome = nome;
            Dosagem = dosagem;
            Frequencia = frequencia;
            Observacoes = observacoes;
        }
        
        
    }
}
