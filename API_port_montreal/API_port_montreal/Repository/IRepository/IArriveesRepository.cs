using API_port_montreal.Models;

namespace API_port_montreal.Repository.IRepository
{
    public interface IArriveesRepository
    {
        // Déclaration des méthodes dans une interface pour les opérations CRUD.

        ICollection<Arrivees> GetArrivees();

        Arrivees GetArrivee(int arriveeId);

        bool ExistArriveeByName(string nom);

        bool ExistArriveeById(int id);

        bool CreateArrivee(Arrivees arrivees);

        bool UpdateArrivee(Arrivees arrivees);

        bool DeleteArrivee(Arrivees arrivees);

        bool Save();

    }
}
