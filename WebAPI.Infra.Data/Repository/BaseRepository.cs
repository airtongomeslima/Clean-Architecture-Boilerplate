using System;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace WebAPI.Infra.Data.Repository
{
    public class BaseRepository
    {
        public static SqlConnection DbConnection()
        {
            var cnnString = "Server=192.168.0.207;Database=BaseTeste;User Id=sa;Password=@GL#9941r70n;";

            return new SqlConnection(cnnString);
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
                builder.Append(memberExpression.Member.Name);
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
