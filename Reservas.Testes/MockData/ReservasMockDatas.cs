using System; // Pegamos ferramentas básicas
using System.Collections.Generic; // Usamos pra fazer listas
using System.Linq; // Ajuda a procurar coisas (aqui não usamos)
using System.Text; // Ajuda com textos (não usamos)
using System.Threading.Tasks; // Ajuda a fazer várias coisas ao mesmo tempo (não usamos)
using Reservas.Api.Models; // Traz o desenho da "Reserva"

namespace Reservas.Testes.MockData // Nome da nossa caixinha de dados de mentira
{
    public class ReservasMockData // Aqui guardamos reservas de faz de conta
    {
        public static List<Reserva> GetReservas() // Esse botão cria a listinha
        {
            return new List<Reserva>() // Aqui montamos 3 reservas só pra brincar/testar
            {
                new Reserva { ReservaId = 1, Nome = "Lula", InicioLocacao = "Sao Paulo", FimLocacao = "Rio de Janeiro" },
                new Reserva { ReservaId = 2, Nome = "Dilma", InicioLocacao = "China", FimLocacao = "Alasca" },
                new Reserva { ReservaId = 3, Nome = "Haddad", InicioLocacao = "Russia", FimLocacao = "Campinas" }
            };
        }
    }
}
