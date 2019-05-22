namespace GameFramework
{

    public interface INoneOfMatcher<TEntity> : ICompoundMatcher<TEntity> where TEntity : Entity {
    }
}
