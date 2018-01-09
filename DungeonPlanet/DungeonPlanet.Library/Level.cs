using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Drawing;

namespace DungeonPlanet.Library
{

    public class Level
    {
        int _columns;
        int _rows;
        public static int _levelRows;
        public static int _levelColumns;
        Path _path;
        Random _random = new Random();
        public Case[,] Cases { get; private set; }
        public Tile[,] EmptyTiles { get; private set; }
        public static Level CurrentBoard { get; private set; }
        public static State ActualState { get; set; }
        public Hub Hub { get; private set; }
        public enum State
        {
            Menu,
            Hub, 
            LevelOne,
            BossRoom
        }
        public Level(int columns, int rows)
        {
            _columns = columns;
            _rows = rows;

            _levelColumns = columns;
            _levelRows = rows;

            _path = new Path(columns, rows, this);
            Cases = new Case[columns, rows];
            Hub = new Hub(10, 40);
            CurrentBoard = this;
        }

        public void NewLevel()
        {
            if(ActualState == State.Hub || ActualState == State.BossRoom)
            {
                Hub.InitializeAllTilesAndBlockSomeRandomly();
                Hub.SetAllBorderTilesBlocked();
                Hub.SetTopLeftTileUnblocked();
            }
            else if(ActualState == State.LevelOne)
            {
                _path.InitializeBoard();
                _path.CreatePath();
                for (int x = 0; x < _columns; x++)
                {
                    for (int y = 0; y < _rows; y++)
                    {
                        Cases[x, y] = new Case(14, 20, _path.Board[x, y], this, x, y);
                        Cases[x, y].InitializeAllTiles();
                        Cases[x, y].SetBorder();
                        Cases[x, y].PartsAnalysis();
                    }
                }
                EmptyTiles = Spawnable();
            }
        }
        public Tile Emptytile()
        {
            bool flag = true;
            Tile tile;
            while (flag != false)
            {
                tile = EmptyTiles[GetNext(0, EmptyTiles.GetLength(0)), GetNext(0, EmptyTiles.GetLength(1))];
                if (tile != null)
                {
                    return tile;
                }
            }
            return null;
            
        }
        public Tile[,] Spawnable()
        {
            Tile[,] emptyTiles = new Tile[_columns*20,_rows*14];
            foreach(Case Case in Cases)
            {
                foreach(Tile tile in Case.Tiles)
                {
                    if(!tile.IsBlocked)
                    emptyTiles[(int)tile.Position.X / 64, (int)tile.Position.Y / 64] = tile;
                }
            }
            return emptyTiles;
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            if (ActualState == State.Hub)
            {
                foreach (var tile in Hub.Tiles)
                {
                    if (tile.IsBlocked && tile.Bounds.IntersectsWith(rectangleToCheck))
                    {
                        return false;
                    }
                }
                return true;
            }
            else if (ActualState == State.LevelOne)
            {
                foreach (Case Case in Cases)
                {
                    foreach (var tile in Case.Tiles)
                    {
                        if (tile.IsBlocked && tile.Bounds.IntersectsWith(rectangleToCheck))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else return false;
           
        }

        public Vector2 WhereCanIGetTo(Vector2 originalPosition, Vector2 destination, Rectangle boundingRectangle)
        {
            MovementWrapper move = new MovementWrapper(originalPosition, destination, boundingRectangle);

            for (int i = 1; i <= move.NumberOfStepsToBreakMovementInto; i++)
            {
                Vector2 positionToTry = originalPosition + move.OneStep * i;
                Rectangle newBoundary = CreateRectangleAtPosition(positionToTry, boundingRectangle.Width, boundingRectangle.Height);
                if (HasRoomForRectangle(newBoundary)) { move.FurthestAvailableLocationSoFar = positionToTry; }
                else
                {
                    if (move.IsDiagonalMove)
                    {
                        move.FurthestAvailableLocationSoFar = CheckPossibleNonDiagonalMovement(move, i);
                    }
                    break;
                }
            }
            return move.FurthestAvailableLocationSoFar;
        }

        public Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        public Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
        {
            if (wrapper.IsDiagonalMove)
            {
                int stepsLeft = wrapper.NumberOfStepsToBreakMovementInto - (i - 1);

                Vector2 remainingHorizontalMovement = wrapper.OneStep.X * Vector2.UnitX * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingHorizontalMovement, wrapper.BoundingRectangle);

                Vector2 remainingVerticalMovement = wrapper.OneStep.Y * Vector2.UnitY * stepsLeft;
                wrapper.FurthestAvailableLocationSoFar =
                    WhereCanIGetTo(wrapper.FurthestAvailableLocationSoFar, wrapper.FurthestAvailableLocationSoFar + remainingVerticalMovement, wrapper.BoundingRectangle);
            }
            return wrapper.FurthestAvailableLocationSoFar;
        }
        public int GetNext(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
