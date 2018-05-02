using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace Assets.Scripts.DataStructures
{
    [Serializable]
    public class LevelScore
    {
        public uint HighestScore;
        public uint DecentScore;
        public uint BadScore;

        public LevelScore(uint highestScore, uint decentScore, uint badScore)
        {
            this.HighestScore = highestScore;
            this.DecentScore = decentScore;
            this.BadScore = badScore;
            CheckValidValues();
        }

        private void CheckValidValues()
        {            
            Assert.IsTrue(DecentScore < BadScore && DecentScore > HighestScore);
        }
    }
}
