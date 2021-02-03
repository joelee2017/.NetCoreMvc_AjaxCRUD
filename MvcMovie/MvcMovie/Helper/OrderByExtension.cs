using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BackendServices.Common
{
    /// <summary>
    /// 排序擴充
    /// </summary>
    public static class OrderByExtension
    {
        /// <summary>
        ///  指定欄位 OrderBy
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<TSource> ColumnOrdersBy<TSource>(this IQueryable<TSource> sources, string propertyName)
        {
            return OrdersByColumn(sources, propertyName, "OrderBy");
        }

        /// <summary>
        /// 指定欄位 OrderByDescending
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static IQueryable<TSource> ColumnOrderByDescending<TSource>(this IQueryable<TSource> sources, string propertyName)
        {
            return OrdersByColumn(sources, propertyName, "OrderByDescending");
        }

        /// <summary>
        /// 擴充方法指定欄位排序
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="sources"></param>
        /// <param name="propertyName"></param>
        /// <param name="orderMethod"></param>
        /// <returns></returns>
        private static IQueryable<TSource> OrdersByColumn<TSource>(IQueryable<TSource> sources, string propertyName, string orderMethod)
        {
            Type type = typeof(TSource);
            string firstNumber = type.GetMembers()[0].Name.Split('_')[1]; // 如果意外 propertyName 為 null 就使用 TSource 第一位成員
            string orderExpression = string.Empty;
            orderExpression = orderMethod ?? "OrderBy"; // 預設 OrderBy ;

            PropertyInfo propertyInfo = type.GetProperty(propertyName ?? firstNumber);
            ParameterExpression parameter = Expression.Parameter(type, "parameter");
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, propertyInfo);
            LambdaExpression orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(
                typeof(Queryable),
                orderExpression,
                new Type[] { type, propertyInfo.PropertyType },
                sources.Expression, Expression.Quote(orderByExp));
            return sources.Provider.CreateQuery<TSource>(resultExp);
        }
    }
}
