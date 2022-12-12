using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace API.Parameters;

public class SortHelper<T> : ISortHelper<T>
{
    public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
    {
        if (!entities.Any())
        {
            return entities;
        }

        if (string.IsNullOrEmpty(orderByQueryString))
        {
            return entities;
        }

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderByQueryBuilder = new StringBuilder();

        foreach (var param in orderParams.Reverse())
        {
            if (string.IsNullOrEmpty(param))
            {
                continue;
            }

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
            {
                continue;
            }

            var sortingOrder = param.EndsWith(" desc") ? "descending" : "ascending";

            orderByQueryBuilder.Append($"{objectProperty.Name} {sortingOrder}, ");
        }

        var orderQuery = orderByQueryBuilder.ToString().TrimEnd(',', ' ');
        return entities.OrderBy(orderQuery);
    }
}