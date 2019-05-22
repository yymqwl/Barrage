namespace GameFramework
{

    public interface IMatcher<TEntity> where TEntity :  Entity {

        int[] Indices { get; }
        bool Matches(TEntity entity);
    }
}
