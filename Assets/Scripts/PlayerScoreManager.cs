using UnityEngine;
using System.Collections;
using Assets.Scripts;
using static Assets.Scripts.GlobalData;
using UnityEngine.Assertions;
using System.Linq;
using Assets.Data.Command;
using Assets.Scripts.DataStructures;
using System;
using Assets.Scripts.Lib.Extensions;
using System.Collections.Generic;

public class PlayerScoreManager : MonoBehaviour
{
    private GameInfo _gameInfo;
    public static PlayerScoreManager Instance { get; private set; }

    public void Awake()
    {
        Instance = this;
        print($"{nameof(PlayerScoreManager)}: awake");
        SceneDataLoader.OnSceneLoaded += gameInfo =>
        {
            this._gameInfo = gameInfo;
            EventManager.OnAllLevelGoalsReached += DeterminePlayersScore;          
        };        
    }
  
    public int DetermineSequenceCount(Sequence sequence, int count)
    {
        Assert.IsNotNull(sequence);

        foreach(var bc in sequence)
        {
            print($"{nameof(PlayerScoreManager)}: in foreach with basecommand { bc.GetType().Name }");
            if(!bc.CountsTowardsScore)
            {
                print($"{nameof(PlayerScoreManager)}: in if doesnt count towards score");
                if(bc is LoopCommand)
                    count += DetermineSequenceCount(((LoopCommand)bc).Sequence, 0);
                else if(bc.CountsTowardsScore)
                    count += 1;
            }
        }
        return count;
    }

    public void DeterminePlayersScore()
    {
        EventManager.OnAllLevelGoalsReached -= DeterminePlayersScore;

        print($"{nameof(PlayerScoreManager)}: in {nameof(DeterminePlayersScore)}");
        var starCount = 0;
        var playerScoreDic = new Dictionary<TGEPlayer, int>();

        foreach(var player in _gameInfo.Players)
        {
            print($"{nameof(PlayerScoreManager)}: in foreach");
            var count = DetermineSequenceCount(player.Player.Sequence, 0);
            print($"{nameof(PlayerScoreManager)}: seqCount {count}");
            var stars = DetermineStars(count, _gameInfo.Level.LevelScore);
            print($"{nameof(PlayerScoreManager)}: {stars}");
            Assert.IsTrue(stars > 0 && stars <= 3);
            starCount += stars;
            playerScoreDic.Add(player, stars);
        }

        var averageStars = (decimal)starCount / _gameInfo.Players.Count;
        var combinedStars = MathHelper.RoundDown(averageStars);
        print($"{nameof(PlayerScoreManager)}: {combinedStars}");
        Assert.IsTrue(combinedStars > 0 && combinedStars <= 3);
      
        EventManager.OnPlayersScoreDetermined?.Invoke(playerScoreDic, combinedStars);
    }

    private int DetermineStars(int count, LevelScore levelScore)
    {
        if(count <= levelScore.HighestScore)
            return 3;
        else if(count <= levelScore.DecentScore && count > levelScore.HighestScore)
            return 2;
        else if(count >= levelScore.DecentScore)
            return 1;
        else
            return -1;     
    }
}
