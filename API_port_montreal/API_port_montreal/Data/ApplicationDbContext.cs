using API_port_montreal.Models;
using Microsoft.EntityFrameworkCore;

namespace API_port_montreal.Data
{
    /*
     // hérite du contexte DB pour pouvoir utiliser le codefirst (dans le cas où vous souhaitez modifier une 
        table ou ajouter une table à partir de l'API), les migrations, le chain tracking (suivi des migrations appliquées à la base de données).
    // Classe ApplicationDbContext : hérite de DbContext et représente une session avec la base de données qui permet 
        d'interagir avec elle à l'aide d'objets .NET.

    // ApplicationDbContext hérite de la classe DbContext fournie par Entity Framework Core. DbContext est la classe de base qui coordonne
    // Entity Framework pour un modèle de données. C'est la classe qui nous permet d'interagir avec la base de données à l'aide d'objets .NET.
     */
    public class ApplicationDbContext : DbContext
    {
        /*
          DbContextOptions<ApplicationDbContext> options : ce paramètre accepte une instance de DbContextOptions<ApplicationDbContext>.
          DbContextOptions est une classe qui contient la configuration nécessaire pour le DbContext, y compris des détails sur la chaîne de connexion,
          le fournisseur de base de données (par exemple SQL Server, SQLite) et d'autres paramètres tels que le suivi des modifications, 
          la gestion des requêtes, etc.

          base(options) : cette partie du constructeur appelle le constructeur de la classe de base (DbContext) et lui transmet les options reçues. 
          Cette étape est cruciale, car DbContext a besoin de cette configuration pour savoir comment se connecter à la base de données et comment se comporter.*/
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //Tous les modèles qui sont équivalents à une table dans la base de données sont groupés ici ...
        public DbSet<Arrivees> Arrivees { get; set; }
        public DbSet<Departs> Departs { get; set; }

    }
}
