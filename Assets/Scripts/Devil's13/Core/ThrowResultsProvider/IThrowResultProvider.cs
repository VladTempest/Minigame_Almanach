using Devil_s13.Core.ThrowResultsProvider.Data;

namespace Devil_s13.Core.ThrowResultsProvider
{
    public interface IThrowResultProvider
    {
        PlayersThrowResultData GetThrowResult(GameStateDataAsset gameStateDataAsset);
    }
}