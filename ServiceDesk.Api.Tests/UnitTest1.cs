using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceDesk.Api.Controllers;
using ServiceDesk.Api.Data;
using ServiceDesk.Api.DTOs;
using ServiceDesk.Api.Models;

namespace ServiceDesk.Api.Tests;

public class ChamadoTests
{
    [Fact]
    public void NovoChamado_DeveIniciarComStatusAberto()
    {
        // Arrange
        var chamado = new Chamado();

        // Act
        var status = chamado.Status;

        // Assert
        Assert.Equal("Aberto", status);
    }

    [Fact]
    public void NovoChamado_DeveTerDataDeAbertura()
    {
        // Arrange
        var antes = DateTime.Now;

        // Act
        var chamado = new Chamado();

        // Assert
        Assert.True(chamado.DataAbertura >= antes);
        Assert.True(chamado.DataAbertura <= DateTime.Now);
    }

    [Fact]
    public void NovoChamado_NaoDeveTerDataDeFechamento()
    {
        // Arrange
        var chamado = new Chamado();

        // Act
        var dataFechamento = chamado.DataFechamento;

        // Assert
        Assert.Null(dataFechamento);
    }

    [Fact]
    public async Task AtualizarChamado_ParaFechado_DevePreencherDataDeFechamento()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        var chamado = new Chamado
        {
            Titulo = "Computador não liga",
            Descricao = "Teste de fechamento",
            Solicitante = "Maria",
            Categoria = "Hardware",
            Prioridade = "Alta",
            Status = "Aberto"
        };

        context.Chamados.Add(chamado);
        await context.SaveChangesAsync();

        var controller = new ChamadosController(context);

        var request = new AtualizarChamadoRequest
        {
            Titulo = "Computador não liga",
            Descricao = "Teste de fechamento",
            Solicitante = "Maria",
            Categoria = "Hardware",
            Prioridade = "Alta",
            Status = "Fechado"
        };

        // Act
        await controller.Atualizar(chamado.Id, request);

        // Assert
        var chamadoAtualizado = await context.Chamados.FindAsync(chamado.Id);

