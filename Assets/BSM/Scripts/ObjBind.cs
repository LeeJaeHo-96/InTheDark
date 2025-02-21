using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBind : MonoBehaviour
{

    private Dictionary<string, GameObject> _gameObjectDict;
    private Dictionary<(string, System.Type), Component> _componentDict;


    protected void Bind()
    {

        Transform[] transforms = GetComponentsInChildren<Transform>(true);

        _gameObjectDict = new Dictionary<string, GameObject>(transforms.Length << 2);

        foreach (Transform child in transforms)
        {
            _gameObjectDict.TryAdd(child.gameObject.name, child.gameObject);
            
        }
        
        _componentDict = new Dictionary<(string, System.Type), Component>();
    }

    protected GameObject GetGameObjectBind(in string name)
    {
        _gameObjectDict.TryGetValue(name, out GameObject obj);
        return obj;
    }

    protected T GetComponentBind<T>(in string name) where T : Component
    {
        (string, System.Type) key = (name, typeof(T));

        _componentDict.TryGetValue(key, out Component component);

        if (component != null) return component as T;

        _gameObjectDict.TryGetValue(name, out GameObject go);

        if (go != null)
        {
            component = go.GetComponent<T>();

            if (component != null)
            {
                _componentDict.TryAdd(key, component);
                return component as T;
            } 
        }

        return null;
    }
    
}
