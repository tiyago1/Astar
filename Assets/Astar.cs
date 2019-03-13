using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar : MonoBehaviour
{

    [Header("Junk")]
    public GameObject CellPrefab;
    public Transform ParentTransform;

    [Header("A*")]
    public Vector2 StartPosition;
    public Vector2 TargetPosition;
    public Vector2 MapSize;
    public int G;

    private List<Cell> OpenList;
    private List<Cell> ClosedList;

    private List<Cell> AllCells;

    public void Start()
    {
        AllCells = new List<Cell>();
        OpenList = new List<Cell>();
        ClosedList = new List<Cell>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            for (int x = 0; x < MapSize.x; x++)
            {
                for (int y = 0; y < MapSize.y; y++)
                {
                    GameObject cellObject = Instantiate(CellPrefab, ParentTransform);
                    cellObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x * 50, y * 50);
                    Cell cell = cellObject.GetComponent<Cell>();
                    cell.Init(new Vector2(x, y));
                    cell.IsWalkable = true;
                    AllCells.Add(cell);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PathFind());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Test());
        }
    }

    private IEnumerator Test()
    {
        Cell parentCell = ClosedList.First(it => it.Position == TargetPosition);
        do
        {
            parentCell.SetColor(Color.blue);
            parentCell = parentCell.ParentCell;

        }
        while (parentCell != null);
        yield return null;
    }

    public IEnumerator PathFind()
    {
        Cell currentCell = AllCells.First(it => it.Position == StartPosition);
        OpenList.Add(currentCell);
        while (true)
        {
            if (ClosedList.Contains(AllCells.First(it => it.Position == TargetPosition)))
                break;
            double minFValue = OpenList.Min(it => it.F);
            currentCell = OpenList.First(it => it.F == minFValue);
            currentCell.SetColor(Color.red);
            ClosedList.Add(currentCell);
            OpenList.Remove(currentCell);
            yield return new WaitForEndOfFrame();
            List<Cell> adjacentCells = GetAdjacentOfTheCell(currentCell);
            G++;

            foreach (Cell adjacentCell in adjacentCells)
            {
                if (!adjacentCell.IsWalkable || ClosedList.Contains(adjacentCell))
                {
                    continue;
                }

                if (!OpenList.Contains(adjacentCell))
                {
                    adjacentCell.G = G;
                    adjacentCell.H = ComputeHScore(currentCell.Position, adjacentCell.Position);
                    adjacentCell.F = adjacentCell.G + adjacentCell.H;
                    adjacentCell.ParentCell = currentCell;

                    OpenList.Add(adjacentCell);
                }
                else
                {
                    if (G + adjacentCell.H < adjacentCell.F)
                    {
                        adjacentCell.G = G;
                        adjacentCell.F = adjacentCell.G + adjacentCell.H;
                        adjacentCell.ParentCell = currentCell;
                    }
                }
            }
        }
    }

    private List<Cell> GetAdjacentOfTheCell(Cell currentCell)
    {
        List<Cell> adjacentOfTheCell = new List<Cell>()
        {
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x - 1, currentCell.Position.y + 1)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x, currentCell.Position.y + 1)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x + 1, currentCell.Position.y + 1)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x - 1, currentCell.Position.y)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x + 1, currentCell.Position.y)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x - 1, currentCell.Position.y - 1)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x, currentCell.Position.y - 1)),
            AllCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x + 1, currentCell.Position.y - 1))
        };

        adjacentOfTheCell.RemoveAll(it => it == null);
      
        return adjacentOfTheCell;
    }

    static int ComputeHScore(Vector2 currentPosition, Vector2 targetPosition)
    {
        return Math.Abs((int)targetPosition.x - (int)currentPosition.x) + Math.Abs((int)targetPosition.y - (int)currentPosition.y);
    }
}
