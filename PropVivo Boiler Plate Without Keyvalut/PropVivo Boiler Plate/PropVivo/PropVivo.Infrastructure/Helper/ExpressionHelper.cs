﻿using PropVivo.Application.Common.Base;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace PropVivo.Infrastructure.Helper
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> and)
        {
            if (expr == null) return and;
            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(new SwapVisitor(expr.Parameters[0], and.Parameters[0]).Visit(expr.Body), and.Body), and.Parameters);
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T>(Expression<Func<T, object>> e, OperationExpression selectedOperator, object fieldValue)
        {
            string name = GetOperand<T>(e);
            return GetCriteriaWhere<T>(name, selectedOperator, fieldValue);
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T, T2>(Expression<Func<T, object>> e, OperationExpression selectedOperator, object fieldValue)
        {
            string name = GetOperand<T>(e);
            return GetCriteriaWhere<T, T2>(name, selectedOperator, fieldValue);
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T>(string fieldName, OperationExpression selectedOperator, object fieldValue)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = GetProperty(props, fieldName, true);

            var parameter = Expression.Parameter(typeof(T));
            var expressionParameter = GetMemberExpression<T>(parameter, fieldName);

            if (prop != null && fieldValue != null)
            {
                BinaryExpression body = null;

                switch (selectedOperator)
                {
                    case OperationExpression.Equals:
                        body = Expression.Equal(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);

                    case OperationExpression.NotEquals:
                        body = Expression.NotEqual(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);

                    case OperationExpression.Minor:
                        body = Expression.LessThan(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);

                    case OperationExpression.MinorEquals:
                        body = Expression.LessThanOrEqual(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);

                    case OperationExpression.Mayor:
                        body = Expression.GreaterThan(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);

                    case OperationExpression.MayorEquals:
                        body = Expression.GreaterThanOrEqual(expressionParameter, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(body, parameter);

                    case OperationExpression.Like:
                        MethodInfo contains = typeof(string).GetMethod("Contains");
                        var bodyLike = Expression.Call(expressionParameter, contains, Expression.Constant(fieldValue, prop.PropertyType));
                        return Expression.Lambda<Func<T, bool>>(bodyLike, parameter);

                    case OperationExpression.Contains:
                        return Contains<T>(fieldValue, parameter, expressionParameter);

                    case OperationExpression.NotContains:
                        return NotContains<T>(fieldValue, parameter, expressionParameter);

                    default:
                        throw new Exception("Not implement Operation");
                }
            }
            else
            {
                Expression<Func<T, bool>> filter = x => true;
                return filter;
            }
        }

        public static Expression<Func<T, bool>> GetCriteriaWhere<T, T2>(string fieldName, OperationExpression selectedOperator, object fieldValue)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            PropertyDescriptor prop = GetProperty(props, fieldName, true);

            var parameter = Expression.Parameter(typeof(T));
            var expressionParameter = GetMemberExpression<T>(parameter, fieldName);

            if (prop != null && fieldValue != null)
            {
                switch (selectedOperator)
                {
                    case OperationExpression.Any:
                        return Any<T, T2>(fieldValue, parameter, expressionParameter);

                    default:
                        throw new Exception("Not implement Operation");
                }
            }
            else
            {
                Expression<Func<T, bool>> filter = x => true;
                return filter;
            }
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr, Expression<Func<T, bool>> or)
        {
            if (expr == null) return or;
            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(new SwapVisitor(expr.Parameters[0], or.Parameters[0]).Visit(expr.Body), or.Body), or.Parameters);
        }

        private static Expression<Func<T, bool>> Any<T, T2>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            var lambda = (Expression<Func<T2, bool>>)fieldValue;
            MethodInfo anyMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
            .First(m => m.Name == "Any" && m.GetParameters().Count() == 2).MakeGenericMethod(typeof(T2));

            var body = Expression.Call(anyMethod, memberExpression, lambda);

            return Expression.Lambda<Func<T, bool>>(body, parameterExpression);
        }

        //private static Expression<Func<T, bool>> Contains<T>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        //{
        //    var list = (List<string>)fieldValue;

        // if (list == null || list.Count == 0) return x => true;

        // MethodInfo containsInList = typeof(List<string>).GetMethod("Contains", new Type[] {
        // typeof(string) }); var bodyContains = Expression.Call(Expression.Constant(fieldValue),
        // containsInList, memberExpression);

        //    return Expression.Lambda<Func<T, bool>>(bodyContains, parameterExpression);
        //}

        private static Expression<Func<T, bool>> Contains<T>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            // Check if fieldValue is null or not a string
            if (fieldValue == null)
            {
                // If fieldValue is not a string, return true predicate
                return x => true;
            }

            if (fieldValue is string stringValue)
            {
                // Split the string by commas and remove any leading/trailing whitespace
                string[] stringArray = stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(s => s.Trim())
                                                   .ToArray();

                // If the array is empty, return true predicate
                if (stringArray.Length == 0)
                {
                    return x => true;
                }

                // Call the Contains method on the string array
                var bodyContains = Expression.Call(
                    typeof(Enumerable),
                    "Contains",
                    new[] { typeof(string) },
                    Expression.Constant(stringArray),
                    memberExpression);

                // Construct and return the lambda expression
                return Expression.Lambda<Func<T, bool>>(bodyContains, parameterExpression);
            }

            // Check if fieldValue is a boolean
            if (fieldValue is bool boolValue)
            {
                // Construct an equality comparison expression
                var bodyEquals = Expression.Equal(memberExpression, Expression.Constant(boolValue));

                // Construct and return the lambda expression
                return Expression.Lambda<Func<T, bool>>(bodyEquals, parameterExpression);
            }
            return x => true;
        }

        private static MemberExpression GetMemberExpression<T>(ParameterExpression parameter, string propName)
        {
            if (string.IsNullOrEmpty(propName)) return null;
            var propertiesName = propName.Split('.');
            if (propertiesName.Count() == 2)
                return Expression.Property(Expression.Property(parameter, propertiesName[0]), propertiesName[1]);
            return Expression.Property(parameter, propName);
        }

        private static string GetOperand<T>(Expression<Func<T, object>> exp)
        {
            MemberExpression body = exp.Body as MemberExpression;

            if (body == null)
            {
                UnaryExpression ubody = (UnaryExpression)exp.Body;
                body = ubody.Operand as MemberExpression;
            }

            var operand = body.ToString();

            return operand.Substring(2);
        }

        private static PropertyDescriptor GetProperty(PropertyDescriptorCollection props, string fieldName, bool ignoreCase)
        {
            if (!fieldName.Contains('.'))
                return props.Find(fieldName, ignoreCase);

            var fieldNameProperty = fieldName.Split('.');
            return props.Find(fieldNameProperty[0], ignoreCase).GetChildProperties().Find(fieldNameProperty[1], ignoreCase);
        }

        private static Expression<Func<T, bool>> NotContains<T>(object fieldValue, ParameterExpression parameterExpression, MemberExpression memberExpression)
        {
            // Check if fieldValue is null or not a string
            if (fieldValue == null)
            {
                // If fieldValue is not a string, return true predicate
                return x => true;
            }

            if (fieldValue is string stringValue)
            {
                // Split the string by commas and remove any leading/trailing whitespace
                string[] stringArray = stringValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                   .Select(s => s.Trim())
                                                   .ToArray();

                // If the array is empty, return true predicate
                if (stringArray.Length == 0)
                {
                    return x => true;
                }

                // Call the Contains method on the string array
                var bodyContains = Expression.Call(
                    typeof(Enumerable),
                    "Contains",
                    new[] { typeof(string) },
                    Expression.Constant(stringArray),
                    memberExpression);

                var notContainsBody = Expression.Not(bodyContains);

                // Construct and return the lambda expression
                return Expression.Lambda<Func<T, bool>>(notContainsBody, parameterExpression);
            }

            return x => true;
        }
    }

    public class SwapVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        public SwapVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}