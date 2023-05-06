using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Application.ViewModels;
using WebAPI.Domain.Model;

namespace WebAPI.Application.Mapper.Profiles
{
    public class TelefoneProfile : Profile
    {
        public TelefoneProfile()
        {
            CreateMap<Telefone, TelefoneViewModel>();
            CreateMap<TelefoneViewModel, Telefone>();
        }
    }
}
