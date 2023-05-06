using Dapper;
using WebAPI.Domain.Interface;
using WebAPI.Domain.Model;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace WebAPI.Infra.Data.Repository
{
    public class EnderecoRepository : BaseRepository, IEnderecoRepository
    {
        readonly SqlConnection cnn = DbConnection();

        public EnderecoRepository()
        {
            cnn.Open();
        }

        public int Count()
        {
            return cnn.Query<int>($"SELECT COUNT(1) FROM [Endereco];").FirstOrDefault();
        }

        public int Create(Endereco entity)
        {
            return cnn.QuerySingleOrDefault<int>(
                    @"INSERT INTO [Endereco]([Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP]) 
                            VALUES(@Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP);
                ", entity);
        }

        public int Create(IEnumerable<Endereco> entities)
        {
            return cnn.Execute(
                    @"INSERT INTO [Endereco]([Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP]) 
                            VALUES(@Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP;
                ", entities);
        }

        public bool CreateOrUpdate(Endereco entity)
        {
            var result = cnn.Execute(
                    @"INSERT INTO [Endereco]([Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP]) 
                            VALUES(@Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP;
                        ON CONFLICT(ID) DO UPDATE SET
                            @Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP
                        WHERE
                            Id=@Id
                ", entity);

            return result > 0;
        }

        public int CreateOrUpdate(IEnumerable<Endereco> entities)
        {
            return cnn.Execute(
                    @"INSERT INTO [Endereco]([Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP]) 
                            VALUES(@Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP;
                        ON CONFLICT(ID) DO UPDATE SET
                            @Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP
                        WHERE
                            Id=@Id
                ", entities);
        }

        public void CreateTable()
        {
            var result = cnn.Execute(
                @"CREATE TABLE [dbo].[Endereco](
	                [Id] [int] IDENTITY(1,1) False,[Logradouro] [varchar]  False,[Numero] [varchar]  True,[Bairro] [varchar]  True,[Cidade] [varchar]  True,[UF] [char]  True,[CEP] [char]  True;
                 CONSTRAINT [PK_Endereco] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                ) ON [PRIMARY]
                GO");
        }

        public void Delete(Endereco entity)
        {
            cnn.Execute($"DELETE FROM Endereco WHERE Id equals {entity.Id}");
        }

        public int DeleteBy(Expression<Func<Endereco, bool>> predicate)
        {
            return cnn.Query<int>($"DELETE FROM [Endereco] WHERE {ConvertExpressionToSql(predicate.Body)};").FirstOrDefault();
        }

        public void Dispose()
        {
            cnn.Dispose();
        }

        public bool DropCollection(string collectionName)
        {
            throw new NotImplementedException();
        }

        public Endereco[] FindAll()
        {
            return cnn.Query<Endereco>(@"SELECT [Id],[Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP] FROM [Endereco]").ToArray();
        }

        public Endereco[] FindBy(Expression<Func<Endereco, bool>> predicate)
        {
            return cnn.Query<Endereco>($"SELECT [Id],[Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP] FROM [Endereco] WHERE {ConvertExpressionToSql(predicate.Body)};").ToArray();
        }

        public Endereco FindById(int id)
        {
            return cnn.Query<Endereco>($"SELECT [Id],[Logradouro],[Numero],[Bairro],[Cidade],[UF],[CEP] FROM [Endereco] WHERE Id equals {id};").FirstOrDefault();
        }

        public void Update(Endereco entity)
        {
            cnn.Execute(
                    @"UPDATE [Endereco] SET
                            @Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP
                        WHERE
                            Id=@Id
                ", entity);
        }

        public int Update(IEnumerable<Endereco> entities)
        {
            return cnn.Execute(
                    @"UPDATE [Endereco] SET
                            @Logradouro,@Numero,@Bairro,@Cidade,@UF,@CEP
                        WHERE
                            Id=@Id
                ", entities);
        }
    }
}
