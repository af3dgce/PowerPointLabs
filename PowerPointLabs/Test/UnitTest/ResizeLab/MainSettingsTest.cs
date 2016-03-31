﻿using System.Collections.Generic;
using Microsoft.Office.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PowerPointLabs.ResizeLab;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;

namespace Test.UnitTest.ResizeLab
{
    [TestClass]
    public class MainSettingsTest : BaseResizeLabTest
    {
        private readonly ResizeLabMain _resizeLab = new ResizeLabMain();
        private List<string> _shapeNames;

        private const string WithAspectRatioShapeNames = "withAspectRatio";
        private const string WithoutAspectRatioShapeNames = "withoutAspectRatio";
        private const string ImageName = "image";

        private const int OriginalShapesSlideNo = 52;
        private const int AnchorTopLeftSlideNo = 53;
        private const int AnchorTopCenterSlideNo = 54;
        private const int AnchorTopRightSlideNo = 55;
        private const int AnchorMiddleLeftSlideNo = 56;
        private const int AnchorCenterSlideNo = 57;
        private const int AnchorMiddleRightSlideNo = 58;
        private const int AnchorBottomLeftSlideNo = 59;
        private const int AnchorBottomCenterSlideNo = 60;
        private const int AnchorBottomRightSlideNo = 61;

        [TestInitialize]
        public void TestInitialize()
        {
            _shapeNames = new List<string> {WithoutAspectRatioShapeNames, WithAspectRatioShapeNames, ImageName};
            InitOriginalShapes(OriginalShapesSlideNo, _shapeNames);
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.TopLeft;
            RestoreShapes(OriginalShapesSlideNo, _shapeNames);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestLockAspectRatio()
        {
            var shapes = GetShapes(OriginalShapesSlideNo, _shapeNames);

            _resizeLab.ChangeShapesAspectRatio(shapes, true);

            foreach (PowerPoint.Shape shape in shapes)
            {
                Assert.AreEqual(shape.LockAspectRatio, MsoTriState.msoTrue);
            }
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestUnlockAspectRatio()
        {
            var shapes = GetShapes(OriginalShapesSlideNo, _shapeNames);

            _resizeLab.ChangeShapesAspectRatio(shapes, false);

            foreach (PowerPoint.Shape shape in shapes)
            {
                Assert.AreEqual(shape.LockAspectRatio, MsoTriState.msoFalse);
            }
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorTopLeft()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorTopLeftSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.TopLeft;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorTopCenter()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorTopCenterSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.TopCenter;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorTopRight()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorTopRightSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.TopRight;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorMiddleLeft()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorMiddleLeftSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.MiddleLeft;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorCenter()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorCenterSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.Center;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorMiddleRight()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorMiddleRightSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.MiddleRight;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorBottomLeft()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorBottomLeftSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.BottomLeft;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorBottomCenter()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorBottomCenterSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.BottomCenter;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }

        [TestMethod]
        [TestCategory("UT")]
        public void TestEqualizeAnchorBottomRight()
        {
            var actualShapes = GetShapes(OriginalShapesSlideNo, _shapeNames);
            var expectedShapes = GetShapes(AnchorBottomRightSlideNo, _shapeNames);

            actualShapes.LockAspectRatio = MsoTriState.msoFalse;
            _resizeLab.AnchorPointType = ResizeLabMain.AnchorPoint.BottomRight;
            _resizeLab.ResizeToSameHeightAndWidth(actualShapes);
            CheckShapes(expectedShapes, actualShapes);
        }
    }
}
