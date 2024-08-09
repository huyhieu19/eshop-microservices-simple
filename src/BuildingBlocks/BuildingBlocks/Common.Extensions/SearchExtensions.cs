using System.Linq.Expressions;

namespace BuildingBlocks;

public static class SearchExtensions
{
    public static Expression<Func<T, bool>> CreateSearchPredicate<T>(string searchTerm, List<Expression<Func<T, object>>> whereToSearchs)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return x => true;
        }
        var elementParameter = Expression.Parameter(typeof(T), "element");
        Expression? combinedExpression = null;

        foreach (var expression in whereToSearchs)
        {
            var extracted = Expression.Invoke(expression, elementParameter);

            var toStringMethod = typeof(object).GetMethod("ToString");
            var extractedAsString = Expression.Call(extracted, toStringMethod);

            var isNotNullOrEmpty = Expression.Not(
                Expression.Call(
                    method: typeof(string).GetMethod("IsNullOrEmpty", [typeof(string)]),
                    extractedAsString
                )
            );

            var containsSearchTerm = Expression.GreaterThanOrEqual(
            Expression.Call(
            extractedAsString,
                    method: typeof(string).GetMethod("IndexOf", [typeof(string), typeof(StringComparison)]),
                    Expression.Constant(searchTerm),
                    Expression.Constant(StringComparison.OrdinalIgnoreCase)
                ),
                Expression.Constant(0)
            );

            var searchExpression = Expression.AndAlso(isNotNullOrEmpty, containsSearchTerm);

            if (combinedExpression == null)
            {
                combinedExpression = searchExpression;
            }
            else
            {
                combinedExpression = Expression.OrElse(combinedExpression, searchExpression);
            }
        }

        return Expression.Lambda<Func<T, bool>>(body: combinedExpression, parameters: elementParameter);
    }
}
