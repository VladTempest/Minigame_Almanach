using UniRx;
using UnityEngine;
using Zenject;

namespace Devil_s13.Core.Devils13UI
{
    public class Devils13GameModel
    {
        public IReactiveProperty<int> firstDiceValue = new ReactiveProperty<int>();
        public IReactiveProperty<int> secondDiceValue = new ReactiveProperty<int>();

        public void SetDiceValues()
        {
            var firstDiceValue = Random.Range(1, 7);
            var secondDiceValue = Random.Range(1, 7);
            
            this.firstDiceValue.Value = firstDiceValue;
            this.secondDiceValue.Value = secondDiceValue;
        }
    }
}