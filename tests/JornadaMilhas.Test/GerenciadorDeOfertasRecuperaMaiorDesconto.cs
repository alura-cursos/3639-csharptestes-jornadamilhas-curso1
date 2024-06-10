using Bogus;
using JornadaMilhasV1.Gerenciador;
using JornadaMilhasV1.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JornadaMilhas.Test;
public class GerenciadorDeOfertasRecuperaMaiorDesconto
{
    [Fact]
    public void RetornaOfertaNulaQuandoListaEstaVazia()
    {
        //arrange
        var lista = new List<OfertaViagem>();
        var gerenciador = new GerenciadorDeOfertas(lista);
        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");

        //act
        var oferta = gerenciador.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.Null(oferta);
    }

    [Fact]
    // destino = são paulo, desconto = 40, preco = 80
    public void RetornaOfertaEspecificaQuandoDestinoSaoPauloEDesconto40()
    {
        //arrange
        var fakerPeriodo = new Faker<Periodo>()
            .CustomInstantiator(f =>
            {
                DateTime dataInicio = f.Date.Soon();
                return new Periodo(dataInicio, dataInicio.AddDays(30));
            });

        var rota = new Rota("Curitiba", "São Paulo");

        var fakerOferta = new Faker<OfertaViagem>()
            .CustomInstantiator(f => new OfertaViagem (
                rota,
                fakerPeriodo.Generate(),
                100 * f.Random.Int(1, 100))
            )
            .RuleFor(o => o.Desconto, f => 40)
            .RuleFor(o => o.Ativa, f => true);

        var ofertaEscolhida = new OfertaViagem(rota, fakerPeriodo.Generate(), 80)
        {
            Desconto = 40,
            Ativa = true
        };

        var ofertaInativa = new OfertaViagem(rota, fakerPeriodo.Generate(), 70)
        {
            Desconto = 40,
            Ativa = false
        };


        var lista = fakerOferta.Generate(200);
        lista.Add(ofertaEscolhida);
        lista.Add(ofertaInativa);
        var gerenciador = new GerenciadorDeOfertas(lista);
        Func<OfertaViagem, bool> filtro = o => o.Rota.Destino.Equals("São Paulo");
        var precoEsperado = 40;

        //act
        var oferta = gerenciador.RecuperaMaiorDesconto(filtro);

        //assert
        Assert.NotNull(oferta);
        Assert.Equal(precoEsperado, oferta.Preco, 0.0001);
    }
}
