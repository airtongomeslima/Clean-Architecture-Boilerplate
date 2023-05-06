using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebAPI.Application.Interfaces;
using WebAPI.Application.Mapper.Profiles;
using WebAPI.Application.Services;
using WebAPI.Domain.Interface;
using WebAPI.Infra.Data.Repository;

namespace WebAPI.Infra.IoC
{

    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            IServiceProvider provider = services.BuildServiceProvider();

            //AutoMapper
            services.AddSingleton<IMapper>(r => {
                var mapperConfiguration = new MapperConfiguration(mc =>
                {
                    mc.AddProfile<PessoaProfile>();
                    mc.AddProfile<TelefoneProfile>();
                    mc.AddProfile<EnderecoProfile>();
                });

                return mapperConfiguration.CreateMapper();
            });


            //Application Layer
            services.AddScoped<IPessoaService, PessoaService>();

            //Data Layer
            services.AddScoped<IPessoaRepository, PessoaRepository>();
            services.AddScoped<ITelefoneRepository, TelefoneRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
        }
    }
}
