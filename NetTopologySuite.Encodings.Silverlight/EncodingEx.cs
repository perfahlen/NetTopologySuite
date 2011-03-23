﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NetTopologySuite.Encodings
{
    public static class EncodingEx
    {
        private static IEncodingRegistry _encodingRegistry = new EncodingRegistry();

        public static Encoding GetACSII()
        {
            return _encodingRegistry.ASCII;
        }

        private static readonly Func<Encoding, int> Accessor = GetAccessor();

        private static Func<Encoding, int> GetAccessor()
        {
            ParameterExpression pex = Expression.Parameter(typeof(Encoding));
            MemberInfo member =
                typeof(Encoding).GetProperties(BindingFlags.Instance | BindingFlags.NonPublic |
                                                BindingFlags.GetProperty).Where(a => a.Name == "CodePage").First();

            return Expression.Lambda<Func<Encoding, int>>(
                Expression.MakeMemberAccess(pex, member), pex
                ).Compile();
        }

        public static int CodePage(this Encoding self)
        {
            if (self is UTF8Encoding)
                return 65001;

            return _encodingRegistry.GetCodePage(self);

            return Accessor(self);
        }
    }
}