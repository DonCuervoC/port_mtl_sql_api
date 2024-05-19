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

    public class AriveesRepository : IArriveesRepository
    {
        // Contexte de connexion à la base de données avec Entity Framework
        private readonly ApplicationDbContext _db;

        public AriveesRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // Fonctions de base pour vérifier les données et pour pouvoir effectuer le CRUD

        public bool CreateArrivee(Arrivees arrivees)
        {
            arrivees.DateHeureArrivee = DateTime.Now;
            _db.Arrivees.Add(arrivees);
            return Save();
        }

        public bool DeleteArrivee(Arrivees arrivees)
        {
            _db.Arrivees.Remove(arrivees);  
            return Save();
        }

        public bool ExistArriveeById(int id)
        {
            bool value = _db.Arrivees.Any(ar => ar.Id == id);

            return value;
        }

        public bool ExistArriveeByName(string nom)
        {
            bool value = _db.Arrivees.Any(ar => ar.NomNavire.ToLower() == nom.ToLower());
            return value;
        }

        public Arrivees GetArrivee(int arriveeId)
        {
            return _db.Arrivees.FirstOrDefault(ar => ar.Id == arriveeId);
        }

        public ICollection<Arrivees> GetArrivees()
        {
            var result = new List<Arrivees>();

            result = _db.Arrivees.OrderBy(ar => ar.Id).ToList();
            return result;
        }

        public bool UpdateArrivee(Arrivees arrivees)
        {
            arrivees.DateHeureArrivee = DateTime.Now;
            _db.Arrivees.Update(arrivees);
            return Save();

        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0 ? true : false;
        }


    }
}
