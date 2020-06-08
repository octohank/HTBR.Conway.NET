﻿namespace Conway.Engine.Tests
{
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [TestFixture]
    public class FertilityMatrixTests
    {
        private Matrix _matrix;
        private FertilityMatrix _sut;

        [SetUp]
        public void Setup()
        {
            _matrix = new Matrix(3, 3);
        }

        [Test]
        public void TestThreeNeighbours()
        {
            _matrix.AddCell(new Cell(0, 0));
            _matrix.AddCell(new Cell(0, 2));
            _matrix.AddCell(new Cell(2, 2));

            _sut = new FertilityMatrix(_matrix);


            IEnumerable<Cell> sprouts = _sut.GetSprouts();
            sprouts.Should().HaveCount(1);
        }
    }

    public class FertilityMatrix
    {
        private const int NumberOfNeighboursToBecomeAlive = 3;
        private int[,] _fertility;

        public FertilityMatrix(Matrix matrix)
        {
            _fertility = new int[matrix.Rows, matrix.Columns];
            ApplyFertilityIndex(matrix.GetLivingCells());
        }


        private void ApplyFertilityIndex(IEnumerable<Cell> cells)
        {
            foreach (var cell in cells)
            {
                int iStart = Math.Abs(cell.X - 1);
                int jStart = Math.Abs(cell.Y - 1);
                int iEnd = Math.Min(cell.X + 1, _fertility.GetLength(0) - 1);
                int jEnd = Math.Min(cell.Y + 1, _fertility.GetLength(1) - 1);

                for (int i = iStart; i <= iEnd; i++)
                {
                    for (int j = jStart; j <= jEnd; j++)
                    {
                        if (i == cell.X && j == cell.Y) // set cell itself to max value
                            _fertility[cell.X, cell.Y] = int.MaxValue;

                        if (_fertility[i, j] == int.MaxValue) // a living cell can't be dead but fertile ground
                            continue;

                        _fertility[i, j]++; // raise fertility index
                    }
                }
            }
        }

        public IEnumerable<Cell> GetSprouts()
        {
            for (int i = 0; i < _fertility.GetLength(0); i++)
            {
                for (int j = 0; j < _fertility.GetLength(1); j++)
                {
                    if(_fertility[i,j] == NumberOfNeighboursToBecomeAlive)
                        yield return new Cell(i,j);
                }
            }
        }
    }
}