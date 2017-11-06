using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    public class Board
    {
        public Tile[,] Tiles { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public Texture2D TileTexture { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private Random _rnd = new Random();
        public static Board CurrentBoard { get; private set; }

        public Board(SpriteBatch spritebatch, Texture2D tileTexture, int columns, int rows)
        {
            Columns = columns;
            Rows = rows;
            TileTexture = tileTexture;
            SpriteBatch = spritebatch;
            Tiles = new Tile[Columns, Rows];
            CreateNewBoard();
            Board.CurrentBoard = this;
        }

        public void CreateNewBoard()
        {
            InitializeAllTilesAndBlockSomeRandomly();
            SetAllBorderTilesBlocked();
            SetTopLeftTileUnblocked();
        }

        private void SetTopLeftTileUnblocked()
        {
            Tiles[1, 1].IsBlocked = false;
        }

        private void InitializeAllTilesAndBlockSomeRandomly()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Vector2 tilePosition = new Vector2(x * TileTexture.Width, y * TileTexture.Height);
                    Tiles[x, y] = new Tile(TileTexture, tilePosition, SpriteBatch, _rnd.Next(5) == 0);
                }
            }
        }

        private void SetAllBorderTilesBlocked()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    if (x == 0 || x == Columns - 1 || y == 0 || y == Rows - 1)
                    { Tiles[x, y].IsBlocked = true; }
                }
            }
        }

        public void Draw()
        {
            foreach (var tile in Tiles)
            {
                tile.Draw();
            }
        }

        public bool HasRoomForRectangle(Rectangle rectangleToCheck)
        {
            foreach (var tile in Tiles)
            {
                if (tile.IsBlocked && tile.Bounds.Intersects(rectangleToCheck))
                {
                    return false;
                }
            }
            return true;
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

        private Rectangle CreateRectangleAtPosition(Vector2 positionToTry, int width, int height)
        {
            return new Rectangle((int)positionToTry.X, (int)positionToTry.Y, width, height);
        }

        private Vector2 CheckPossibleNonDiagonalMovement(MovementWrapper wrapper, int i)
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
    }
}