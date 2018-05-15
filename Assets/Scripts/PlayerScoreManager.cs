using UnityEngine;
using System.Collections;
using Assets.Scripts;
using static Assets.Scripts.GlobalData;
using UnityEngine.Assertions;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.DataStructures;

public class PlayerScoreManager : MonoBehaviour
{
    private GameInfo _gameInfo;
    public static PlayerScoreManager Instance { get; private set; }

    public void Start()
    {
        Instance = this;
        SceneDataLoader.OnSceneLoaded += gameInfo =>
        {
            this._gameInfo = gameInfo;
            if(_gameInfo.IsMultiplayer)
                EventManager.OnAllLevelGoalsReached += DetermineMultiplayerScore;
            else
                EventManager.OnAllLevelGoalsReached += DetermineSinglePlayerScore;
        };        
    }

    public void DetermineSinglePlayerScore()
    {
        var player = _gameInfo.LocalPlayer.Player.Sequence;
        
        var score = DeterminePlayerScore(player, 0);
        print($"{nameof(PlayerScoreManager)}: SinglePlayer score is: {score}");   
    }

    public int DeterminePlayerScore(Sequence sequence, int count)
    {
        Assert.IsNotNull(sequence);

        foreach(var bc in sequence)
            if(!bc.CountsTowardsScore)
                if(bc is LoopCommand)
                    count += DeterminePlayerScore(((LoopCommand)bc).Sequence, 0);
            else if(bc.CountsTowardsScore)
                count += 1;

        return count;
    }

    public void DetermineMultiplayerScore()
    {
        foreach(TGEPlayer p in _gameInfo.Players)
        {
            int count = DeterminePlayerScore(p.Player.Sequence, 0);
        }
    }

    #region DEPRECATED
    public int DetermineLoopCount(Sequence seq, int count)
    {
        foreach(BaseCommand bc in seq)
        {
            if(bc is LoopCommand)
                DetermineLoopCount(((LoopCommand)bc).Sequence, count);
            else if(bc.CountsTowardsScore)
                count = count + 1;
        }
        return count;
    }
    #endregion
}
