using API_port_montreal.Models;
using API_port_montreal.Models.Dtos;
using AutoMapper;

namespace API_port_montreal.Mappers
{
    /* Ici, nous établissons un lien entre les Dtos et les classes
     * qui représentent les tables dans la base de données afin d'ajouter 
     * une couche de sécurité supplémentaire à notre API.
     * 
     * En créant le Mapper, nous effectuons un casting entre les classes principales et les DTOS qui sont exposées.
     * 
     * Nous utilisons l'AutoMApper
     */

    public class MyMapper : Profile
    {
        public MyMapper()
        {
            CreateMap<Arrivees, ArriveesDto>().ReverseMap();
            CreateMap<Arrivees, CreateArriveeDto>().ReverseMap();
            CreateMap<Departs, DepartsDto>().ReverseMap();
            CreateMap<Departs, CreateDepartsDto>().ReverseMap();
        }
    }
}
