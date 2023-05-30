using System;
using System.Collections.Generic;
using System.Linq;

namespace VG.Extensions
{
    public static class FuncExtensions
    {
        public static bool Any(this Func<bool> func)
        {
            return func.GetInvocationList()
                .Cast<Func<bool>>()
                .Select(method => method())
                .ToList() //Warning: Has side-effects
                .Any(ret => ret);
        }

        public static bool All(this Func<bool> func)
        {
            return func.GetInvocationList()
                .Cast<Func<bool>>()
                .Select(method => method())
                .ToList() //Warning: Has side-effects
                .All(ret => ret);
        }

        public static bool Any<T>(this Func<T, bool> func, T t)
        {
            return func.GetInvocationList()
                .Cast<Func<T, bool>>()
                .Select(method => method(t))
                .ToList() //Warning: Has side-effects
                .Any(ret => ret);
        }

        public static bool All<T>(this Func<T, bool> func, T t)
        {
            return func.GetInvocationList()
                .Cast<Func<T, bool>>()
                .Select(method => method(t))
                .ToList() //Warning: Has side-effects
                .All(ret => ret);
        }

        public static bool Any<T0, T1>(this Func<T0, T1, bool> func, T0 t0, T1 t1)
        {
            return func.GetInvocationList()
                .Cast<Func<T0, T1, bool>>()
                .Select(method => method(t0, t1))
                .ToList() //Warning: Has side-effects
                .Any(ret => ret);
        }

        public static bool All<T0, T1>(this Func<T0, T1, bool> func, T0 t0, T1 t1)
        {
            return func.GetInvocationList()
                .Cast<Func<T0, T1, bool>>()
                .Select(method => method(t0, t1))
                .ToList() //Warning: Has side-effects
                .All(ret => ret);
        }

        public static bool Any<T0, T1, T2>(this Func<T0, T1, T2, bool> func, T0 t0, T1 t1, T2 t2)
        {
            return func.GetInvocationList()
                .Cast<Func<T0, T1, T2, bool>>()
                .Select(method => method(t0, t1, t2))
                .ToList() //Warning: Has side-effects
                .Any(ret => ret);
        }

        public static bool All<T0, T1, T2>(this Func<T0, T1, T2, bool> func, T0 t0, T1 t1, T2 t2)
        {
            return func.GetInvocationList()
                .Cast<Func<T0, T1, T2, bool>>()
                .Select(method => method(t0, t1, t2))
                .ToList() //Warning: Has side-effects
                .All(ret => ret);
        }

        public static IEnumerable<TResult> Select<TSource, TResult>(this Func<TSource> source,
            Func<TSource, TResult> selector)
        {
            return source.GetInvocationList()
                .Cast<Func<TSource>>()
                .Select(method => method())
                .ToList() //Warning: Has side-effects
                .Select(selector);
        }

        public static IEnumerable<TResult> SelectMany<TSource, TResult>(this Func<TSource> source,
            Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.GetInvocationList()
                .Cast<Func<TSource>>()
                .Select(method => method())
                .ToList() //Warning: Has side-effects
                .SelectMany(selector);
        }

        public static IEnumerable<TSource> SelectAll<TSource>(this Func<IEnumerable<TSource>> source)
        {
            return source.GetInvocationList()
                .Cast<Func<IEnumerable<TSource>>>()
                .Select(method => method())
                .ToList() //Warning: Has side-effects
                .SelectMany(selector => selector);
        }
    }
}