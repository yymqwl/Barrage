using System;
using System.Collections.Generic;

namespace GameFramework
{

    public partial class Matcher<TEntity> {

        static readonly List<int> m_indexBuffer = new List<int>();
        static readonly HashSet<int> m_indexSetBuffer = new HashSet<int>();

        public static IAllOfMatcher<TEntity> AllOf(params int[] indices) {
            var matcher = new Matcher<TEntity>();
            matcher.m_allOfIndices = distinctIndices(indices);
            return matcher;
        }

        public static IAllOfMatcher<TEntity> AllOf(params IMatcher<TEntity>[] matchers) {
            var allOfMatcher = (Matcher<TEntity>)Matcher<TEntity>.AllOf(mergeIndices(matchers));
            setComponentNames(allOfMatcher, matchers);
            return allOfMatcher;
        }

        public static IAnyOfMatcher<TEntity> AnyOf(params int[] indices) {
            var matcher = new Matcher<TEntity>();
            matcher.m_anyOfIndices = distinctIndices(indices);
            return matcher;
        }

        public static IAnyOfMatcher<TEntity> AnyOf(params IMatcher<TEntity>[] matchers) {
            var anyOfMatcher = (Matcher<TEntity>)Matcher<TEntity>.AnyOf(mergeIndices(matchers));
            setComponentNames(anyOfMatcher, matchers);
            return anyOfMatcher;
        }

        static int[] mergeIndices(int[] allOfIndices, int[] anyOfIndices, int[] noneOfIndices) {
            if (allOfIndices != null) {
                m_indexBuffer.AddRange(allOfIndices);
            }
            if (anyOfIndices != null) {
                m_indexBuffer.AddRange(anyOfIndices);
            }
            if (noneOfIndices != null) {
                m_indexBuffer.AddRange(noneOfIndices);
            }

            var mergedIndices = distinctIndices(m_indexBuffer);

            m_indexBuffer.Clear();

            return mergedIndices;
        }

        static int[] mergeIndices(IMatcher<TEntity>[] matchers) {
            var indices = new int[matchers.Length];
            for (int i = 0; i < matchers.Length; i++) {
                var matcher = matchers[i];
                if (matcher.Indices.Length != 1) {
                    throw new MatcherException(matcher.Indices.Length);
                }
                indices[i] = matcher.Indices[0];
            }

            return indices;
        }

        static string[] getComponentNames(IMatcher<TEntity>[] matchers) {
            for (int i = 0; i < matchers.Length; i++) {
                var matcher = matchers[i] as Matcher<TEntity>;
                if (matcher != null && matcher.componentNames != null) {
                    return matcher.componentNames;
                }
            }

            return null;
        }

        static void setComponentNames(Matcher<TEntity> matcher, IMatcher<TEntity>[] matchers) {
            var componentNames = getComponentNames(matchers);
            if (componentNames != null) {
                matcher.componentNames = componentNames;
            }
        }

        static int[] distinctIndices(IList<int> indices) {
            foreach (var index in indices) {
                m_indexSetBuffer.Add(index);
            }

            var uniqueIndices = new int[m_indexSetBuffer.Count];
            m_indexSetBuffer.CopyTo(uniqueIndices);
            Array.Sort(uniqueIndices);

            m_indexSetBuffer.Clear();

            return uniqueIndices;
        }
    }
}
