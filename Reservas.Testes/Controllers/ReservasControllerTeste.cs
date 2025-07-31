using System; // Caixinha com ferramentas básicas
using System.Collections.Generic; // Pra criar listinhas
using System.Linq; // Pra mexer em listas (aqui não usamos)
using System.Text; // Pra mexer com texto (não usamos)
using System.Threading.Tasks; // Pra fazer coisas ao mesmo tempo (não usamos)
using Moq; // Biblioteca que ajuda a fingir objetos (mocks)
using Reservas.Api.Interfaces; // Onde está a regra que a reserva segue (interface)
using Reservas.Testes.MockData; // Dados de mentira (mock) pras reservas
using Reservas.Api.Controllers; // O lugar do controlador que queremos testar
using Xunit; // Biblioteca usada pra fazer testes
using Microsoft.AspNetCore.Mvc; // Ajuda a usar resultados como Ok(), NotFound(), etc
using FluentAssertions;
using Reservas.Api.Models;
using Microsoft.AspNetCore.JsonPatch; // Deixa o "Afirmar" mais fácil de ler

namespace Reservas.Testes.Controllers // Nome da pastinha onde está o teste
{
    public class ReservasControllerTeste // A caixinha do teste
    {
        [Fact] // Diz que isso aqui é um teste
        public void GetTodasReservas_DeveRetornar200Status() // Nome do teste (o que ele faz)
        {
            // ARRANGE (Organizar): preparar tudo antes do teste
            var reservaService = new Mock<IReservaRepository>(); // Criamos um serviço de mentira
            reservaService.Setup(i => i.Reservas) // Quando chamarem .Reservas...
                          .Returns(ReservasMockData.GetReservas()); // ...devolve as reservas de brincadeira
            var sut = new ReservasController(reservaService.Object); // Criamos o controlador, usando o serviço de mentira

            // ACT (Agir): chamar a função que queremos testar
            var result = (OkObjectResult)sut.Get(); // Chamamos o método Get() e guardamos o resultado

            // ASSERT (Afirmar): ver se o resultado está certo
            result.StatusCode.Should().Be(200); // Verifica se o código de resposta é 200 (tudo certo)
        }

        [Fact]
        public void GetReservaPorId_DeveRetornar200_QuandoEncontrar()
        {
            // Arrange - preparar dados e objetos
            var reservaMock = ReservasMockData.GetReservas()[0]; // Pega a primeira reserva de mentira
            var reservaService = new Mock<IReservaRepository>(); // Cria serviço falso

            // Configura o mock: quando chamar ObterReservaPorId com o ID da reserva, retorna a reserva falsa
            reservaService.Setup(i => i[reservaMock.ReservaId])
                          .Returns(reservaMock);

            var sut = new ReservasController(reservaService.Object); // Cria controlador com serviço falso

            // Act - executa o método que queremos testar, passando o ID
            var result = sut.Get(reservaMock.ReservaId);

            // Assert - verifica se o resultado é status 200 (OK)
            var okResult = result.Result as OkObjectResult; // Pega o resultado dentro da resposta

            okResult.Should().NotBeNull();

            okResult.StatusCode.Should().Be(200); // Verifica se o código é 200 (tudo certo)

        }

        [Fact]
        public void GetReservaPorId_DeveRetornar404_QuandoNaoEncontrar()
        {
            // Arrange - preparar o mock para devolver null quando pedir a reserva com ID 999 (não existe)
            var reservaService = new Mock<IReservaRepository>();
            reservaService.Setup(i => i[999]).Returns((Reserva)null);
            var sut = new ReservasController(reservaService.Object);

            // Act - chamar o método Get passando o ID 999
            var result = sut.Get(999);

            // Assert - verificar se o resultado é NotFound (404), pois não encontrou a reserva
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void PostReserva_DeveRetornar201Status()
        {
            //Arrange - Organizar
            var novaReserva = new Reserva { Nome = "Teste" };
            var reservaService = new Mock<IReservaRepository>();
            reservaService.Setup(i => i.AddReserva(It.IsAny<Reserva>())).Returns(novaReserva);
            var sut = new ReservasController
                (reservaService.Object);

            //Act - Agir
            var result = sut.Post(novaReserva);


            //Assert - Afirmar
            var createdResult = result.Result as CreatedAtActionResult;
            createdResult.Should().NotBeNull();
            createdResult.StatusCode.Should().Be(201);
        }

        [Fact]
        public void PutReserrva_DeveRetornar200_QuandoAtualizada()
        {
            //Arrange - Organizar
            var reservaAtualizada = new Reserva { ReservaId = 1, Nome = "Atualizada" };
            var reservaService = new Mock<IReservaRepository> ();
            reservaService.Setup(i => i.UpdateReserva(It.IsAny<Reserva>())).Returns(reservaAtualizada);
            var sut = new ReservasController(reservaService.Object);

            //Act - Agir
            var result = sut.Put(reservaAtualizada);

            //Assert - Afirmar
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

        }

        [Fact]
        public void PatchReserva_DeveRetornar200_QuandoEncontrar()
        {
            //Arrange - Organizar
            var reservaOriginal = new Reserva { ReservaId = 1, Nome = "Original" };
            var patch = new JsonPatchDocument<Reserva>();
            patch.Replace(r => r.Nome, "Atualizado");

            var reservaService = new Mock<IReservaRepository> ();
            reservaService.Setup(i => i[1]).Returns(reservaOriginal);
            var sut = new ReservasController (reservaService.Object);

            //Act - Agir
            var result = sut.Patch(1, patch);

            //Assert - Afirmar
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void PatchReserva_DeveRetornar404_QuandoNaoEncontrar()
        {
        //Arrange - Organizar
            var patch = new JsonPatchDocument<Reserva>();
            var reservaService = new Mock <IReservaRepository> ();
            reservaService.Setup(i => i[999]).Returns((Reserva)null);
            var sut = new ReservasController(reservaService.Object);
        //Act - Agir
        var result = sut.Patch(999, patch);

        //Assert - Afirmar
        result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void DeleteReserva_DeveRetornar204()
        {
            //Arrange - Organizar
            var reservaService = new Mock<IReservaRepository> ();
            var sut = new ReservasController(reservaService.Object);

            //Act - Agir
            var result = sut.Delete(1);

            //Assert - Afirmar
            result.Should().BeOfType<NoContentResult>();
        }


    }

}
