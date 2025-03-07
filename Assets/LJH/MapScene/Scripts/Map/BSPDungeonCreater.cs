using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSPDungeonCreater : MonoBehaviour
{
    public Vector3 dungeonSize = new Vector3(50, 5, 50); // 맵 크기
    private BSPNode rootNode;

    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject corriderPrefab;

    private void Start()
    {
        rootNode = new BSPNode(new Bounds(Vector3.zero, dungeonSize));
        Split(rootNode);
        CreateRooms(rootNode);
        RenderDungeon(rootNode);

    }

    void Split(BSPNode node)
    {
        if (node.Area.size.x < 10 || node.Area.size.z < 10)
            return;

        bool splitHorizontally = Random.value > 0.5f;
        float splitPoint = splitHorizontally ?
            Random.Range(5, node.Area.size.z - 5) :
            Random.Range(5, node.Area.size.x - 5);

        if (splitHorizontally)
        {
            node.Left = new BSPNode(new Bounds(
            node.Area.center + new Vector3(0, 0, -splitPoint / 2),
            new Vector3(node.Area.size.x, node.Area.size.y, splitPoint)));

            node.Right = new BSPNode(new Bounds(
                node.Area.center + new Vector3(0, 0, splitPoint / 2),
                new Vector3(node.Area.size.x, node.Area.size.y, node.Area.size.z - splitPoint)));
        }
        else
        {
            node.Left = new BSPNode(new Bounds(
                node.Area.center + new Vector3(-splitPoint / 2, 0, 0),
                new Vector3(splitPoint, node.Area.size.y, node.Area.size.z)));

            node.Right = new BSPNode(new Bounds(
                node.Area.center + new Vector3(splitPoint / 2, 0, 0),
                new Vector3(node.Area.size.x - splitPoint, node.Area.size.y, node.Area.size.z)));
        }

        Split(node.Left);
        Split(node.Right);
    }

    void CreateRooms(BSPNode node)
    {
        if (node.Left == null && node.Right == null) // Leaf 노드인 경우 방 생성
        {
            node.Room = new Bounds(
                node.Area.center,
                new Vector3(Mathf.Max(1, 1), 0.3f, Mathf.Max(1, 1)));
            return;
        }

        if (node.Left != null) CreateRooms(node.Left);
        if (node.Right != null) CreateRooms(node.Right);

        //Todo: 복도는 현재 이상한 상태 추후 수정하여 추가
        //if (node.Left != null && node.Right != null)
        //{
        //    CreateCorridor(node.Left.Room.center, node.Right.Room.center);  // 방과 방을 연결하는 복도 추가
        //}
    }

    void RenderDungeon(BSPNode node)
    {
        if (node == null) return;

        if (node.Room != null)
        {
            //여기서 방 크기에 따라 다른 프리팹을 뽑으면 다른 방들이 나오지 않을까?
            CreateRoom(node.Room);
        }

        RenderDungeon(node.Left);
        RenderDungeon(node.Right);
    }

    void CreateRoom(Bounds room)
    {
        if (roomPrefab == null)
        {
            Debug.LogError("방 프리팹이 할당되지 않았습니다!");
            return;
        }

        GameObject newRoom = Instantiate(roomPrefab, room.center, Quaternion.identity); // 프리팹 생성
        newRoom.transform.localScale = new Vector3(Mathf.Max(room.size.x, 6), 3, Mathf.Max(room.size.z, 6)); // 크기 조정
    }

    void CreateCorridor(Vector3 start, Vector3 end)
    {
        Vector3 corridorPosition = (start + end) / 2;
        Vector3 corridorSize = new Vector3(Mathf.Abs(start.x - end.x) + 2, 0.1f, Mathf.Abs(start.z - end.z) + 2);

        GameObject corridor = Instantiate(corriderPrefab, corridorPosition, Quaternion.identity);
        corridor.transform.position = corridorPosition;
        corridor.transform.localScale = corridorSize;
    }

}
