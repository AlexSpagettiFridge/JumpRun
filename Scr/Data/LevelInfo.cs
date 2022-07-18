using System.Linq;

namespace JumpRun.Scr.Data
{
    public class LevelInfo
    {
        public LevelInfo(int coinAmount)
        {
            collectedCoins = new bool[coinAmount];
            for (int i = 0; i < collectedCoins.Length; i++)
            {
                collectedCoins[i] = false;
            }
        }

        //Coins
        private bool[] collectedCoins = null;

        public int TotalCoinAmount => collectedCoins.Length;
        public int TotalCollectedCoins => collectedCoins.Count(x => x);
        public bool IsCoinCollected(int index) => collectedCoins[index];
        public void CollectCoin(int index) => collectedCoins[index] = true;
    }
}