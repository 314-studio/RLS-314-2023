using System;
using GridSystem;
using NUnit.Framework;
using UnityEngine;

namespace Tests.GridSystem.EditMode
{
    [TestFixture]
    public class TestGridBase
    {
        private GridBase<int> _intGrid;
        private readonly  int _width = 20;
        private readonly int _depth = 20;
        private readonly  float _cellSize = 0.5f;
        private readonly Vector3 _leftBottom = Vector3.zero;

        [SetUp]
        public void Setup()
        {
            _intGrid = new GridBase<int>(_width, _depth, _cellSize, _leftBottom);
        }

        [Test]
        public void Test_GetGridSize()
        {
            _intGrid.GetGridSize(out var width, out var depth);
            Assert.AreEqual(_width, width);
            Assert.AreEqual(_depth, depth);
        }

        [Test]
        public void Test_SetValue()
        {
            const int x = 0;
            const int y = 0;
            const int value = 5;
            _intGrid.SetValue(x, y, value);
            Assert.AreEqual(_intGrid.gridArray[x, y], value);
        }

        [Test]
        public void Test_SetValueOutOfBoundsShouldNotThrowError()
        {
            const int value = 5;
            try
            {
                _intGrid.SetValue(_width, _depth, value);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void Test_SetValueByWorldPosition()
        {
            var point = Vector3.one * 4.6f;
            const int value = -1;
            int x = Mathf.FloorToInt((point / _cellSize).x);
            int y = Mathf.FloorToInt((point / _cellSize).z);
            _intGrid.SetValue(point, value);
            Assert.AreEqual(_intGrid.gridArray[x, y], value);
        }
        
        [Test]
        public void Test_SetValueByWorldPositionOutOfBoundsShouldNotThrowError()
        {
            var point = new Vector3(_width, 0, _depth) * 4.6f * _cellSize;
            const int value = -1;
            try
            {
                _intGrid.SetValue(point, value);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Test]
        public void Test_SetValueMultiple()
        {
            const int x = 6;
            const int y = 6;
            const int expandX = 6;
            const int expandY = 6;
            const int value = 666;
            _intGrid.SetValueMultiple(x, y, expandX, expandY, value);
            Assert.AreEqual(value, _intGrid.gridArray[x, y]);
            Assert.AreEqual(value, _intGrid.gridArray[x + expandX - 1, y + expandY - 1]);
        }

        [Test]
        public void Test_GetValue()
        {
            const int x = 6;
            const int y = 6;
            const int value = 666;
            _intGrid.SetValue(x, y, value);
            _intGrid.gridArray[x + 1, y + 1] = value;
            Assert.AreEqual(value, _intGrid.GetValue(x, y));
            Assert.AreEqual(value, _intGrid.GetValue(x + 1, y + 1));
            Assert.AreNotEqual(value, _intGrid.GetValue(x - 1, y - 1));
        }

        [Test]
        public void Test_GetValueMultiple()
        {
            const int x = 6;
            const int y = 6;
            const int expandX = 6;
            const int expandY = 3;
            const int value = 666;
            _intGrid.SetValueMultiple(x, y, expandX, expandY, value);
            var intArray = _intGrid.GetValueMultiple(x, y, expandX, expandY);
            Assert.AreEqual(expandX * expandY, intArray.Length);
            Assert.AreEqual(value, _intGrid.GetValue(x, y));
            Assert.AreEqual(value, _intGrid.gridArray[x, y]);
            Assert.AreNotEqual(value, _intGrid.gridArray[x -1, y -1]);
            Assert.AreNotEqual(value, _intGrid.GetValue(x - 1, y - 1));
        }

        [Test]
        public void Test_GetGridPosition()
        {
            const int x = 6;
            const int y = 6;
            var point = Vector3.one * _cellSize / 2;
            _intGrid.GetGridPosition(point, out var outX, out var outY);
            Assert.AreEqual(0, outX);
            Assert.AreEqual(0, outY);
            var point2 = new Vector3(x, 0, y) * _cellSize + new Vector3(_cellSize / 2, 0, _cellSize / 2);
            _intGrid.GetGridPosition(point2, out var outX2, out var outY2);
            Assert.AreEqual(x, outX2);
            Assert.AreEqual(y, outY2);
        }

        [Test]
        public void Test_GetGridPositionWithOffset()
        {
            var offset = new Vector3(1.5f, 4.5f, 1.5f);
            var point = new Vector3(6.6f, 3.2f, 6.6f);
            var offsetGrid = new GridBase<int>(_width, _depth, _cellSize, offset);
            offsetGrid.GetGridPosition(point, out var actualX, out var actualY);
            _intGrid.GetGridPositionWithOffset(point, offset, out var x, out var y);
            Assert.AreEqual(actualX, x);
            Assert.AreEqual(actualY, y);
        }
    }
}

