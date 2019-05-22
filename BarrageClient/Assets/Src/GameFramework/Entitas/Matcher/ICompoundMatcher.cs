namespace GameFramework
{

    public interface ICompoundMatcher<TEntity> : IMatcher<TEntity> where TEntity :  Entity {

        int[] AllOfIndices { get; }
        int[] AnyOfIndices { get; }
        int[] NoneOfIndices { get; }
    }
}
