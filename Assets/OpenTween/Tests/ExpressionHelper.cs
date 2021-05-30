using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace OpenTween.Tests
{
    public class ExpressionHelper
    {
        public static Action<TComponent, T> CreateSetter<TComponent, T>(Expression<Func<TComponent, T>> getter)
        {
            ParameterExpression compParam = getter.Parameters[0];
            ParameterExpression valueParam = Expression.Parameter(typeof(T), "value");

            var accesses = new List<(Expression, MemberInfo)>();

            var variables = new List<ParameterExpression>();
            var assignment = new List<BinaryExpression>();
            var reverse = new Stack<BinaryExpression>();

            Expression exp = getter.Body;
            while (exp is MemberExpression e)
            {
                accesses.Add((e.Expression, e.Member));
                exp = e.Expression;
            }

            MemberExpression access;
            if (accesses.Count > 1)
            {
                // Last one is a simple access witch requires no temp variables
                accesses.RemoveAt(accesses.Count - 1);

                accesses.Reverse();

                Expression previousAssignment = null;
                int tempNum = 0;
                foreach ((Expression e, MemberInfo m) in accesses)
                {
                    previousAssignment ??= e;

                    ParameterExpression left = Expression.Parameter(e.Type, "temp_" + tempNum++);
                    BinaryExpression assign = Expression.Assign(left, previousAssignment);

                    variables.Add(left);
                    previousAssignment = Expression.MakeMemberAccess(left, m);
                    assignment.Add(assign);
                    reverse.Push(Expression.Assign(assign.Right, assign.Left));
                }

                access = Expression.MakeMemberAccess(assignment[assignment.Count - 1].Left, ((MemberExpression) getter.Body).Member);
            }
            else
            {
                access = Expression.MakeMemberAccess(accesses[0].Item1, accesses[0].Item2);
            }
            
            assignment.Add(Expression.Assign(access, valueParam));

            while (reverse.Count > 0)
            {
                assignment.Add(reverse.Pop());
            }

            BlockExpression block = Expression.Block(
                variables,
                assignment
            );

            return Expression.Lambda<Action<TComponent, T>>(block, compParam, valueParam).Compile();
        }
    }
}