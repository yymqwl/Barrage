namespace GameFramework
{

    public partial class Matcher<TEntity> : IAllOfMatcher<TEntity> where TEntity :  Entity {

        public int[] Indices {
            get {
                if (m_indices == null) {
                    m_indices = mergeIndices(m_allOfIndices, m_anyOfIndices, m_noneOfIndices);
                }
                return m_indices;
            }
        }

        public int[] AllOfIndices { get { return m_allOfIndices; } }
        public int[] AnyOfIndices { get { return m_anyOfIndices; } }
        public int[] NoneOfIndices { get { return m_noneOfIndices; } }

        public string[] componentNames { get; set; }

        int[] m_indices;
        int[] m_allOfIndices;
        int[] m_anyOfIndices;
        int[] m_noneOfIndices;

        Matcher() {
        }

        IAnyOfMatcher<TEntity> IAllOfMatcher<TEntity>.AnyOf(params int[] indices) {
            m_anyOfIndices = distinctIndices(indices);
            m_indices = null;
            m_isHashCached = false;
            return this;
        }

        IAnyOfMatcher<TEntity> IAllOfMatcher<TEntity>.AnyOf(params IMatcher<TEntity>[] matchers) {
            return ((IAllOfMatcher<TEntity>)this).AnyOf(mergeIndices(matchers));
        }

        public INoneOfMatcher<TEntity> NoneOf(params int[] indices) {
            m_noneOfIndices = distinctIndices(indices);
            m_indices = null;
            m_isHashCached = false;
            return this;
        }

        public INoneOfMatcher<TEntity> NoneOf(params IMatcher<TEntity>[] matchers) {
            return NoneOf(mergeIndices(matchers));
        }

        public bool Matches(TEntity entity) {

            //return true;
            return (m_allOfIndices == null || entity.HasComponents(m_allOfIndices))
                   && (m_anyOfIndices == null || entity.HasAnyComponent(m_anyOfIndices))
                   && (m_noneOfIndices == null || !entity.HasAnyComponent(m_noneOfIndices));
            
        }
    }
}
