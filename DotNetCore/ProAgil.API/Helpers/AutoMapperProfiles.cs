using System.Linq;
using AutoMapper;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {

        public AutoMapperProfiles(){
            CreateMap<Evento, EventoDto>()
            .ForMember(dto => dto.Palestrantes, opt => {
                opt.MapFrom(bo => bo.PalestranteEventos.Select(x=>x.Palestrante).ToList());
            }).ReverseMap();
            CreateMap<Palestrante, PalestranteDto>()
            .ForMember(dto => dto.Eventos, opt=>{
                opt.MapFrom(bo => bo.PalestranteEventos.Select(x=>x.Evento).ToList());
            }).ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
        
    }
}