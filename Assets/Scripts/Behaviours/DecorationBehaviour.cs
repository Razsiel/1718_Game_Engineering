using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Data.Tiles;
using Assets.Scripts.DataStructures;
using Assets.Scripts.DataStructures.Channel;
using Assets.Scripts.Lib.Extensions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Behaviours {
    public class DecorationBehaviour : MonoBehaviour {
        public DecorationConfiguration Configuration;

        public void Start() {
            // register self to receive messages from 'interacts' if I'm a mechanism listening to a channel
            if (Configuration.IsMechanismWithChannel) {
                DecorationChannelManager.Instance.RegisterToChannel(Configuration.Channel, OnChannelTriggered);
            }
        }

        public void OnDisable() {
            if (Configuration.IsMechanismWithChannel) {
                DecorationChannelManager.Instance.UnRegisterFromChannel(Configuration.Channel, OnChannelTriggered);
            }
        }

        private void OnChannelTriggered(Channel channel, Player player) {
            Debug.Log($"Received message from channel: {channel}");
            this.transform.DOShakeScale(1f, 1.25f);
        }

        public void OnInteract(Player player) {
            this.transform.DOShakePosition(
                1f, (player.ViewDirection.ToVector3() + new Vector3(0.15f, 0.15f, 0.15f)) * 0.4f);
        }
    }
}