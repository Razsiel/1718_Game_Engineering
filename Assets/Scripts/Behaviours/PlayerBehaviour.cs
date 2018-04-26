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
    }

    public void AnimateMoveTo(Vector3 to)
    {
        transform.DOMove(to, 1f);

        // Visual movement
//        while (!player.transform.position.AlmostEquals(destinationPosition, player.Data.MovementData.OffsetAlmostPosition))
//        {
//            player.transform.position = Vector3.Lerp(
//                player.transform.position,
//                destinationPosition,
//                player.Data.MovementData.MovementSpeed * Time.deltaTime);
//
//            yield return new WaitForEndOfFrame();
//        }
//
//        player.transform.position = destinationPosition;
    }

    public void AnimateTurn(Vector3 targetRotation)
    {
        transform.DORotate(targetRotation, 1f);
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
