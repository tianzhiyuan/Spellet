using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Ors.Core;
using Ors.Core.Data;

namespace Ors.Framework.Complilation
{
    public class CompileContext
    {
        public PropertyInfo[] QueryProperties { get; set; }
        public PropertyInfo[] ModelProperties { get; set; }
        public ParameterExpression QueryParam { get; set; }
        public ParameterExpression SourceParam { get; set; }
        public Type ModelType { get; set; }
        public int Depth { get; set; }
        public string[] Navigations { get; set; }
    }
    public static class Compiler
    {
        /*
         *
         * IQueryable<TModel> DynamicFunc<TModel>(IQuery<TModel> query, IQueryable<TModel> source)
         * {
         *      var concreteQuery = query as ConcreteQueryType;
         *      ----    Contains
         *      if(concreteQuery.IDs != null)
         *     ｛
         *          source = source.Where(o=>concreteQuery.IDList.Contains(o.ID));
         *      ｝
         *      ----   Equals
         *      if(concreteQuery.ID != null){
         *          source = source.Where(o=>concreteQuery.ID == o.ID);
         *      }
         *      ----    StringContains
         *      if(concreteQuery.NamePattern != null){
         *          source = source.Where(o=>o.Name.Contains(concreteQuery.NamePattern));
         *      }
         *      ----    Range
         *      if(concreteQuery.CreateAtRange != null){
         *          if(concreteQuery.CreateAtRange.Left != null){
         *              source = concreteQuery.LeftOpen??source.Where(o=>o.CreateAt > concreteQuery.CreateAtRange.Left):source.Where(o=>o.CreateAtRange>=concreteQuery.CreateAtRange.Left);
         *          }
         *          if(concreteQuery.CreateAtRange.Right != null){
         *              source = concreteQuery.RightOpen??source.Where(o=>o.CreateAt < concreteQuery.CreateAtRange.Right):source.Where(o=>o.CreateAtRange<=concreteQuery.CreateAtRange.Left);
         *      }
         *      ----    Take
         *      if(concreteQuery.Take != null){
         *          source = sourc.Take(concreteQuery.Take);
         *      }
         *      ----    Skip
         *      if(concreteQuery.Skip != null){
         *          source = sourc.Skip(concreteQuery.Skip);
         *      }
         *      ----    Order
         *      if(concreteQuery.OrderField != null){
         *          if(concreteQuery.OrderField == 'ID'){
         *              source = (concreteQuery.OrderDirection == OrderDirection.ASC)?source.OrderBy(o=>o.ID):source.OrderByDescending(o=>o.ID);
         *          }
         *      }
         *      return source;
         * }
         */
        public static Func<IQueryable<TModel>, IQuery<TModel>, IQueryable<TModel>> Compile<TModel>(IQuery query)
            where TModel:class, IModel
        {
            Func<IQueryable<TModel>, IQuery<TModel>, IQueryable<TModel>> handler = null;
            var concreteQueryType = query.GetType();
            var modelType = query.ModelType;
            var queryProperties = concreteQueryType.GetProperties();
            var modelProperties = modelType.GetProperties();
            var queryParamExpr = Expression.Parameter(typeof (IQuery<TModel>), "query");
            var concreteQueryExpr = Expression.Parameter(concreteQueryType, "concreteQuery");
            var sourceParamExpr = Expression.Parameter(typeof (IQueryable<TModel>), "source");
            var context = new CompileContext()
                {
                    ModelProperties = modelProperties,
                    QueryProperties = queryProperties,
                    QueryParam = concreteQueryExpr,
                    SourceParam = sourceParamExpr,
                    ModelType = modelType,
                    Depth = 1,
                    Navigations = new string[0]
                };

            //Handle Equals
            var blocks = new List<Expression>()
                {
                    Expression.Assign(concreteQueryExpr, Expression.Convert(queryParamExpr, concreteQueryType))
                };
            blocks.AddRange(EqualExprssion(context));
            blocks.AddRange(StringContainExpression(context));
            blocks.AddRange(ListContainsExpression(context));
            blocks.AddRange(RangeExpression(context));
            blocks.AddRange(PaginationExpression(context));
            blocks.AddRange(NavigationExpression(context));
            blocks.Add(sourceParamExpr);
            var expr = Expression.Lambda(
                Expression.Block(new[] { concreteQueryExpr }, blocks), new[] { sourceParamExpr, queryParamExpr }
                );
            handler = (Func<IQueryable<TModel>, IQuery<TModel>, IQueryable<TModel>>)expr.Compile();
            return handler;
        }
        public static MethodInfo WhereMethod(Type modelType)
        {
             var whereMethod =
                        typeof (Queryable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                                             .FirstOrDefault(
                                                 mi =>
                                                 mi.Name == "Where");
                    whereMethod = whereMethod.MakeGenericMethod(new Type[] {modelType});
            return whereMethod;
        }
        
        public static Expression[] EqualExprssion(CompileContext context)
        {
            /**
             * *      ----   Equals
             *      if(concreteQuery.ID != null){
             *          source = source.Where(o=>concreteQuery.ID == o.ID);
             *      }
             */
            
            var modelProperties = context.ModelProperties;
            var blocks = new List<Expression>();
            var queryParam = context.QueryParam;
            var sourceParam = context.SourceParam;
            var modelType = context.ModelType;
            Func<Expression, Expression> modelAccess = expr => context.Navigations.Aggregate(expr, Expression.Property);
            foreach (var queryProperty in context.QueryProperties)
            {
                var queryPropertyName = queryProperty.Name;
                foreach (var modelProperty in modelProperties)
                {
                    if (queryPropertyName == modelProperty.Name)
                    {
                        var whereMethod = WhereMethod(modelType);

                        var t = Expression.Parameter(modelType, "t");
                        
                        var predictExpr = Expression.Lambda(
                            typeof (Func<,>).MakeGenericType(new Type[] {modelType, typeof (bool)}),
                            Expression.Equal(Expression.Property(queryParam, queryProperty),
                                             Expression.Property(modelAccess(t), modelProperty)),
                            t

                            );
                        var expr =
                            Expression.IfThen(
                                Expression.NotEqual(
                                    Expression.Property(queryParam, queryProperty),
                                    Expression.Constant(null)),
                                Expression.Assign(sourceParam,
                                                  Expression.Call(whereMethod, new Expression[] { sourceParam, predictExpr })
                                    ));
                        blocks.Add(expr);
                    }
                }
            }
            return blocks.ToArray();

        }

        public static Expression[] StringContainExpression(CompileContext context)
        {
            /*      ----    StringContains
             *      if(concreteQuery.NamePattern != null){
             *          source = source.Where(o=>o.Name.Contains(concreteQuery.NamePattern));
             *      }
             */
            var suffix = "Pattern";
            var modelProperties = context.ModelProperties;
            var blocks = new List<Expression>();
            var queryParam = context.QueryParam;
            var sourceParam = context.SourceParam;
            var modelType = context.ModelType;
            var stringContainMethod = typeof (string).GetMethod("Contains");
            Func<Expression, Expression> modelAccess = expr => context.Navigations.Aggregate(expr, Expression.Property);
            foreach (var queryProperty in context.QueryProperties.Where(p=>p.Name.EndsWith(suffix)))
            {
                var queryPropertyName = queryProperty.Name;
                foreach (var modelProperty in modelProperties)
                {
                    if (queryPropertyName == modelProperty.Name + suffix &&
                        modelProperty.PropertyType == typeof (string) &&
                        queryProperty.PropertyType == typeof (string))
                    {
                        var whereMethod = WhereMethod(modelType);

                        var t = Expression.Parameter(modelType, "t");
                        var predictExpr = Expression.Lambda(
                            Expression.Call(Expression.Property(modelAccess(t), modelProperty),
                                            stringContainMethod,
                                            new Expression[] {Expression.Property(queryParam, queryProperty)}
                                ),
                            t
                            );
                        
                        var expr =
                            Expression.IfThen(
                                Expression.NotEqual(
                                    Expression.Property(queryParam, queryProperty),
                                    Expression.Constant(null)),
                                Expression.Assign(sourceParam,
                                                  Expression.Call(whereMethod,
                                                                  new Expression[] {sourceParam, predictExpr})
                                    ));
                        blocks.Add(expr);
                    }
                }
            }
            return blocks.ToArray();
        }

        public static Expression[] ListContainsExpression(CompileContext context)
        {
            /**
             *      ***Contains
             *      if(concreteQuery.IDs != null)
             *     ｛
             *          source = source.Where(o=>concreteQuery.IDList.Contains(o.ID.Value));
             *      ｝
             */

            var suffix = "List";
            var modelProperties = context.ModelProperties;
            var blocks = new List<Expression>();
            var queryParam = context.QueryParam;
            var sourceParam = context.SourceParam;
            var modelType = context.ModelType;
            var cm = typeof (Enumerable).GetMethods().FirstOrDefault(mi => mi.Name == "Contains");
            Func<Expression, Expression> modelAccess = expr => context.Navigations.Aggregate(expr, Expression.Property);
            foreach (var queryProperty in context.QueryProperties.Where(p=>p.Name.EndsWith(suffix)))
            {
                var queryPropertyName = queryProperty.Name;
                foreach (var modelProperty in modelProperties)
                {
                    var containsMethod = cm;
                    if (queryPropertyName == modelProperty.Name + suffix &&
                        queryProperty.PropertyType.IsArray &&
                        (queryProperty.PropertyType.GetElementType() == modelProperty.PropertyType ||
                        queryProperty.PropertyType.GetElementType() == Nullable.GetUnderlyingType(modelProperty.PropertyType)))
                    {
                        var whereMethod = WhereMethod(modelType);
                        var t = Expression.Parameter(modelType, "t");
                        Expression predictExpr;
                        if (modelProperty.PropertyType == typeof (string))
                        {
                            containsMethod = containsMethod.MakeGenericMethod(typeof (string));
                            predictExpr = Expression.Lambda(
                                Expression.Call(
                                    containsMethod,
                                    Expression.Property(queryParam, queryProperty),
                                    Expression.Property(modelAccess(t), modelProperty)
                                    ),
                                t
                                );
                        }
                        else
                        {
                            containsMethod =
                                containsMethod.MakeGenericMethod(queryProperty.PropertyType.GetElementType());
                            predictExpr = Expression.Lambda(
                                Expression.Call(
                                    containsMethod,
                                    Expression.Property(queryParam, queryProperty),
                                    Expression.Property(Expression.Property(modelAccess(t), modelProperty), "Value")
                                    ),
                                t
                                );
                        }
                        var expr =
                            Expression.IfThen(
                                Expression.NotEqual(
                                    Expression.Property(queryParam, queryProperty),
                                    Expression.Constant(null)),
                                Expression.Assign(sourceParam,
                                                  Expression.Call(whereMethod,
                                                                  new Expression[] { sourceParam, predictExpr })
                                    ));
                        blocks.Add(expr);
                    }
                }
            }
            return blocks.ToArray();
        }

        public static Expression[] RangeExpression(CompileContext context)
        {
            /*      ----    Range
             *      if(concreteQuery.CreateAtRange != null){
             *          if(concreteQuery.CreateAtRange.Left != null){
             *              source = concreteQuery.LeftOpen??source.Where(o=>o.CreateAt > concreteQuery.CreateAtRange.Left):source.Where(o=>o.CreateAtRange>=concreteQuery.CreateAtRange.Left);
             *          }
             *          if(concreteQuery.CreateAtRange.Right != null){
             *              source = concreteQuery.RightOpen??source.Where(o=>o.CreateAt < concreteQuery.CreateAtRange.Right):source.Where(o=>o.CreateAtRange<=concreteQuery.CreateAtRange.Left);
             *      }
             */
            var suffix = "Range";
            var modelProperties = context.ModelProperties;
            var blocks = new List<Expression>();
            var queryParam = context.QueryParam;
            var sourceParam = context.SourceParam;
            var modelType = context.ModelType;
            var whereMethod = WhereMethod(modelType);
            Func<Expression, Expression> modelAccess = expr => context.Navigations.Aggregate(expr, Expression.Property);
            foreach (var queryProperty in context.QueryProperties.Where(p => p.Name.EndsWith(suffix)))
            {
                var queryPropertyName = queryProperty.Name;
                foreach (var modelProperty in modelProperties)
                {
                    if (queryPropertyName == modelProperty.Name + suffix &&
                        queryProperty.PropertyType.GetGenericTypeDefinition() == typeof(Range<>) )
                    {
                        
                        var t = Expression.Parameter(modelType, "t");
                        var leftLargerThan =
                            Expression.Call(whereMethod,
                                            new Expression[]
                                                {
                                                    sourceParam,
                                                    Expression.Lambda(Expression.GreaterThan(
                                                        Expression.Property(modelAccess(t), modelProperty),
                                                        Expression.Property(
                                                            Expression.Property(queryParam, queryProperty), "Left")
                                                                          ),
                                                                          t)
                                                });
                        var leftLargerThanOrEqual =
                            Expression.Call(whereMethod,
                                            new Expression[]
                                                {
                                                    sourceParam,
                                                    Expression.Lambda(Expression.GreaterThanOrEqual(
                                                        Expression.Property(modelAccess(t), modelProperty),
                                                        Expression.Property(
                                                            Expression.Property(queryParam,
                                                                                queryProperty),
                                                            "Left")
                                                                          ),t)
                                                }
                                );
                        var rightLessThan =
                            Expression.Call(whereMethod,
                                            new Expression[]
                                                {
                                                    sourceParam,
                                                    Expression.Lambda(Expression.LessThan(
                                                        Expression.Property(modelAccess(t), modelProperty),
                                                        Expression.Property(
                                                            Expression.Property(queryParam, queryProperty), "Right")
                                                                          ), t)
                                                }
                                );
                        var rightLessThanOrEuqal =
                            Expression.Call(whereMethod,
                                            new Expression[]
                                                {
                                                    sourceParam,
                                                    Expression.Lambda(Expression.LessThanOrEqual(
                                                        Expression.Property(modelAccess(t), modelProperty),
                                                        Expression.Property(
                                                            Expression.Property(queryParam, queryProperty), "Right")), t)

                                                }
                                );
                        var expr =
                            Expression.IfThen(
                                Expression.NotEqual(
                                    Expression.Property(queryParam, queryProperty),
                                    Expression.Constant(null)),
                                Expression.Block(new Expression[]
                                    {
                                        Expression.IfThen(
                                            Expression.NotEqual(
                                                Expression.Property(Expression.Property(queryParam, queryProperty),
                                                                    "Left"),
                                                Expression.Constant(null)
                                                ),
                                            Expression.IfThenElse(
                                                Expression.Property(Expression.Property(queryParam, queryProperty),
                                                                    "LeftOpen"),
                                                Expression.Assign(
                                                    sourceParam,
                                                    leftLargerThan
                                                    ),
                                                Expression.Assign(
                                                    sourceParam,
                                                    leftLargerThanOrEqual
                                                    )
                                                )),
                                        Expression.IfThen(
                                            Expression.NotEqual(
                                                Expression.Property(Expression.Property(queryParam, queryProperty),
                                                                    "Right"),
                                                Expression.Constant(null)
                                                ),
                                            Expression.IfThenElse(
                                                Expression.Property(Expression.Property(queryParam, queryProperty),
                                                                    "RightOpen"),
                                                Expression.Assign(
                                                    sourceParam,
                                                    rightLessThan
                                                    ),
                                                Expression.Assign(
                                                    sourceParam,
                                                    rightLessThanOrEuqal
                                                    )
                                                )
                                                     )
                                    })
                                );
                        blocks.Add(expr);
                    }
                }
            }
            return blocks.ToArray();
        }

        public static Expression[] PaginationExpression(CompileContext context)
        {
            var blocks = new List<Expression>();
            /*
             *      ----    Take
             *      if(concreteQuery.Take != null){
             *          source = sourc.Take(concreteQuery.Take);
             *      }
             *      ----    Skip
             *      if(concreteQuery.Skip != null){
             *          source = sourc.Skip(concreteQuery.Skip);
             *      }
             *      ----    Order
             *      if(concreteQuery.OrderField != null){
             *          if(concreteQuery.OrderField == 'ID'){
             *              source = (concreteQuery.OrderDirection == OrderDirection.ASC)?source.OrderBy(o=>o.ID):source.OrderByDescending(o=>o.ID);
             *          }
             *      }
             *      query.Count = source.Count();
             */
            var queryParam = context.QueryParam;
            var sourceParam = context.SourceParam;
            var modelType = context.ModelType;
            var defaultOrderField = "ID";
            var canOrderedProperties = context.ModelProperties.Where(
                pi => pi.PropertyType == typeof (string) || pi.PropertyType.IsValueType).ToArray();
            var stringIsNullOrEmptyMethod = typeof (string).GetMethod("IsNullOrEmpty");
            var orderFieldProperty = Expression.Property(queryParam, "OrderField");
            var orderDirectionProperty = Expression.Property(queryParam, "OrderDirection");
            var orderByMethod =
                typeof (Queryable).GetMethods().FirstOrDefault(mi => mi.Name == "OrderBy");
            var orderByDescendingMethod =
                typeof (Queryable).GetMethods()
                                  .FirstOrDefault(mi => mi.Name == "OrderByDescending");
            var orderFieldParam = Expression.Parameter(typeof (string), "orderField");
            var directionParam = Expression.Parameter(typeof (OrderDirection), "direction");
            var exprAssignDefault = Expression.Condition(
                Expression.Call(
                    stringIsNullOrEmptyMethod,
                    new Expression[] {orderFieldProperty}
                    ),
                Expression.Assign(orderFieldParam, Expression.Constant(defaultOrderField)),
                Expression.Assign(orderFieldParam, orderFieldProperty)
                );
            blocks.Add(exprAssignDefault);
            blocks.Add(Expression.Condition(
                Expression.Equal(orderDirectionProperty, Expression.Constant(null)),
                Expression.Assign(directionParam, Expression.Constant(OrderDirection.ASC)),
                Expression.Assign(directionParam, Expression.Property(orderDirectionProperty, "Value"))
                           ));
            var tempParam = Expression.Parameter(modelType, "t");
            var switchCaseExpr =
                canOrderedProperties.Select(
                    property =>
                    Expression.SwitchCase(
                        Expression.Condition(
                            Expression.NotEqual(directionParam,
                                                Expression.Constant(OrderDirection.DESC)),
                            Expression.Assign(sourceParam,
                                              Expression.Call(
                                                  orderByMethod.MakeGenericMethod(new[]
                                                      {modelType, property.PropertyType}),
                                                  sourceParam,
                                                  Expression.Lambda(
                                                      Expression.Property(tempParam,
                                                                          property.Name),
                                                      tempParam))),
                            Expression.Assign(sourceParam,
                                              Expression.Call(
                                                  orderByDescendingMethod
                                                      .MakeGenericMethod(new[] {modelType, property.PropertyType}),
                                                  sourceParam,
                                                  Expression.Lambda(
                                                      Expression.Property(tempParam,
                                                                          property.Name),
                                                      tempParam)))),
                        Expression.Constant(property.Name))
                    ).ToArray();
            var switchExpr =
                Expression.Switch(orderFieldParam,
                                  Expression.Assign(sourceParam, sourceParam),
                                  switchCaseExpr
                                  );
            blocks.Add(switchExpr);

            blocks.Add(Expression.Assign(sourceParam,
                                         Expression.Call(
                                             typeof (Compiler).GetMethod("PagerHandler").MakeGenericMethod(modelType),
                                             sourceParam, queryParam)));

            var blockExpr = Expression.Block(new ParameterExpression[] {orderFieldParam, directionParam}, blocks);



            return new Expression[] {blockExpr};
        }

        public static IQueryable<TModel> PagerHandler<TModel>(IQueryable<TModel> source, IQuery<TModel> query)
            where TModel : class, IModel
        {
            query.Count = source.Count();
            if (query.Skip != null && query.Skip > 0)
            {
                source = source.Skip(query.Skip.Value);
            }
            if (query.Take != null && query.Take > 0)
            {
                source = source.Take(query.Take.Value);
            }
            return source;
        }

        public static Expression[] NavigationExpression(CompileContext context)
        {
            /**
             *      ***Navigation
             *      if(concreteQuery.UserQuery != null)
             *     ｛
             *          {
             *              var naviQuery = concreteQuery.UserQuery;
             *              if(naviQuery.Id != null){
             *                  source = source.Where(t=>t.User.Id == naviQuery.Id);
             *              }
             *          }
             *      ｝
             */
            if (context.Depth >= 4) return null;
            var suffix = "Query";
            var modelProperties = context.ModelProperties;
            var blocks = new List<Expression>();
            var sourceParam = context.SourceParam;
            foreach (var queryProperty in context.QueryProperties.Where(p => p.Name.EndsWith(suffix)))
            {
                var queryPropertyName = queryProperty.Name;
                foreach (var modelProperty in modelProperties)
                {
                    if (queryPropertyName == modelProperty.Name + suffix && typeof(IQuery).IsAssignableFrom(queryProperty.PropertyType))
                    {
                        var naviBlocks = new List<Expression>();
                        var naviQueryParam = Expression.Parameter(queryProperty.PropertyType,
                                                                  "naviQuery_" + (context.Depth + 1));
                        var naviContext = new CompileContext()
                            {
                                Depth = context.Depth + 1,
                                ModelProperties = modelProperty.PropertyType.GetProperties(),
                                ModelType = context.ModelType,
                                QueryParam = naviQueryParam,
                                QueryProperties = queryProperty.PropertyType.GetProperties(),
                                SourceParam = sourceParam,
                                Navigations = context.Navigations.Concat(new []{modelProperty.Name}).ToArray()
                            };
                        naviBlocks.Add(Expression.Assign(naviQueryParam, Expression.Property(context.QueryParam, queryPropertyName)));
                        naviBlocks.AddRange(EqualExprssion(naviContext));
                        naviBlocks.AddRange(StringContainExpression(naviContext));
                        naviBlocks.AddRange(ListContainsExpression(naviContext));
                        naviBlocks.AddRange(RangeExpression(naviContext));
                        blocks.Add(Expression.Block(new[] {naviQueryParam}, naviBlocks));
                    }
                }
            }
            return blocks.ToArray();
        }
    }
}
