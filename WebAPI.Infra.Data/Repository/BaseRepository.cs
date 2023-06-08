using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace WebAPI.Infra.Data.Repository
{
    public class BaseRepository
    {
        private protected static IConfiguration _configuration;

        public BaseRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static SqlConnection DbConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("Base"));
        }

        public static string ConvertExpressionToSql(Expression expression)
        {
            var builder = new StringBuilder();
            if (expression is BinaryExpression binaryExpression)
            {
                builder.Append("(");
                builder.Append(ConvertExpressionToSql(binaryExpression.Left));
                builder.Append(" ");
                builder.Append(GetSqlOperator(binaryExpression.NodeType));
                builder.Append(" ");
                builder.Append(ConvertExpressionToSql(binaryExpression.Right));
                builder.Append(")");
            }
            else if (expression is MemberExpression memberExpression)
            {
                if (memberExpression.Expression != null && memberExpression.Expression.NodeType == ExpressionType.Constant)
                {
                    var container = ((ConstantExpression)memberExpression.Expression).Value;
                    var value = ((FieldInfo)memberExpression.Member).GetValue(container);

                    if (value == null)
                        builder.Append("NULL");
                    else if (value is string)
                        builder.Append("'").Append(value).Append("'");
                    else
                        builder.Append(value);
                }
                else
                {
                    builder.Append(memberExpression.Member.Name);
                }
            }
            else if (expression is ConstantExpression constantExpression)
            {
                if (constantExpression.Value == null)
                    builder.Append("NULL");
                else if (constantExpression.Value is string)
                    builder.Append("'").Append(constantExpression.Value).Append("'");
                else
                    builder.Append(constantExpression.Value);
            }
            else if (expression is MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.Name == "StartsWith")
                {
                    builder.Append("(");
                    builder.Append(ConvertExpressionToSql(methodCallExpression.Object));
                    builder.Append(" LIKE '");
                    builder.Append(ConvertExpressionToSql(methodCallExpression.Arguments[0]));
                    builder.Append(" % ')");
                }
            }
            return builder.ToString();
        }

        public static string ValidateOrderBy<T>(string orderBy)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name.Equals(orderBy))
                {
                    return orderBy;
                }
            }

            throw new Exception($"Invalid order by {orderBy}.");
        }

        public static string ValidateOrderByDirection(string orderByDirection)
        {
            if (orderByDirection.ToUpper().Equals("ASC") || orderByDirection.ToUpper().Equals("DESC"))
            {
                return orderByDirection.ToUpper();
            }
            throw new Exception($"Invalid order by direction {orderByDirection}.");
        }

        private static string GetSqlOperator(ExpressionType type)
        {
            switch (type)
            {
                case ExpressionType.Equal:
                    return " = ";
                case ExpressionType.AndAlso:
                    return "AND";
                case ExpressionType.GreaterThan:
                    return " > ";
                case ExpressionType.GreaterThanOrEqual:
                    return " >= ";
                case ExpressionType.LessThan:
                    return " < ";
                case ExpressionType.LessThanOrEqual:
                    return " <= ";
                case ExpressionType.NotEqual:
                    return " <> ";
                case ExpressionType.OrElse:
                    return "OR";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
