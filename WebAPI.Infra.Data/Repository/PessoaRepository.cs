using Dapper;
using WebAPI.Domain.Interface;
using WebAPI.Domain.Model;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System;
using static Dapper.SqlMapper;

namespace WebAPI.Infra.Data.Repository
{
    public class PessoaRepository : BaseRepository, IPessoaRepository
    {
        readonly SqlConnection cnn = DbConnection();
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly ITelefoneRepository _telefoneRepository;

        public PessoaRepository(IEnderecoRepository enderecoRepository, ITelefoneRepository telefoneRepository)
        {
            cnn.Open();
            _enderecoRepository = enderecoRepository;
            _telefoneRepository = telefoneRepository;
        }

        public int Count()
        {
            return cnn.Query<int>($"SELECT COUNT(1) FROM [Pessoa];").FirstOrDefault();
        }

        public int Create(Pessoa entity)
        {
            var idEndereco = _enderecoRepository.Create(entity.Endereco);
            entity.IdEndereco = idEndereco;


            var query = entity.IdPessoaResponsavel == 0 ?
                @"INSERT INTO [Pessoa]([IdEndereco],[Nome],[SobreNome],[Sexo],[Idade]) 
                    VALUES(@IdEndereco,@Nome,@SobreNome,@Sexo,@Idade);
                    SELECT SCOPE_IDENTITY()" :
                @"INSERT INTO [Pessoa]([IdEndereco],[IdPessoaResponsavel],[Nome],[SobreNome],[Sexo],[Idade]) 
                    VALUES(@IdEndereco,@IdPessoaResponsavel,@Nome,@SobreNome,@Sexo,@Idade);
                    SELECT SCOPE_IDENTITY()";
            var id = cnn.QuerySingleOrDefault<int>(query,
                new
                {
                    entity.IdEndereco,
                    IdPessoaResponsavel = (object)entity.IdPessoaResponsavel ?? DBNull.Value,
                    entity.Nome,
                    entity.SobreNome,
                    entity.Sexo,
                    entity.Idade
                });

            foreach (var telefone in entity.Telefones)
            {
                telefone.IdPessoa = id;
                _telefoneRepository.Create(telefone);
            }

            return id;

        }

        public int Create(IEnumerable<Pessoa> entities)
        {
            foreach (var entity in entities)
            {
                Create(entity);
            }

            return entities.Count();
        }

        public bool CreateOrUpdate(Pessoa entity)
        {
            var exists = FindById(entity.Id) != null;
            if (exists)
            {
                Update(entity);
            }
            else
            {
                Create(entity);
            }
            return !exists;
        }

        public int CreateOrUpdate(IEnumerable<Pessoa> entities)
        {
            var affectedRows = 0;
            foreach (var entity in entities)
            {
                if (CreateOrUpdate(entity))
                {
                    affectedRows++;
                }
            }
            return affectedRows;
        }

        public void CreateTable()
        {
            var result = cnn.Execute(
                @"CREATE TABLE [dbo].[Pessoa](
	                [Id] [int] IDENTITY(1,1) False,[IdEndereco] [int]  False,[IdPessoaResponsavel] [int]  True,[Nome] [varchar]  False,[SobreNome] [varchar]  True,[Sexo] [char]  True,[Idade] [int]  True;
                 CONSTRAINT [PK_Pessoa] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                ) ON [PRIMARY]
                GO");
        }

        public void Delete(Pessoa entity)
        {
            if (FindById(entity.Id) != null)
            {
                foreach (var telefone in entity.Telefones)
                {
                    _telefoneRepository.Delete(telefone);
                }
                cnn.Execute($"DELETE FROM Pessoa WHERE Id = @id", new { id = entity.Id });
                _enderecoRepository.Delete(entity.Endereco);
            }
        }

        public int DeleteBy(Expression<Func<Pessoa, bool>> predicate)
        {
            var toDelete = FindBy(predicate);

            foreach (var item in toDelete)
            {
                _enderecoRepository.Delete(item.Endereco);
                foreach (var telefone in item.Telefones)
                {
                    _telefoneRepository.Delete(telefone);
                }
                Delete(item);
            }

            return toDelete.Count();
        }