        Assert.NotNull(chamadoAtualizado);
        Assert.Equal("Fechado", chamadoAtualizado.Status);
        Assert.NotNull(chamadoAtualizado.DataFechamento);
    }

    [Fact]
    public async Task AtualizarChamado_DeFechadoParaEmAndamento_DeveRemoverDataDeFechamento()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        var chamado = new Chamado
        {
            Titulo = "Computador não liga",
            Descricao = "Teste de reabertura",
            Solicitante = "Maria",
            Categoria = "Hardware",
            Prioridade = "Alta",
            Status = "Fechado",
            DataFechamento = DateTime.Now
        };

        context.Chamados.Add(chamado);
        await context.SaveChangesAsync();

        var controller = new ChamadosController(context);

        var request = new AtualizarChamadoRequest
        {
            Titulo = "Computador não liga",
            Descricao = "Teste de reabertura",
            Solicitante = "Maria",
            Categoria = "Hardware",
            Prioridade = "Alta",
            Status = "Em andamento"
        };

        // Act
        await controller.Atualizar(chamado.Id, request);

        // Assert
        var chamadoAtualizado = await context.Chamados.FindAsync(chamado.Id);

        Assert.NotNull(chamadoAtualizado);
        Assert.Equal("Em andamento", chamadoAtualizado.Status);
        Assert.Null(chamadoAtualizado.DataFechamento);
    }

    [Fact]
    public async Task ObterTodos_ComFiltroDeStatus_DeveRetornarApenasChamadosDoStatusInformado()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        context.Chamados.AddRange(
            new Chamado
            {
                Titulo = "Chamado fechado",
                Descricao = "Teste",
                Solicitante = "Maria",
                Categoria = "Hardware",
                Prioridade = "Alta",
                Status = "Fechado"
            },
            new Chamado
            {
                Titulo = "Chamado aberto",
                Descricao = "Teste",
                Solicitante = "João",
                Categoria = "Software",
                Prioridade = "Baixa",
                Status = "Aberto"
            }
        );

        await context.SaveChangesAsync();

        var controller = new ChamadosController(context);

        // Act
        var resultado = await controller.ObterTodos(
            "Fechado",
            null,
            null);

        var resposta = Assert.IsType<OkObjectResult>(resultado);
        var chamados = Assert.IsAssignableFrom<IEnumerable<Chamado>>(resposta.Value);

        // Assert
        var lista = chamados.ToList();

        Assert.Single(lista);
        Assert.Equal("Fechado", lista[0].Status);
    }

    [Fact]
    public async Task ObterTodos_ComFiltroDePrioridade_DeveRetornarApenasChamadosDaPrioridadeInformada()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        context.Chamados.AddRange(
            new Chamado
            {
                Titulo = "Chamado urgente",
                Descricao = "Teste",
                Solicitante = "Maria",
                Categoria = "Hardware",
                Prioridade = "Alta",
                Status = "Aberto"
            },
            new Chamado
            {
                Titulo = "Chamado normal",
                Descricao = "Teste",
                Solicitante = "João",
                Categoria = "Software",
                Prioridade = "Baixa",
                Status = "Aberto"
            }
        );

        await context.SaveChangesAsync();

        var controller = new ChamadosController(context);

        // Act
        var resultado = await controller.ObterTodos(
            null,
            "Alta",
            null);

        var resposta = Assert.IsType<OkObjectResult>(resultado);
        var chamados = Assert.IsAssignableFrom<IEnumerable<Chamado>>(resposta.Value);

        // Assert
        var lista = chamados.ToList();

        Assert.Single(lista);
        Assert.Equal("Alta", lista[0].Prioridade);
    }

    [Fact]
    public async Task ObterTodos_ComFiltroDeCategoria_DeveRetornarApenasChamadosDaCategoriaInformada()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        context.Chamados.AddRange(
            new Chamado
            {
                Titulo = "Computador não liga",
                Descricao = "Teste de hardware",
                Solicitante = "Maria",
                Categoria = "Hardware",
                Prioridade = "Alta",
                Status = "Aberto"
            },
            new Chamado
            {
                Titulo = "Sistema não abre",
                Descricao = "Teste de software",
                Solicitante = "João",
                Categoria = "Software",
                Prioridade = "Baixa",
                Status = "Aberto"
            }
        );

        await context.SaveChangesAsync();

        var controller = new ChamadosController(context);

        // Act
        var resultado = await controller.ObterTodos(
            null,
            null,
            "Hardware");

        var resposta = Assert.IsType<OkObjectResult>(resultado);
        var chamados = Assert.IsAssignableFrom<IEnumerable<Chamado>>(resposta.Value);

        // Assert
        var lista = chamados.ToList();

        Assert.Single(lista);
        Assert.Equal("Hardware", lista[0].Categoria);
    }

    [Fact]
    public async Task ObterTodos_ComFiltroDeStatusEPrioridade_DeveRetornarApenasChamadosQueAtendemAosDoisFiltros()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        context.Chamados.AddRange(
            new Chamado
            {
                Titulo = "Chamado fechado e urgente",
                Descricao = "Teste",
                Solicitante = "Maria",
                Categoria = "Hardware",
                Prioridade = "Alta",
                Status = "Fechado"
            },
            new Chamado
            {
                Titulo = "Chamado aberto e urgente",
                Descricao = "Teste",
                Solicitante = "João",
                Categoria = "Software",
                Prioridade = "Alta",
                Status = "Aberto"
            },
            new Chamado
            {
                Titulo = "Chamado fechado e normal",
                Descricao = "Teste",
                Solicitante = "Pedro",
                Categoria = "Hardware",
                Prioridade = "Baixa",
                Status = "Fechado"
            }
        );

        await context.SaveChangesAsync();

        var controller = new ChamadosController(context);

        // Act
        var resultado = await controller.ObterTodos(
            "Fechado",
            "Alta",
            null);

        var resposta = Assert.IsType<OkObjectResult>(resultado);
        var chamados = Assert.IsAssignableFrom<IEnumerable<Chamado>>(resposta.Value);

        // Assert
        var lista = chamados.ToList();

        Assert.Single(lista);
        Assert.Equal("Fechado", lista[0].Status);
        Assert.Equal("Alta", lista[0].Prioridade);
        Assert.Equal("Chamado fechado e urgente", lista[0].Titulo);
    }

    [Fact]
    public async Task ObterPorId_ComChamadoInexistente_DeveRetornarNotFound()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ServiceDeskContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using var context = new ServiceDeskContext(options);

        var controller = new ChamadosController(context);

        // Act
        var resultado = await controller.ObterPorId(999);

        // Assert
        Assert.IsType<NotFoundResult>(resultado);
    }
}