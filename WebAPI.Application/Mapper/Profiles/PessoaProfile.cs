using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.ViewModels;
using WebAPI.Domain.Model;
using AutoMapper;

namespace WebAPI.Application.Mapper.Profiles
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile()
        {
            CreateMap<Pessoa, PessoaViewModel>()
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));

            CreateMap<PessoaViewModel, Pessoa>()
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.Endereco))
                .ForMember(dest => dest.Telefones, opt => opt.MapFrom(src => src.Telefones));
        }
    }
}
