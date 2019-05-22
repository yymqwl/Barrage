namespace GameFramework
{

    public interface IAllOfMatcher<TEntity> : IAnyOfMatcher<TEntity> where TEntity : Entity {

        IAnyOfMatcher<TEntity> AnyOf(params int[] indices);
        IAnyOfMatcher<TEntity> AnyOf(params IMatcher<TEntity>[] matchers);
    }
}
