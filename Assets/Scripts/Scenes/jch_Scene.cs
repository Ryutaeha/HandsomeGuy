using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : BaseScene
{
    public GameObject _playerPrefab;

    protected override bool Initialize()
    {
        if (!base.Initialize()) return false;
        SceneType = Define.Scene.Game;
        test();
        Instantiate(_playerPrefab, null);

        return true;
    }

    public void test()
    {
        Main.ResourceManager.LoadAllAsync<UnityEngine.Object>("testGroup", (key, count, totalCount) =>
        {
            if (count >= totalCount)
            {
                Main.DataManager.Initialize();
            }
        });
    }

    public override void Clear()
    {
        Debug.Log("Clear TestScene");
    }
}
