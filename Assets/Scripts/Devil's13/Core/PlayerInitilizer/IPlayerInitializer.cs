using Cysharp.Threading.Tasks;

namespace Devil_s13.Core.GameLoop
{
    public interface IPlayerInitializer
    {
        public UniTask CreatePlayers();
        
        public void AddPlayerToGame(int index);
    }
}