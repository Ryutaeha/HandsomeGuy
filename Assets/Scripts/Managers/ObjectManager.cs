using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager
{
    public List<Enemy> Enemies { get; private set; } = new();

    public T Spawn<T>(string key, Vector2 position) where T : MonoBehaviour
    {
        System.Type type = typeof(T);

        //if (type == typeof(Player))
        //{
        //    GameObject obj = Main.Resource.Instantiate("Player.prefab");
        //    obj.transform.position = position;

        //    Player = obj.GetOrAddComponent<Player>();
        //    Player.SetInfo(key);

        //    return Player as T;
        //}
        //else if (type == typeof(Enemy))
        //{
        //    CreatureData data = Main.Data.Creatures[key];
        //    GameObject obj = Main.ResourceManager.Instantiate($"{data.prefabName}.prefab", pooling: true);
        //    obj.transform.position = position;

        //    Enemy enemy = obj.GetOrAddComponent<Enemy>();
        //    enemy.SetInfo(key);
        //    Enemies.Add(enemy);

        //    return enemy as T;
        //}
        return null;
    }

    public void Despawn<T>(T obj) where T : MonoBehaviour
    {
        System.Type type = typeof(T);

        //if (type == typeof(Player))
        //{

        //}
        //else if (type == typeof(Enemy))
        //{
        //    Enemies.Remove(obj as Enemy);
        //    Main.Resource.Destroy(obj.gameObject);
        //}
    }
}