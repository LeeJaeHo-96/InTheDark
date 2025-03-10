using System.Collections.Generic;
using UnityEngine;

public class BSPDungeonCreater : MonoBehaviour
{
    private Vector3 dungeonSize = new Vector3(30, 1, 30); // 던전 전체 크기
    private BSPNode rootNode;

    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private GameObject corridorPrefab;

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
            Random.Range(6, node.Area.size.z - 6) :
            Random.Range(6, node.Area.size.x - 6);

        if (splitHorizontally)
        {
            node.Left = new BSPNode(new Bounds(
                node.Area.center + new Vector3(0, 0, -splitPoint / 2),
                new Vector3(node.Area.size.x, 1, splitPoint)));

            node.Right = new BSPNode(new Bounds(
                node.Area.center + new Vector3(0, 0, splitPoint / 2),
                new Vector3(node.Area.size.x, 1, node.Area.size.z - splitPoint)));
        }
        else
        {
            node.Left = new BSPNode(new Bounds(
                node.Area.center + new Vector3(-splitPoint / 2, 0, 0),
                new Vector3(splitPoint, 1, node.Area.size.z)));

            node.Right = new BSPNode(new Bounds(
                node.Area.center + new Vector3(splitPoint / 2, 0, 0),
                new Vector3(node.Area.size.x - splitPoint, 1, node.Area.size.z)));
        }

        Split(node.Left);
        Split(node.Right);
    }

    void CreateRooms(BSPNode node)
    {
        if (node.Left == null && node.Right == null) // Leaf 노드일 경우 방 생성
        {
            float roomWidth = Mathf.Min(6, node.Area.size.x - 2);
            float roomDepth = Mathf.Min(6, node.Area.size.z - 2);

            node.Room = new Bounds(node.Area.center, new Vector3(roomWidth, 5, roomDepth));
            return;
        }

        if (node.Left != null) CreateRooms(node.Left);
        if (node.Right != null) CreateRooms(node.Right);
    }

    void RenderDungeon(BSPNode node)
    {
        if (node == null) return;

        if (node.Room != null)
            CreateRoom(node.Room);

        RenderDungeon(node.Left);
        RenderDungeon(node.Right);
    }

    void CreateRoom(Bounds room)
    {
        if (rooms == null || rooms.Count == 0)
        {
            Debug.LogError("방 프리팹이 없습니다!");
            return;
        }

        GameObject roomPrefab = rooms[Random.Range(0, rooms.Count)];
        GameObject newRoom = Instantiate(roomPrefab, room.center, Quaternion.identity);
        newRoom.transform.localScale = new Vector3(room.size.x, 5, room.size.z);
    }
}