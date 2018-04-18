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
using M16h;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Behaviours {
    public class DecorationBehaviour : MonoBehaviour {
        public DecorationConfiguration Configuration;

        public void Start() {
            Configuration.Init(OnActivate, OnDeactivate);
        }

        private void OnActivate(Player player) {
            Debug.Log("OnActive!");
            switch (Configuration.Type)
            {
                case ChannelType.Trigger:
                    // trigger active animation
                    this.transform.DOScaleY(-1f, 1f);
                    break;
                case ChannelType.Mechanism:
                    // mechanism active animation
                    var animateDeco = Configuration.DecorationData as AnimatibleDecorationData;
                    if (animateDeco != null) {
                        this.transform.GetChild(0)
                            .DOLocalMove(animateDeco.AnimatePosition, 1f);
                    }
                    else {
                        this.transform.DOLocalMoveY(Configuration.RelativePosition.y + 32f, 1.5f);
                    }
                    break;
                case ChannelType.Decoration:
                    this.transform.DOShakePosition(
                        1f, (player.ViewDirection.ToVector3() + new Vector3(0.15f, 0.15f, 0.15f)) * 2f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDeactivate()
        {
            Debug.Log("OnDeactivate!");
            switch (Configuration.Type)
            {
                case ChannelType.Trigger:
                    // trigger inactive animation
                    this.transform.DOScaleY(1f, 1f);
                    break;
                case ChannelType.Mechanism:
                    // mechanism inactive animation
                    var animateDeco = Configuration.DecorationData as AnimatibleDecorationData;
                    if (animateDeco != null)
                    {
                        this.transform.GetChild(0)
                            .DOLocalMove(Vector3.zero, 1f);
                    }
                    else
                    {
                        this.transform.DOLocalMoveY(Configuration.RelativePosition.y, 1.5f);
                    }
                    
                    break;
                case ChannelType.Decoration:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}