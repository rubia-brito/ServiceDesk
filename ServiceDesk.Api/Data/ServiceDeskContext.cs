using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Data;

public class ServiceDeskContext : DbContext
{
    public ServiceDeskContext(DbContextOptions<ServiceDeskContext> options)
        : base(options)
    {
    }

    public DbSet<Chamado> Chamados { get; set; }
}