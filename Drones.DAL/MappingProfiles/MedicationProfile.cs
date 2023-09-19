using AutoMapper;
using Drones.DAL.DronesDTO;

namespace Drones.DAL.MappingProfiles;

public class MedicationProfile : Profile
{
    public MedicationProfile()
    {

        CreateMap<Medication, MedicationDto>().ReverseMap();
    }
}