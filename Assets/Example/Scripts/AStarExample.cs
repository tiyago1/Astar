using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using XAstar;


public class AStarExample : MonoBehaviour
{
    [SerializeField] private Tilemap tileMap;

    [SerializeField] private Vector2 mapSize;

    public Tile white;
    public Tile blue;
    public Tile red;
    public Tile green;

    private Astar astar;


    private void Awake()
    {
        astar = new Astar();
        astar.CreateMap(mapSize);
    }

    private void Start()
    {
        RefreshMap();
        StartCoroutine(Test());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RefreshMap();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Test());
        }
    }

    private void Test2()
    {
        RefreshMap();

        Astar star = new Astar();
        star.CreateMap(mapSize);

        Vector2 startPos = new Vector2(Random.Range(0, 10), Random.Range(0, 10));
        Vector2 endPos = new Vector2(Random.Range(0, 10), Random.Range(0, 10));

        tileMap.SetTile(new Vector3Int((int)startPos.x, (int)startPos.y, 0), red);
        tileMap.SetTile(new Vector3Int((int)endPos.x, (int)endPos.y, 0), blue);

        List<Vector2> path = star.PathFind(startPos, endPos).Select(it => it.Position).ToList();

        path.Remove(startPos);
        path.Remove(endPos);

        foreach (Vector2 position in path)
        {
            tileMap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), green);
        }
    }

    private IEnumerator Test()
    {
        Vector2 startPos;
        Vector2 endPos;
        while (true)
        {
            RefreshMap();
            Astar star = new Astar();
            star.CreateMap(mapSize);

            startPos = new Vector2(Random.Range(0, 10), Random.Range(0, 10));
            endPos = new Vector2(Random.Range(0, 10), Random.Range(0, 10));

            tileMap.SetTile(new Vector3Int((int)startPos.x, (int)startPos.y, 0), red);
            tileMap.SetTile(new Vector3Int((int)endPos.x, (int)endPos.y, 0), blue);

            yield return new WaitForSeconds(1f);

            List<Vector2> path = star.PathFind(startPos, endPos).Select(it => it.Position).ToList();

            path.Remove(startPos);
            path.Reverse();

            foreach (Vector2 position in path)
            {
                tileMap.SetTile(new Vector3Int((int)startPos.x, (int)startPos.y, 0), white);
                tileMap.SetTile(new Vector3Int((int)position.x, (int)position.y, 0), red);
                startPos = position;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void RefreshMap()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                tileMap.SetTile(new Vector3Int(x, y, 0), white);
            }
        }
    }

}
