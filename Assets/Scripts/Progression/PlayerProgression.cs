using System;
using UnityEngine;

namespace TaskReccoGame.Progression
{
    public sealed class PlayerProgression : MonoBehaviour
    {
        public int Coins { get; private set; }
        public int Score { get; private set; }

        public event Action Changed;

        public void AddCoins(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Coins = checked(Coins + amount);
            Changed?.Invoke();
        }

        public void AddScore(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount));
            }

            Score = checked(Score + amount);
            Changed?.Invoke();
        }
    }
}
