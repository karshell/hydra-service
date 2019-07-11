using AutoMapper.Configuration;
using Services.Hydra.DTO;
using Services.Hydra.WebApi.Models;

namespace Services.Hydra.WebApi
{
    public class MappingProfile : MapperConfigurationExpression
    {
        public MappingProfile()
        {
            CreateMap<ContainerFillStateDTO, ContainerFillState>();
            CreateMap<ContainerDTO, Container>();
        }
    }
}
