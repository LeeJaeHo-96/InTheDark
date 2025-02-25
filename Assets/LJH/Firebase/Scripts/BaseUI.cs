using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUI : MonoBehaviour
{
    //string Key로 게임 오브젝트 Value를 가져올 딕셔너리

    protected Dictionary<string, GameObject> _gameObjectDict;

    //string Key로 Component Value를 가져올 딕셔너리

    private Dictionary<(string, System.Type), Component> _componentDict;

    protected void Bind()
    {

        //모든 UI 요소들을 가져옴

        //Transform으로 가져오는 이유는 Transform이 없는 오브젝트는 없기 때문에.

        //비활성화 되어 있는 오브젝트는 가져오지 않는게 기본 설정이라 true를 할당

        Transform[] _transforms = GetComponentsInChildren<Transform>(true);

        //딕셔너리의 용량은 _transforms 배열의 4배로 설정

        _gameObjectDict = new Dictionary<string, GameObject>(_transforms.Length << 2);

        foreach (Transform child in _transforms)
        {
            //똑같은 이름이 있는 것을 생각해서 Try로 추가
            _gameObjectDict.TryAdd(child.gameObject.name, child.gameObject);
        }

        _componentDict = new Dictionary<(string, System.Type), Component>();

    }

    /// <summary>
    /// 로딩 과정에서 바인딩 할 수 있으면 BindAll
    /// </summary>

    protected void BindAll()
    {
        Transform[] _transforms = GetComponentsInChildren<Transform>(true);

        //딕셔너리의 용량은 _transforms 배열의 4배로 설정

        _gameObjectDict = new Dictionary<string, GameObject>(_transforms.Length << 2);

        foreach (Transform child in _transforms)
        {
            //똑같은 이름이 있는 것을 생각해서 Try로 추가
            _gameObjectDict.TryAdd(child.gameObject.name, child.gameObject);

        }

        Component[] components = GetComponentsInChildren<Component>(true);

        _componentDict = new Dictionary<(string, System.Type), Component>(components.Length << 4);

        foreach (Component child in components)
        {
            _componentDict.TryAdd((child.gameObject.name, components.GetType()), child);

        }

    }



    //이름이 name인 UI 게임오브젝트 가져오기

    //GetUI("Key 01") > Key 01 인 게임오브젝트 가져오기

    public GameObject GetUI(in string name)
    {
        _gameObjectDict.TryGetValue(name, out GameObject obj);

        return obj;
    }

    //이름이 Name인 UI에서 컴포넌트 T 가져오기

    //GetUI<Image>("Key") : Key이름의 게임오브젝트에서 Image 컴포넌트 가져옴

    public T GetUI<T>(in string name) where T : Component
    {
        //클래스의 이름을 typeOf(T).Name 으로 찾을 수 있음

        //컴포넌트 딕셔너리 키로 사용할 튜플
        (string, System.Type) _key = (name, typeof(T));

        // 1. Component 딕셔너리에 없을 때 (찾아본적이 없는 상태) > 찾은 후 딕셔너리에 추가하고 줌
        _componentDict.TryGetValue(_key, out Component component);

        if (component != null)
            return component as T;

        // 2. Component 딕셔너리에 이미 있을 때 (찾아본적이 있는 상태) > 찾았던걸 줌

        _gameObjectDict.TryGetValue(name, out GameObject go);

        if (go != null)
        {
            component = go.GetComponent<T>();

            if (component != null)
            {
                _componentDict.TryAdd(_key, component);
                return component as T;
            }
        }
        return null;
    }

}
