using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data.Levels
{
    [CreateAssetMenu(menuName = "Data/LevelLibrary", fileName = "LevelLibrary")]
    public class LevelLibrary : ScriptableObject {
        [SerializeField] public List<LevelData> Levels;
    }
}
