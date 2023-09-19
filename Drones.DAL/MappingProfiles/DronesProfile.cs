using AutoMapper;
using Drones.DAL.DronesDTO;

namespace Drones.DAL.MappingProfiles;

public class DronesProfile : Profile
    {
        public DronesProfile()
        {
            
            CreateMap<Drone, DronesDto>().ReverseMap();
            //CreateMap<DronesDto, Drone>();

        }
    }
