namespace GameFramework
{

    public class RootBehaviour<T> :ABehaviourSet
    {
        T m_Owner;
        public T Owner
        {
            get
            {
                return m_Owner;
            }
        }
        public RootBehaviour(T owner)
        {
            m_Owner = owner;
        }
    }
}