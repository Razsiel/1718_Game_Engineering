using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Data.Command
{
    [Serializable]
    public class SerializedCommand
    {
        [SerializeField] public CommandEnum Command;

        public SerializedCommand()
        {

        }

        public SerializedCommand(CommandEnum command)
        {
            Command = command;
        }
    }
}
