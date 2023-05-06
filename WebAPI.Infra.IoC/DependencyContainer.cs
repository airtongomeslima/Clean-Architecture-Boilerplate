using AutoMapper;
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
            //AutoMapper
            services.AddSingleton(provider =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<PessoaProfile>();
                    // adicione outros perfis, se houver
                });

                return config.CreateMapper();
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
