using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife
{
    [TestClass]
    public class GameOfLifeTests
    {
        [TestMethod]
        public void Register_OneCellIsAlive_WhenRegistered()
        {
            var game = new GameOfLife();

            game.Register(new Cell(1, 1));

            Assert.IsTrue(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void IsAlive_NoInitialPopulation_ReturnsFalse()
        {
            var game = new GameOfLife();

            Assert.IsFalse(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_WithSingleCell_CellIsDead()
        {
            var game = new GameOfLife();

            game.Register(new Cell(1, 1));

            game.Run();

            Assert.IsFalse(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_CellWithOnlyOneNeighbour_CellIsDead()
        {
            var game = new GameOfLife();

            game.Register(new Cell(0, 1));
            game.Register(new Cell(1, 1));
            game.Register(new Cell(2, 5));

            game.Run();

            Assert.IsFalse(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_CellWithTwoNeighbors_CellIsAlive()
        {
            var game = new GameOfLife();

            game.Register(new Cell(0, 1));
            game.Register(new Cell(1, 1));
            game.Register(new Cell(2, 1));

            game.Run();

            Assert.IsTrue(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_CellWithThreeNeighbors_CellIsAlive()
        {
            var game = new GameOfLife();

            game.Register(new Cell(0, 1));
            game.Register(new Cell(1, 1));
            game.Register(new Cell(2, 1));
            game.Register(new Cell(1, 0));

            game.Run();

            Assert.IsTrue(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_CellWithFourNeighbors_CellIsDead()
        {
            var game = new GameOfLife();

            game.Register(new Cell(0, 1));
            game.Register(new Cell(1, 1));
            game.Register(new Cell(2, 1));
            game.Register(new Cell(2, 0));
            game.Register(new Cell(1, 0));

            game.Run();

            Assert.IsFalse(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_DeadCell11WithExactlyThreeNeighbors_CellIsResurected()
        {
            var game = new GameOfLife();

            game.Register(new Cell(0, 1));
            game.Register(new Cell(2, 1));
            game.Register(new Cell(2, 0));

            game.Run();

            Assert.IsTrue(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_DeadCell22WithExactlyThreeNeighbors_CellIsResurected()
        {
            var game = new GameOfLife();

            game.Register(new Cell(1, 2));
            game.Register(new Cell(3, 2));
            game.Register(new Cell(3, 1));

            game.Run();

            Assert.IsTrue(game.isAliveAt(2, 2));
        }
    }

    public class GameOfLife
    {
        List<Cell> _cells = new List<Cell>();

        public void Register(Cell cell)
        {
            _cells.Add(cell);
        }

        public bool isAliveAt(int x, int y)
        {
            return _cells.Contains(new Cell(x, y));
        }

        public void Run()
        {
            var resurectedCells = GenerateDeadNeighbours().Where(CanBeResurected);
            var survivingCells = _cells.Where(CanSurvive);

            _cells = survivingCells.Union(resurectedCells).ToList();
        }

        private IEnumerable<Cell> GenerateDeadNeighbours()
        {
            return _cells.SelectMany(GetCellDeadNeighbours);
        }

        private IEnumerable<Cell> GetCellDeadNeighbours(Cell cell)
        {
            for (int i = cell.X - 1; i < cell.X + 1; i++)
            {
                for (int j = cell.Y - 1; j < cell.Y + 1; j++)
                {
                    if (isAliveAt(i, j) == false)
                    {
                        yield return new Cell(i, j); 
                    }
                }                
            }
        }

        private bool CanSurvive(Cell arg)
        {
            var count = GetAliveNeighbourCount(arg);

            return count == 2 || count == 3;
        }

        private bool CanBeResurected(Cell arg)
        {
            var count = GetAliveNeighbourCount(arg);

            return count == 3;
        }

        private int GetAliveNeighbourCount(Cell arg)
        {
            var count = _cells.Count(cell =>
            {
                var dx = Math.Abs(cell.X - arg.X);
                var dy = Math.Abs(cell.Y - arg.Y);

                return (dx <= 1 && dy <= 1) && !IsSameCell(dx, dy);
            });
            return count;
        }

        private bool IsSameCell(int dx, int dy)
        {
            return (dx == 0 && dy == 0);
        }
    }

    public class Cell
    {
        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int Y { get; set; }

        public int X { get; set; }

        public override bool Equals(object obj)
        {
            var cell = (Cell)obj;

            if (cell == null)
            {
                return false;
            }
            return cell.X == X && cell.Y == Y;
        }
    }
}
