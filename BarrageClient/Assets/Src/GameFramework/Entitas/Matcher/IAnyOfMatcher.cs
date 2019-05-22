namespace GameFramework
{

    public interface IAnyOfMatcher<TEntity> : INoneOfMatcher<TEntity> where TEntity : Entity {

        INoneOfMatcher<TEntity> NoneOf(params int[] indices);
        INoneOfMatcher<TEntity> NoneOf(params IMatcher<TEntity>[] matchers);
    }
}
