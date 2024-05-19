using API_port_montreal.Models;

namespace API_port_montreal.Repository.IRepository
{
    public interface IDepartsRepository
    {
        // Déclaration des méthodes dans une interface pour les opérations CRUD.

        ICollection<Departs> GetAllDeparts();

        Departs GetDeparts(int DepartsId);

        bool ExistDepartsById(int id);

        bool CreateDeparts(Departs departs);

        bool UpdateDeparts(Departs departs);

        bool DeleteDeparts(Departs departs);

        bool Save();
    }
}
