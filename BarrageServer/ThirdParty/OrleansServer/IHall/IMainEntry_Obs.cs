using Orleans;

namespace IHall
{
    public interface IMainEntry_Obs : IGrainObserver
    {
        void Handle(string msg);
    }
}