        public void Dispose()
        {
            cnn.Dispose();
        }

        public bool DropCollection(string collectionName)
        {
            throw new NotImplementedException();
        }

        public Pessoa[] FindAll(int pageNumber = 1, int pageSize = 25, string orderBy = "Id", string order = "asc")
        {
            int offset = (pageNumber - 1) * pageSize;
            var result = cnn.Query<Pessoa>(
                @$"SELECT [Id],[IdEndereco],[IdPessoaResponsavel],[Nome],[SobreNome],[Sexo],[Idade] 
                    FROM [Pessoa] 
                    ORDER BY [{ValidateOrderBy<Pessoa>(orderBy)}] {ValidateOrderByDirection(order)}
                    OFFSET @offset ROWS 
                    FETCH NEXT @pageSize ROWS ONLY",
                new { offset, pageSize }
            ).ToArray();

            foreach (var item in result)
            {
                int idPessoa = item.Id;
                item.Telefones = _telefoneRepository.FindBy(t => t.IdPessoa == idPessoa).ToList();
                item.Endereco = _enderecoRepository.FindById(item.IdEndereco);
            }

            return result;
        }

        public Pessoa[] FindBy(Expression<Func<Pessoa, bool>> predicate)
        {
            var sql = $"SELECT [Id],[IdEndereco],[IdPessoaResponsavel],[Nome],[SobreNome],[Sexo],[Idade] FROM [Pessoa] WHERE {ConvertExpressionToSql(predicate.Body)}";
            var result = cnn.Query<Pessoa>(sql, predicate.Parameters).ToArray();

            foreach (var item in result)
            {
                int idPessoa = item.Id;
                item.Telefones = _telefoneRepository.FindBy(t => t.IdPessoa == idPessoa).ToList();
                item.Endereco = _enderecoRepository.FindById(item.IdEndereco);
            }
            return result;
        }

        public Pessoa FindById(int id)
        {
            var sql = $"SELECT [Id],[IdEndereco],[IdPessoaResponsavel],[Nome],[SobreNome],[Sexo],[Idade] FROM [Pessoa] WHERE id = @id";
            var result = cnn.Query<Pessoa>(sql, new { id }).FirstOrDefault();

            if (result == null) return null;
            int idPessoa = result.Id;
            result.Telefones = _telefoneRepository.FindBy(t => t.IdPessoa == idPessoa).ToList();
            result.Endereco = _enderecoRepository.FindById(result.IdEndereco);
            return result;
        }

        public void Update(Pessoa entity)
        {
            cnn.Execute(
                    @$"UPDATE [Pessoa] 
                        SET 
                            [IdEndereco]=@IdEndereco,
                            [IdPessoaResponsavel] = CASE 
                                                        WHEN @IdPessoaResponsavel = 0 
                                                            THEN NULL 
                                                        ELSE @IdPessoaResponsavel 
                                                    END,
                            [Nome]=@Nome,
                            [SobreNome]=@SobreNome,
                            [Sexo]=@Sexo,
                            [Idade]=@Idade
                        WHERE Id=@Id
                ", entity);

            _enderecoRepository.Update(entity.Endereco);
            _telefoneRepository.Update(entity.Telefones);
        }

        public int Update(IEnumerable<Pessoa> entities)
        {
            var result = cnn.Execute(
                    @"UPDATE [Pessoa] 
                        SET 
                            [IdEndereco]=@IdEndereco,
                            [IdPessoaResponsavel] = CASE 
                                                        WHEN @IdPessoaResponsavel = 0 
                                                            THEN NULL 
                                                        ELSE @IdPessoaResponsavel 
                                                    END,
                            [Nome]=@Nome,
                            [SobreNome]=@SobreNome,
                            [Sexo]=@Sexo,
                            [Idade]=@Idade
                        WHERE Id=@Id
                ", entities);

            foreach (var entity in entities)
            {
                _enderecoRepository.Update(entity.Endereco);
                _telefoneRepository.Update(entity.Telefones);
            }

            return result;
        }
    }
}
