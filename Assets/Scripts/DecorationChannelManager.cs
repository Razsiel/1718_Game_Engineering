using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.DataStructures.Channel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Assets.Scripts {
    public class DecorationChannelManager : Singleton<DecorationChannelManager> {
        public Dictionary<Channel, UnityAction<Channel, Player>> ChannelDictionary;

        public DecorationChannelManager() {
            ChannelDictionary = new Dictionary<Channel, UnityAction<Channel, Player>>();
            foreach (Channel channel in Enum.GetValues(typeof(Channel))) {
                ChannelDictionary.Add(channel, null);
            }
        }


        public void RegisterToChannel(Channel channel, UnityAction<Channel, Player> action) {
            Assert.IsNotNull(action);
            ChannelDictionary[channel] += action;
        }

        public void TriggerChannel(Channel channel, Player player) {
            ChannelDictionary[channel]?.Invoke(channel, player);
        }

        public void UnRegisterFromChannel(Channel channel, UnityAction<Channel, Player> action) {
            Assert.IsNotNull(action);
            ChannelDictionary[channel] -= action;
        }
    }

    public class Singleton<T> where T : class, new() {
        private static T _instance;
        public static T Instance => _instance ?? (_instance = new T());
    }
}