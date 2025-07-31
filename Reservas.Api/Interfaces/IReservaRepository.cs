using Reservas.Api.Models;

namespace Reservas.Api.Interfaces
{
    public interface IReservaRepository
    {
        IEnumerable<Reserva> Reservas { get; }
        Reserva this[int id] { get; }
        Reserva AddReserva(Reserva reserva);
        Reserva UpdateReserva(Reserva reserva);
        void DeleteReserva(int id);
    }
}
