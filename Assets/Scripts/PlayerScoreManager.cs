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
            {
                EventManager.OnAllLevelGoalsReached += DetermineMultiplayerScore;
                EventManager.OnAllLevelGoalsReached -= DetermineMultiplayerScore;
            }
            else
            {
                EventManager.OnAllLevelGoalsReached += DetermineSinglePlayerScore;
                EventManager.OnAllLevelGoalsReached -= DetermineSinglePlayerScore;
            }
        };        
    }

    public void DetermineSinglePlayerScore()
    {
        var player = _gameInfo.LocalPlayer.Player.Sequence;
        
        var score = DeterminePlayerScore(player, 0);
        var levelScore = _gameInfo.Level.LevelScore;

        

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
        var totalCount = 0;
        foreach(var player in _gameInfo.Players)
        {
            var count = DeterminePlayerScore(player.Player.Sequence, 0);
            var stars = DetermineStars(count, _gameInfo.Level.LevelScore);
        }

        
    }

    private int DetermineStars(int count, LevelScore levelScore)
    {
        if(count <= levelScore.HighestScore)
            return 3;
        else if(count <= levelScore.DecentScore && count > levelScore.HighestScore)
            return 2;
        else if(count >= levelScore.BadScore)
            return 1;
        else
            return -1;
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
