namespace ServiceDesk.Api.Models;

public class Chamado
{
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public string Solicitante { get; set; } = string.Empty;

    public string Categoria { get; set; } = string.Empty;

    public string Prioridade { get; set; } = "Média";

    public string Status { get; set; } = "Aberto";

    public DateTime DataAbertura { get; set; } = DateTime.Now;

    public DateTime? DataFechamento { get; set; }
}