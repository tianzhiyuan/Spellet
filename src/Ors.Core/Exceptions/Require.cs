using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ors.Core.Data;
using Ors.Core.Properties;

namespace Ors.Core.Exceptions
{
    public static class Require
    {
        public static void SafeInvoke(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }

        public static void NotNullOrEmpty(string source, string name)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw RuleViolatedException.ArgumentNull(name);
            }
        }
        public static void NotNullOrEmpty<T>(IEnumerable<T> source, string name)
        {
            if (source == null || !source.Any())
            {
                throw RuleViolatedException.ArgumentNull(name);
            }
        }
        public static void NotNull<T>(T? source, string name)
            where T : struct
        {
            if (source == null)
            {
                throw RuleViolatedException.ArgumentNull(name);
            }
        }
        public static void NotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw RuleViolatedException.ArgumentNull(name);
            }
        }
        public static void LengthWithin<T>(IEnumerable<T> source, int min, int max, string name)
        {
            if (source == null) return;
            int length = source.Count();
            if (length < min || length > max)
            {
                throw new RuleViolatedException(string.Format(Resources.RuleViolated_LengthOutOfRange, name, min, max),
                                                RuleViolatedType.ArgumentError);
            }

        }
        public static void LengthWithin<T>(IEnumerable<T> source, int max, string name)
        {
            LengthWithin(source, 0, max, name);
        }
        public static void WithinRange<T>(IComparable<T> source, T min, T max, string name)
        {
            if (source == null) return;
            if (source.CompareTo(min) < 0 || source.CompareTo(max) > 0)
            {
                throw new RuleViolatedException(string.Format(Resources.RuleViolated_OutOfRange, name, min, max),
                                                 RuleViolatedType.ArgumentError);
            }
        }
        public static void WithinRange<T>(IComparable<T> source, T max, string name)
        {
            WithinRange(source, default(T), max, name);
        }
        public static void That(bool condition, string message)
        {
            if (!condition)
            {
                throw new RuleViolatedException(message);
            }
        }
        public static void Duplicated<T>(T left, T right, string name) where T : IComparable
        {
            if (left.CompareTo(right) == 0)
            {
                throw new RuleViolatedException(string.Format(Resources.RuleViolated_Duplicated, name));
            }
        }
        public static void Duplicate<TModel>(TModel left, TModel right, string name) where TModel : class ,IModel
        {
            if (new ModelComparer<TModel>().Equals(left, right))
            {
                throw new RuleViolatedException(string.Format(Resources.RuleViolated_Duplicated, name));
            }
        }
    }
}
