using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using DG.Tweening;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private Player _player;

    void Awake()
    {
        EventManager.OnPlayerSpawned += OnPlayerSpawned;
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
