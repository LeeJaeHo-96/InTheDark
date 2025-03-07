using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPNode
{
    public Bounds Area; //노드가 차지하는 공간
    public BSPNode Left, Right; //나눠진 하위 노드들
    public Bounds Room; //실제 방의 크기

    public BSPNode(Bounds area)
    {
        Area = area;
    }
}
