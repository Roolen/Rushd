
namespace Assets.Scripts.Controllers
{
    public interface ICommandController
    {
        void Execute();
        void Undo();
    }
}
