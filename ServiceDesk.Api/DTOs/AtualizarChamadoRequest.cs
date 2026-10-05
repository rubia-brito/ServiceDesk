using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.Api.DTOs;

public class AtualizarChamadoRequest
{
    [Required]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    [StringLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Solicitante { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Categoria { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Prioridade { get; set; } = "Média";

    [Required]
    [StringLength(30)]
    public string Status { get; set; } = "Aberto";
}