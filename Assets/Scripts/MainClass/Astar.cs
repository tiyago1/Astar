using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace XAstar
{
    public class Astar
    {
        private List<Cell> openList;
        private List<Cell> closedList;
        private List<Cell> allCells;

        //public void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.C))
        //    {
        //        CreateMap(MapSize);
        //    }

        //    if (Input.GetKeyDown(KeyCode.E))
        //    {
        //        PathFind(StartPosition, TargetPosition);
        //    }

        //    if (Input.GetKeyDown(KeyCode.L))
        //    {
        //    }
        //}

        public void CreateMap(Vector2 mapSize)
        {
            allCells = new List<Cell>();
            openList = new List<Cell>();
            closedList = new List<Cell>();

            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    Cell cell = new Cell(new Vector2(x, y));
                    cell.SetWalkablity(true);
                    allCells.Add(cell);
                }
            }
        }

        public List<Cell> PathFind(Vector2 startPosition, Vector2 targetPosition)
        {
            Cell currentCell = allCells.First(it => it.Position == startPosition);
            openList.Add(currentCell);

            while (true)
            {
                if (closedList.Contains(allCells.First(it => it.Position == targetPosition)))
                    break;

                int f = openList.Min(it => it.F);
                currentCell = openList.First(it => it.F == f);
                closedList.Add(currentCell);
                openList.Remove(currentCell);
                List<Cell> adjacentCells = GetAdjacentOfTheCell(currentCell);

                foreach (Cell adjacentCell in adjacentCells)
                {
                    if (!adjacentCell.IsWalkable || closedList.Contains(adjacentCell))
                    {
                        continue;
                    }

                    if (!openList.Contains(adjacentCell))
                    {
                        adjacentCell.ParentCell = currentCell;

                        adjacentCell.G = currentCell.G + 1;
                        adjacentCell.H = ComputeHScore(adjacentCell.Position, targetPosition);
                        adjacentCell.F = adjacentCell.G + adjacentCell.H;

                        openList.Add(adjacentCell);
                    }
                    else
                    {
                        if ((currentCell.G + 1) + adjacentCell.H < adjacentCell.F)
                        {
                            adjacentCell.G = currentCell.G + 1;
                            adjacentCell.F = adjacentCell.G + adjacentCell.H;
                            adjacentCell.ParentCell = currentCell;
                            openList.Add(adjacentCell);
                        }
                    }
                }
            }

            return GetPath(targetPosition);
        }

        private List<Cell> GetPath(Vector2 targetPosition)
        {
            List<Cell> path = new List<Cell>();
            Cell parentCell = closedList.First(it => it.Position == targetPosition);
            do
            {
                path.Add(parentCell);
                parentCell = parentCell.ParentCell;
            }
            while (parentCell != null);

            return path;
        }

        private List<Cell> GetAdjacentOfTheCell(Cell currentCell)
        {
            List<Cell> adjacentOfTheCell = new List<Cell>()
            {
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x - 1, currentCell.Position.y + 1)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x, currentCell.Position.y + 1)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x + 1, currentCell.Position.y + 1)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x - 1, currentCell.Position.y)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x + 1, currentCell.Position.y)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x - 1, currentCell.Position.y - 1)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x, currentCell.Position.y - 1)),
                allCells.FirstOrDefault(it => it.Position == new Vector2(currentCell.Position.x + 1, currentCell.Position.y - 1))
            };

            adjacentOfTheCell.RemoveAll(it => it == null);

            return adjacentOfTheCell;
        }

        private int ComputeHScore(Vector2 currentPosition, Vector2 targetPosition)
        {
            return (int)Vector2.Distance(currentPosition, targetPosition);
        }
    }
}