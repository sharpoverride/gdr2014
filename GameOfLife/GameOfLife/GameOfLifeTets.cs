using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfLife
{
    [TestClass]
    public class GameOfLifeTests
    {
        [TestMethod]
        public void Run_OneCellIsAlive_WhenRegistered()
        {
            var game = new GameOfLife();

            game.Register(new Cell(1, 1));

            Assert.IsTrue(game.isAliveAt(1,1));
        }
    }

    public class GameOfLife
    {
        public GameOfLife()
        {

        }

        public void Register(Cell cell)
        {
        }

        public bool isAliveAt(int x, int y)
        {
            return true;
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
    }
}
