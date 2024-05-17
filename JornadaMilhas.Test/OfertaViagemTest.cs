using JornadaMilhasV1.Modelos;
using System;
using System.Linq;
using Xunit;

namespace JornadaMilhas.Test
{
    public class OfertaViagemTest
    {
        [Theory]
        [InlineData("", null, "2024-01-01", "2024-01-02", 0, false)]
        [InlineData("OrigemTeste", "DestinoTeste", "2024-02-01", "2024-02-05", 100, true)]
        [InlineData(null, "S�o Paulo", "2024-01-01", "2024-01-02", -1, false)]
        [InlineData("Vit�ria", "S�o Paulo", "2024-01-01", "2024-01-01", 0, false)]
        [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", -500, false)]
        public void RetornaOfertaValidaQuandoDadosValidos(string origem, string destino, string dataIda, string dataVolta, double preco, bool validacao)
        {
            //arrange
            Rota rota = new Rota(origem, destino);
            Periodo periodo = new Periodo(DateTime.Parse(dataIda), DateTime.Parse(dataVolta));

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Equal(validacao, oferta.EhValido);
        }

        [Fact]
        public void OfertaComRotaNulaTest()
        {
            //arrange
            Rota rota = null;
            Periodo periodo = new Periodo(new DateTime(2024, 2, 1), new DateTime(2024, 2, 5));
            double preco = 100.0;

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Contains("A oferta de viagem n�o possui rota ou per�odo v�lidos.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }

        [Fact]
        public void OfertaComPeriodoInvalidoTest()
        {
            //arrange
            Rota rota = new Rota("OrigemTeste", "DestinoTeste");
            Periodo periodo = new Periodo(new DateTime(2024, 2, 5), new DateTime(2024, 2, 1));
            double preco = 100.0;

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Contains("Erro: Data de ida n�o pode ser maior que a data de volta.", oferta.Erros.Sumario);
            Assert.False(oferta.EhValido);
        }
        [Theory]
        [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", -500, false)]
        [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", -1, false)]
        [InlineData("Rio de Janeiro", "S�o Paulo", "2024-01-01", "2024-01-02", 0, false)]
        public void RetornaMensagemDeErroDePrecoInvalidoQuandoPrecoMenorQueZero(string origem, string destino, string dataIda, string dataVolta, double preco, bool validacao)
        {
            // arrange
            Rota rota = new Rota(origem, destino);
            Periodo periodo = new Periodo(DateTime.Parse(dataIda), DateTime.Parse(dataVolta));
            
            // act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            // assert
            Assert.Contains("O pre�o da oferta de viagem deve ser maior que zero.", oferta.Erros.Sumario);
        }

        [Fact]
        public void RetornaTresErrosDeValidacaoQuandoRotaPeriodoEPRecoSaoInvalidos()
        {
            //arrange
            int quantidadeEsperada = 3;
            Rota rota = null;
            Periodo periodo = new Periodo(new DateTime(2024, 6, 1), new DateTime(2024, 5, 10));
            double preco = -100;

            //act
            OfertaViagem oferta = new OfertaViagem(rota, periodo, preco);

            //assert
            Assert.Equal(quantidadeEsperada, oferta.Erros.Count());
        }
    }
}