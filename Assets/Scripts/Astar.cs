using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Astar : MonoBehaviour
{
    [Header("A*")]
    public Vector2 StartPosition;
    public Vector2 TargetPosition;
    public Vector2 MapSize;

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
                    Cell cell = new Cell(new Vector2(x, y));
                    cell.SetWalkablity(true);
                    AllCells.Add(cell);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PathFind(StartPosition, TargetPosition);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
        }
    }

    private List<Cell> GetPath(Vector2 targetPosition)
    {
        List<Cell> path = new List<Cell>();
        Cell parentCell = ClosedList.First(it => it.Position == targetPosition);
        do
        {
            path.Add(parentCell);
            parentCell = parentCell.ParentCell;
        }
        while (parentCell != null);

        return path;
    }

    public List<Cell> PathFind(Vector2 startPosition, Vector2 targetPosition)
    {
        Cell currentCell = AllCells.First(it => it.Position == startPosition);
        OpenList.Add(currentCell);

        while (true)
        {
            if (ClosedList.Contains(AllCells.First(it => it.Position == targetPosition)))
                break;

            int f = OpenList.Min(it => it.F);
            currentCell = OpenList.First(it => it.F == f);
            ClosedList.Add(currentCell);
            OpenList.Remove(currentCell);
            List<Cell> adjacentCells = GetAdjacentOfTheCell(currentCell);

            foreach (Cell adjacentCell in adjacentCells)
            {
                if (!adjacentCell.IsWalkable || ClosedList.Contains(adjacentCell))
                {
                    continue;
                }

                if (!OpenList.Contains(adjacentCell))
                {
                    adjacentCell.ParentCell = currentCell;

                    adjacentCell.G = currentCell.G + 1;
                    adjacentCell.H = ComputeHScore(adjacentCell.Position, targetPosition);
                    adjacentCell.F = adjacentCell.G + adjacentCell.H;

                    OpenList.Add(adjacentCell);
                }
                else
                {
                    if ((currentCell.G + 1) + adjacentCell.H < adjacentCell.F)
                    {
                        adjacentCell.G = currentCell.G + 1;
                        adjacentCell.F = adjacentCell.G + adjacentCell.H;
                        adjacentCell.ParentCell = currentCell;
                        OpenList.Add(adjacentCell);
                    }
                }
            }
        }

        return GetPath(targetPosition);
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
        return (int)Vector2.Distance(currentPosition, targetPosition);
    }
}
