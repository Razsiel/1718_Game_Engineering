using System.Collections;
using System.Collections.Generic;
using Assets.Data.Levels;
using Assets.Scripts;
using Assets.Scripts.DataStructures;
using DG.Tweening;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Player _player;
    private bool _isJumping;
    private bool _isSimulating;
    private Coroutine JumpAnimationCoroutine;

    void Awake()
    {
        EventManager.OnPlayerSpawned += OnPlayerSpawned;
        EventManager.OnPlayerReady += AnimatePlayerReady;
        EventManager.OnStopButtonClicked += OnSimulationEnded;
        EventManager.OnSimulate += OnSimulationStarted;

        _isJumping = false;
    }

    public void OnSimulationEnded()
    {
        _isSimulating = false;
    }

    public void OnSimulationStarted(LevelData lvlData, List<TGEPlayer> players)
    {
        _isSimulating = true;
        StopJumping();
    }

    void OnPlayerSpawned(Player player)
    {
        this._player = player;
        player.OnMoveTo += AnimateMoveTo;
        player.OnTurn += AnimateTurn;
        player.OnWait += AnimateWait;
        player.OnInteract += AnimateInteract;
        player.OnFailMoveTo += AnimateFailedMove;
    }

    public void AnimateMoveTo(Vector3 to)
    {
        transform.DOMove(to, 1f);
    }

    public void AnimateTurn(Vector3 targetRotation)
    {
        transform.DORotate(targetRotation, 1f);
    }

    // Bump into wall or player
    public void AnimateFailedMove(Vector3 targetDestination)
    {
        Vector3 bumpPosition = (transform.position + targetDestination) / 2;

        StartCoroutine(RunBumpAnimation(transform.position, bumpPosition));
    }

    // Ready and waiting
    public void AnimatePlayerReady(bool isReady)
    {
        if (isReady)
        {
            if (!_isJumping && !_isSimulating)
                JumpAnimationCoroutine = StartCoroutine(RunJumpAnimation());
        }
        else
        {
            StopJumping();
        }
    }

    private IEnumerator RunJumpAnimation()
    {
        _isJumping = true;
        while (_isJumping)
        {
            for (float i = 0.5f; i > 0; i -= 0.3f)
            {
                Tween jump = transform.DOJump(transform.position, i, 1, i/3);
                yield return jump.WaitForCompletion();
            }

            yield return new WaitForSeconds(1);
        }
    }

    private void StopJumping()
    {
        if (JumpAnimationCoroutine == null) return;

        StopCoroutine(JumpAnimationCoroutine);
        _isJumping = false;
    }

    private IEnumerator RunBumpAnimation(Vector3 startPosition, Vector3 bumpPosition)
    {
        Tween move = transform.DOMove(bumpPosition, 0.5f);
        yield return move.WaitForCompletion();
        transform.DOShakeScale(0.3f, 0.2f, 30, 20f);
        transform.DOMove(startPosition, 0.5f);
    }

    public void AnimateInteract()
    {

    }

    public void AnimateBumpIntoWall()
    {

    }

    public void AnimateBumpIntoPlayer()
    {

    }

    public void AnimateWait()
    {

    }
}
