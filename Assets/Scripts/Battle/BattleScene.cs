using SUMessage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleScene : GameScene
{
   
    public BattleScene(SceneEnum scene, string sceneName) : base(scene, sceneName) { }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode);
        BattleManager.instance.Start();
    }

    public override  void OnExit()
    {
        BattleManager.instance.Destroy();
    }

    public override void OnUpdate(float deltaTime)
    {
        BattleManager.instance.Update(deltaTime);
    }



}

