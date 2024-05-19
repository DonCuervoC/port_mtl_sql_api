using API_port_montreal.Data;
using API_port_montreal.Models;
using API_port_montreal.Repository.IRepository;

namespace API_port_montreal.Repository
{

    /*
    Pourquoi le modèle du référentiel ? 
    Parce qu'il a une couche d'abstraction des données, avant d'entrer dans 
    le contrôleur sur EntityFramework et de faire un appel à une table pour 
    obtenir les données, il fournira une couche, un intermédiaire, 
    les avantages sont qu'il minimise la nécessité de dupliquer la logique de l'application,
    l'évolutivité et l'optimisation future, permet de recycler le code, 
    sépare l'application des frameworks persistants tels que entityFramework.
     */

    public class DepartsRepository : IDepartsRepository
    {
        // Contexte de connexion à la base de données avec Entity Framework
        private readonly ApplicationDbContext _db;

        public DepartsRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Fonctions de base pour vérifier les données et pour pouvoir effectuer le CRUD
        public bool CreateDeparts(Departs departs)
        {
            departs.DateHeureDepart = DateTime.Now;
            _db.Departs.Add(departs);
            return Save();
        }

        public bool DeleteDeparts(Departs departs)
        {
            _db.Departs.Remove(departs);
            return Save();
        }

        public bool ExistDepartsById(int id)
        {
            bool value = _db.Departs.Any(c => c.Id == id);
            return value;
        }

        public ICollection<Departs> GetAllDeparts()
        {
            return _db.Departs.OrderBy(dp => dp.Id).ToList();
        }


        public Departs GetDeparts(int DepartsId)
        {
            return _db.Departs.FirstOrDefault(c => c.Id == DepartsId);
        }

        public bool UpdateDeparts(Departs departs)
        {
            departs.DateHeureDepart = DateTime.Now;
            _db.Departs.Update(departs);
            return Save();
        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }
    }
}
