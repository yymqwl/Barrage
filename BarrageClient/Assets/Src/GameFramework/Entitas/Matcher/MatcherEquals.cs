namespace GameFramework
{

    public partial class Matcher<TEntity> {

        public override bool Equals(object obj) {
            if (obj == null || obj.GetType() != GetType() ||
                obj.GetHashCode() != GetHashCode()) {
                return false;
            }

            var matcher = (Matcher<TEntity>)obj;
            if (!equalIndices(matcher.AllOfIndices, AllOfIndices)) {
                return false;
            }
            if (!equalIndices(matcher.AnyOfIndices, AnyOfIndices)) {
                return false;
            }
            if (!equalIndices(matcher.NoneOfIndices, NoneOfIndices)) {
                return false;
            }

            return true;
        }

        static bool equalIndices(int[] i1, int[] i2) {
            if ((i1 == null) != (i2 == null)) {
                return false;
            }
            if (i1 == null) {
                return true;
            }
            if (i1.Length != i2.Length) {
                return false;
            }

            for (int i = 0; i < i1.Length; i++) {
                if (i1[i] != i2[i]) {
                    return false;
                }
            }

            return true;
        }

        int m_hash;
        bool m_isHashCached;

        public override int GetHashCode() {
            if (!m_isHashCached) {
                var hash = GetType().GetHashCode();
                hash = applyHash(hash, m_allOfIndices, 3, 53);
                hash = applyHash(hash, m_anyOfIndices, 307, 367);
                hash = applyHash(hash, m_noneOfIndices, 647, 683);
                m_hash = hash;
                m_isHashCached = true;
            }

            return m_hash;
        }

        static int applyHash(int hash, int[] indices, int i1, int i2) {
            if (indices != null) {
                for (int i = 0; i < indices.Length; i++) {
                    hash ^= indices[i] * i1;
                }
                hash ^= indices.Length * i2;
            }

            return hash;
        }
    }
}
