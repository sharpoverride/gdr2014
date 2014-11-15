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

            Assert.IsTrue(game.isAliveAt(1,1));
        }

        [TestMethod]
        public void IsAlive_NoInitialPopulation_ReturnsFalse()
        {
            var game = new GameOfLife();

            Assert.IsFalse(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_KillsCell_WithNoNeighbors()
        {
            var game = new GameOfLife();

            game.Register(new Cell(1, 1));

            game.Run();

            Assert.IsFalse(game.isAliveAt(1, 1));
        }

        [TestMethod]
        public void Run_KeppsCells_WithTwoNeighbors()
        {
            var game = new GameOfLife();

            game.Register(new Cell(0, 1));
            game.Register(new Cell(1, 1));
            game.Register(new Cell(2, 1));

            game.Run();

            Assert.IsTrue(game.isAliveAt(1, 1));
        }

    }

    public class GameOfLife
    {
        List<Cell> _cells = new List<Cell>();
        public GameOfLife()
        {

        }

        public void Register(Cell cell)
        {
            _cells.Add(cell);
        }

        public bool isAliveAt(int x, int y)
        {
            return _cells.Contains(new Cell(x,y));
        }

        public void Run()
        {
            var survivingCells = _cells.Where(HasTwoNeighbours).ToList();

            _cells = survivingCells;
        }

        private bool HasTwoNeighbours(Cell arg)
        {
            var count = _cells.Count(cell =>
            {
                var dx = Math.Abs(cell.X - arg.X);
                var dy = Math.Abs(cell.Y - arg.Y);

                return dx == 1 || dy == 1;
            });
            
            return count == 2;
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
