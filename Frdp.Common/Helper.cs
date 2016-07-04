using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frdp.Common
{
    public static class Helper
    {
        public static bool NotIn<T>(
            this T v,
            params T[] objs
            ) where T : struct, IComparable
        {
            return
                !In(v, objs);
        }

        public static bool In<T>(
            this T v,
            params T[] objs
            ) where T : struct, IComparable
        {
            for (var c = 0; c < objs.Length; c++)
            {
                if(objs[c].CompareTo(v) == 0)
                {
                    return
                        true;
                }
            }

            return
                false;
        }

        public static List<TOutput> ConvertAll<TInput, TOutput>(
            this List<TInput> input,
            Converter<TInput, TOutput> converter
            )
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (converter == null)
            {
                throw new ArgumentNullException("converter");
            }

            var result = new List<TOutput>(input.Count);

            foreach(var i in input)
            {
                result.Add(converter(i));
            }

            return
                result;
        }

    }
}
