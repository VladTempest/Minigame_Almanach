using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Devil_s13.Core.ThrowResultsProvider.Data
{
    public struct ThrowResultData
    {
        public int ParticipantIndex;
        [FormerlySerializedAs("DiceValues")] public List<int> OtherDiceValues;
        public int BetValue;
    }
}