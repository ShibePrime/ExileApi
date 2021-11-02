using System.Collections.Generic;
using System.Linq;

namespace ExileCore.Shared.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third)
        {
            return first.Concat(second.Concat(third));
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third,
            IEnumerable<T> fourth)
        {
            return first.Concat(second).Concat(third.Concat(fourth));
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third,
            IEnumerable<T> fourth, IEnumerable<T> fifth)
        {
            return first.Concat(second, third, fourth).Concat(fifth);
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third,
            IEnumerable<T> fourth, IEnumerable<T> fifth, IEnumerable<T> sixth)
        {
            return first.Concat(second, third, fourth).Concat( fifth.Concat(sixth));
        }
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third,
            IEnumerable<T> fourth, IEnumerable<T> fifth, IEnumerable<T> sixth, IEnumerable<T> seventh)
        {
            return first.Concat(second, third, fourth).Concat(fifth.Concat(sixth, seventh));
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> first, IEnumerable<T> second, IEnumerable<T> third,
            IEnumerable<T> fourth, IEnumerable<T> fifth, IEnumerable<T> sixth, IEnumerable<T> seventh, IEnumerable<T> eighth)
        {
            return first.Concat(second, third, fourth).Concat(fifth.Concat(sixth, seventh, eighth));
        }
    }
}