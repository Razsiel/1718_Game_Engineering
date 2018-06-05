﻿using System.Collections;
using System.Collections.Generic;
using Assets.Data.Grids;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using Assets.Scripts.Lib.Extensions;
using DG.Tweening;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class PlayerAnimationBehaviour : MonoBehaviour {
    private Player _player;
    private bool _isJumping;
    private bool _isSimulating;

    private Tweener _currentAnimation;
    private Tween _jumpTween;
    private Vector3 _startPosition;

    void Awake() {
        EventManager.OnAllPlayersReady += StopJumping;
        EventManager.OnSimulationStop += OnSimulationEnded;
        EventManager.OnSimulate += OnSimulationStarted;

        _isJumping = false;
    }

    void OnDestroy() {
        EventManager.OnAllPlayersReady -= StopJumping;
        EventManager.OnSimulationStop -= OnSimulationEnded;
        EventManager.OnSimulate -= OnSimulationStarted;

        if (_player != null) {
            _player.OnReady -= AnimatePlayerReady;
            _player.OnMoveTo -= AnimateMoveTo;
            _player.OnTurn -= AnimateTurn;
            _player.OnWait -= AnimateWait;
            _player.OnInteract -= AnimateInteract;
            _player.OnFailMoveTo -= AnimateFailedMove;
        }
    }

    public void OnSimulationEnded() {
        _isSimulating = false;
    }

    public void OnSimulationStarted(LevelData lvlData, List<TGEPlayer> players) {
        _isSimulating = true;
        StopJumping();
    }

    public void OnPlayerSpawned(Player player) {
        _player = player;
        _player.OnReady += AnimatePlayerReady;
        _player.OnMoveTo += AnimateMoveTo;
        _player.OnTurn += AnimateTurn;
        _player.OnWait += AnimateWait;
        _player.OnInteract += AnimateInteract;
        _player.OnFailMoveTo += AnimateFailedMove;
        _player.OnReset += OnReset;

        _startPosition = _player.transform.position;
    }

    private void OnReset() {
        print("Completing player animation");
        _currentAnimation.Complete();
        StopJumping();
    }

    public void AnimateMoveTo(Vector3 to) {
        _currentAnimation = transform.DOMove(to, 1f);
    }

    public void AnimateTurn(Vector3 targetRotation) {
        _currentAnimation = transform.DORotate(targetRotation, 1f);
    }

    // Bump into wall or player
    public void AnimateFailedMove(Vector3 targetDestination) {
        Vector3 bumpPosition = (transform.position + targetDestination) / 2;
        var startPosition = transform.position;

        _currentAnimation = transform.DOMove(bumpPosition, 0.5f)
                              .OnComplete(() => {
                                  transform.DOShakeScale(0.3f, 0.2f, 30, 20f);
                                  _currentAnimation = transform.DOMove(startPosition, 0.5f);
                              });
    }

    // Ready and waiting
    public void AnimatePlayerReady(bool isReady) {
        print($"{nameof(PlayerAnimationBehaviour)}: AnimatePlayerReady isReady = {isReady} isJumping = {_isJumping}");

        StopJumping();

        if (isReady) {
            if (!_isJumping && !_isSimulating)
            {
                _isJumping = true;
                DoJumpAnimation();
            }
        }
    }

    private void DoJumpAnimation()
    {
        if (_isJumping && !_isSimulating)
        {
            _jumpTween = transform.DOJump(transform.position, 0.8f, 1, 0.35f).SetAutoKill(false);
            _jumpTween.OnComplete(() => _jumpTween.Restart());
        }
    }

    private void StopJumping()
    {
        _jumpTween?.OnComplete(() => _jumpTween?.SetAutoKill(true));
        _player.transform.position = _startPosition;
        _isJumping = false;
    }

    public void AnimateInteract() {
        _jumpTween = transform.DOJump(transform.position, 0.8f, 1, 0.8f);
    }

    public void AnimateBumpIntoWall() { }

    public void AnimateBumpIntoPlayer() { }

    public void AnimateWait() {
        var startRotation = transform.rotation.eulerAngles;
        const float totalTime = 1.75f;
        const float time = totalTime / 4f;
        _currentAnimation = transform
            .DORotate(new Vector3(0, 45, 0), time) // rotate right (1/4 of totalTime)
            .OnComplete(() => {
                _currentAnimation = transform
                    .DORotate(new Vector3(0, 135, 0), time * 2) // rotate left (1/2 of totalTime)
                    .OnComplete(
                        () => {
                            _currentAnimation = transform.DORotate(startRotation, time); // return to original (1/4 of totalTime)
                        });
            });

    }
}