using System.ComponentModel.DataAnnotations;

namespace ServiceDesk.Api.DTOs;

public class CriarChamadoRequest
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
}