using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data.Command
{
    [Serializable]
    public class SerializedLoopCommand : SerializedCommand
    {
        [SerializeField]
        public int LoopCount;

        [SerializeField]
        public List<SerializedLoopCommand> Commands;

        public SerializedLoopCommand()
        {

        }

        public SerializedLoopCommand(int loopCount, List<SerializedLoopCommand> commands)
        {
            LoopCount = loopCount;
            Commands = commands;
        }
    }
}
