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
    public class TelefoneRepository : BaseRepository, ITelefoneRepository
    {
        readonly SqlConnection cnn = DbConnection();

        public TelefoneRepository()
        {
            cnn.Open();
        }

        public int Count()
        {
            return cnn.Query<int>($"SELECT COUNT(1) FROM [Telefone];").FirstOrDefault();
        }

        public int Create(Telefone entity)
        {
            return cnn.QuerySingleOrDefault<int>(
                    @"INSERT INTO [Telefone]([IdPessoa],[DDD],[Numero]) 
                            VALUES(@IdPessoa,@DDD,@Numero);
                ", entity);
        }

        public int Create(IEnumerable<Telefone> entities)
        {
            return cnn.Execute(
                    @"INSERT INTO [Telefone]([IdPessoa],[DDD],[Numero]) 
                            VALUES(@IdPessoa,@DDD,@Numero;
                ", entities);
        }

        public bool CreateOrUpdate(Telefone entity)
        {
            var result = cnn.Execute(
                    @"INSERT INTO [Telefone]([IdPessoa],[DDD],[Numero]) 
                            VALUES(@IdPessoa,@DDD,@Numero;
                        ON CONFLICT(ID) DO UPDATE SET
                            @IdPessoa,@DDD,@Numero
                        WHERE
                            Id=@Id
                ", entity);

            return result > 0;
        }

        public int CreateOrUpdate(IEnumerable<Telefone> entities)
        {
            return cnn.Execute(
                    @"INSERT INTO [Telefone]([IdPessoa],[DDD],[Numero]) 
                            VALUES(@IdPessoa,@DDD,@Numero;
                        ON CONFLICT(ID) DO UPDATE SET
                            @IdPessoa,@DDD,@Numero
                        WHERE
                            Id=@Id
                ", entities);
        }

        public void CreateTable()
        {
            var result = cnn.Execute(
                @"CREATE TABLE [dbo].[Telefone](
	                [Id] [int] IDENTITY(1,1) False,[IdPessoa] [int]  False,[DDD] [char]  False,[Numero] [varchar]  False;
                 CONSTRAINT [PK_Telefone] PRIMARY KEY CLUSTERED 
                (
	                [Id] ASC
                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
                ) ON [PRIMARY]
                GO");
        }

        public void Delete(Telefone entity)
        {
            if(FindById(entity.Id) != null)
                cnn.Execute($"DELETE FROM Telefone WHERE Id = {entity.Id}");
        }

        public int DeleteBy(Expression<Func<Telefone, bool>> predicate)
        {
            return cnn.Query<int>($"DELETE FROM [Telefone] WHERE {ConvertExpressionToSql(predicate.Body)};").FirstOrDefault();
        }

        public void Dispose()
        {
            cnn.Dispose();
        }

        public bool DropCollection(string collectionName)
        {
            throw new NotImplementedException();
        }

        public Telefone[] FindAll(int pageNumber = 1, int pageSize = 25, string orderBy = "Id", string order = "asc")
        {
            int offset = (pageNumber - 1) * pageSize;
            return cnn.Query<Telefone>(
                    $@"SELECT [Id],[IdPessoa],[DDD],[Numero] 
                        FROM [Telefone] 
                        ORDER BY [{ValidateOrderBy<Telefone>(orderBy)}] {ValidateOrderByDirection(order)}
                        OFFSET @offset ROWS FETCH NEXT @pageSize ROWS ONLY",
                    new { offset, pageSize }
                ).ToArray();
        }

        public Telefone[] FindBy(Expression<Func<Telefone, bool>> predicate)
        {
            var sqlWhere = ConvertExpressionToSql(predicate.Body);
            var query = $"SELECT [Id],[IdPessoa],[DDD],[Numero] FROM [Telefone] WHERE {sqlWhere};";
            return cnn.Query<Telefone>(query).ToArray();
        }


        public Telefone FindById(int id)
        {
            return cnn.Query<Telefone>($"SELECT [Id],[IdPessoa],[DDD],[Numero] FROM [Telefone] WHERE Id equals {id};").FirstOrDefault();
        }

        public void Update(Telefone entity)
        {
            cnn.Execute(
                    @"UPDATE [Telefone] SET
                            @IdPessoa,@DDD,@Numero
                        WHERE
                            Id=@Id
                ", entity);
        }

        public int Update(IEnumerable<Telefone> entities)
        {
            return cnn.Execute(
                    @"UPDATE [Telefone] SET
                            [IdPessoa]=@IdPessoa,[DDD]=@DDD,[Numero]=@Numero
                        WHERE
                            Id=@Id
                ", entities);
        }
    }
}
