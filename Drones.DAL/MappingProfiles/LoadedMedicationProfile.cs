using AutoMapper;

namespace Drones.DAL.MappingProfiles;

public class LoadedMedicationProfile : Profile
{
    public LoadedMedicationProfile()
    {
        CreateMap<LoadedMedication, Medication>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MedicationId))
            .ForMember(dest => dest.DroneId, opt => opt.MapFrom(src => src.DroneId))
            // Map other properties as needed
            .ReverseMap(); // If you need a reverse mapping
    }
}