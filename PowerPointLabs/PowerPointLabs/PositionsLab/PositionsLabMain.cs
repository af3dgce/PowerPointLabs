﻿using System;
using System.Collections.Generic;
using Microsoft.Office.Core;
using PowerPointLabs.Utils;
using Shape = Microsoft.Office.Interop.PowerPoint.Shape;
using AutoShape = Microsoft.Office.Core.MsoAutoShapeType;
using System.Diagnostics;
using Drawing = System.Drawing;

namespace PowerPointLabs.PositionsLab
{
    class PositionsLabMain
    {
        private const float Epsilon = 0.00001f;
        private const float RotateLeft = 90f;
        private const float RotateRight = 270f;
        private const float RotateUp = 0f;
        private const float RotateDown = 180f;
        private const int None = -1;
        private const int Right = 0;
        private const int Down = 1;
        private const int Left = 2;
        private const int Up = 3;
        private const int Leftorright = 4;
        private const int Upordown = 5;

        //Error Messages
        private const string ErrorMessageFewerThanTwoSelection = TextCollection.PositionsLabText.ErrorFewerThanTwoSelection;
        private const string ErrorMessageUndefined = TextCollection.PositionsLabText.ErrorUndefined;

        //For Grid
        public enum GridAlignment
        {
            AlignLeft,
            AlignCenter,
            AlignRight
        }

        public static GridAlignment DistributeGridAlignment { get; private set; }
        public static float MarginTop { get; private set; }
        public static float MarginBottom { get; private set; }
        public static float MarginLeft { get; private set; }
        public static float MarginRight { get; private set; }

        private static Dictionary<MsoAutoShapeType, float> shapeDefaultUpAngle;
        public static bool AlignUseSlideAsReference { get; private set; }
        public static bool DistributeUseSlideAsReference { get; private set; }

        #region API

        #region Class Methods

        /// <summary>
        /// Tells the Positions Lab to use the slide as the reference point for Align methods
        /// </summary>
        public static void AlignReferToSlide()
        {
            AlignUseSlideAsReference = true;
        }

        /// <summary>
        /// Tells the Positions Lab to use first selected shape as reference shape for Align methods
        /// </summary>
        public static void AlignReferToShape()
        {
            AlignUseSlideAsReference = false;
        }

        /// <summary>
        /// Tells the Position Lab to use the slide as the reference point for Distribute methods
        /// </summary>
        public static void DistributeReferToSlide()
        {
            DistributeUseSlideAsReference = true;
        }

        /// <summary>
        /// Tells the Positions Lab to use first selected shape as reference shape for Distribute methods
        /// </summary>
        public static void DistributeReferToShape()
        {
            DistributeUseSlideAsReference = false;
        }

        public static void SetDistributeGridAlignment(GridAlignment alignment)
        {
            DistributeGridAlignment = alignment;
        }

        public static void SetDistributeMarginTop (float marginTop)
        {
            MarginTop = marginTop;
        }

        public static void SetDistributeMarginBottom(float marginBottom)
        {
            MarginBottom = marginBottom;
        }

        public static void SetDistributeMarginLeft(float marginLeft)
        {
            MarginLeft = marginLeft;
        }

        public static void SetDistributeMarginRight(float marginRight)
        {
            MarginRight = marginRight;
        }

        #endregion

        #region Align
        public static void AlignLeft(List<Shape> selectedShapes)
        {
            if (AlignUseSlideAsReference)
            {
                foreach (var s in selectedShapes)
                {
                    var allPointsOfShape = Graphics.GetRealCoordinates(s);
                    var leftMost = Graphics.LeftMostPoint(allPointsOfShape);
                    s.IncrementLeft(-leftMost.X);
                }
            }
            else
            {
                if (selectedShapes.Count < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var refShape = selectedShapes[0];
                var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
                var leftMostRef = Graphics.LeftMostPoint(allPointsOfRef);

                for (var i = 1; i < selectedShapes.Count; i++)
                {
                    var s = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var leftMost = Graphics.LeftMostPoint(allPoints);
                    s.IncrementLeft(leftMostRef.X - leftMost.X);
                }
            }
        }

        public static void AlignRight(List<Shape> selectedShapes, float slideWidth)
        {
            if (AlignUseSlideAsReference)
            {
                foreach (var s in selectedShapes)
                {
                    var allPointsOfShape = Graphics.GetRealCoordinates(s);
                    var leftMost = Graphics.LeftMostPoint(allPointsOfShape);
                    var shapeWidth = Graphics.RealWidth(allPointsOfShape);
                    s.IncrementLeft(slideWidth - leftMost.X - shapeWidth);
                }
            }
            else
            {
                if (selectedShapes.Count < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var refShape = selectedShapes[0];
                var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
                var rightMostRef = Graphics.RightMostPoint(allPointsOfRef);

                for (var i = 1; i < selectedShapes.Count; i++)
                {
                    var s = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var rightMost = Graphics.RightMostPoint(allPoints);
                    s.IncrementLeft(rightMostRef.X - rightMost.X);
                }
            }
        }

        public static void AlignTop(List<Shape> selectedShapes)
        {
            if (AlignUseSlideAsReference)
            {
                foreach (var s in selectedShapes)
                {
                    var allPointsOfShape = Graphics.GetRealCoordinates(s);
                    var topMost = Graphics.TopMostPoint(allPointsOfShape);
                    s.IncrementTop(-topMost.Y);
                }
            }
            else
            {
                if (selectedShapes.Count < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var refShape = selectedShapes[0];
                var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
                var topMostRef = Graphics.TopMostPoint(allPointsOfRef);

                for (var i = 1; i < selectedShapes.Count; i++)
                {
                    var s = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var topMost = Graphics.TopMostPoint(allPoints);
                    s.IncrementTop(topMostRef.Y - topMost.Y);
                }
            }
        }

        public static void AlignBottom(List<Shape> selectedShapes, float slideHeight)
        {
            if (AlignUseSlideAsReference)
            {
                foreach (var s in selectedShapes)
                {
                    var allPointsOfShape = Graphics.GetRealCoordinates(s);
                    var topMost = Graphics.TopMostPoint(allPointsOfShape);
                    var shapeHeight = Graphics.RealHeight(allPointsOfShape);
                    s.IncrementTop(slideHeight - topMost.Y - shapeHeight);
                }
            }
            else
            {
                if (selectedShapes.Count < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var refShape = selectedShapes[0];
                var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
                var lowestRef = Graphics.BottomMostPoint(allPointsOfRef);

                for (var i = 1; i < selectedShapes.Count; i++)
                {
                    var s = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var lowest = Graphics.BottomMostPoint(allPoints);
                    s.IncrementTop(lowestRef.Y - lowest.Y);
                }
            }
        }

        public static void AlignMiddle(List<Shape> selectedShapes, float slideHeight)
        {
            if (AlignUseSlideAsReference)
            {
                foreach (var s in selectedShapes)
                {
                    var allPointsOfShape = Graphics.GetRealCoordinates(s);
                    var topMost = Graphics.TopMostPoint(allPointsOfShape);
                    var shapeHeight = Graphics.RealHeight(allPointsOfShape);
                    s.IncrementTop(slideHeight/2 - topMost.Y - shapeHeight/2);
                }
            }
            else
            {
                if (selectedShapes.Count < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var refShape = selectedShapes[0];
                var originRef = Graphics.GetCenterPoint(refShape);

                for (var i = 1; i < selectedShapes.Count; i++)
                {
                    var s = selectedShapes[i];
                    var origin = Graphics.GetCenterPoint(s);
                    s.IncrementTop(originRef.Y - origin.Y);
                }
            }
        }

        public static void AlignCenter(List<Shape> selectedShapes, float slideWidth, float slideHeight)
        {
            if (AlignUseSlideAsReference)
            {
                foreach (var s in selectedShapes)
                {
                    var allPointsOfShape = Graphics.GetRealCoordinates(s);
                    var topMost = Graphics.TopMostPoint(allPointsOfShape);
                    var leftMost = Graphics.LeftMostPoint(allPointsOfShape);
                    var shapeHeight = Graphics.RealHeight(allPointsOfShape);
                    var shapeWidth = Graphics.RealWidth(allPointsOfShape);
                    s.IncrementTop(slideHeight/2 - topMost.Y - shapeHeight/2);
                    s.IncrementLeft(slideWidth/2 - leftMost.X - shapeWidth/2);
                }
            }
            else
            {
                if (selectedShapes.Count < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var refShape = selectedShapes[0];
                var originRef = Graphics.GetCenterPoint(refShape);

                for (var i = 1; i < selectedShapes.Count; i++)
                {
                    var s = selectedShapes[i];
                    var origin = Graphics.GetCenterPoint(s);
                    s.IncrementLeft(originRef.X - origin.X);
                    s.IncrementTop(originRef.Y - origin.Y);
                }
            }
        }

        #endregion

        #region Snap
        public static void SnapVertical(List<Shape> selectedShapes)
        {
            foreach (var s in selectedShapes)
            {
                SnapShapeVertical(s);
            }
        }

        public static void SnapHorizontal(List<Shape> selectedShapes)
        {
            foreach (var s in selectedShapes)
            {
                SnapShapeHorizontal(s);
            }
        }

        public static void SnapAway(List<Shape> shapes)
        {
            if (shapes.Count < 2)
            {
                throw new Exception(ErrorMessageFewerThanTwoSelection);
            }

            var refShapeCenter = Graphics.GetCenterPoint(shapes[0]);
            var isAllSameDir = true;
            var lastDir = -1;

            for (var i = 1; i < shapes.Count; i++)
            {
                var shape = shapes[i];
                var shapeCenter = Graphics.GetCenterPoint(shape);
                var angle = (float)AngleBetweenTwoPoints(refShapeCenter, shapeCenter);

                var dir = GetDirectionWrtRefShape(shape, angle);

                if (i == 1)
                {
                    lastDir = dir;
                }

                if (!IsSameDirection(lastDir, dir))
                {
                    isAllSameDir = false;
                    break;
                }

                //only maintain in one direction instead of dual direction
                if (dir < Leftorright)
                {
                    lastDir = dir; 
                }
            }

            if (!isAllSameDir || lastDir == None)
            {
                lastDir = 0;
            }
            else
            {
                lastDir++;
            }

            for (var i = 1; i < shapes.Count; i++)
            {
                var shape = shapes[i];
                var shapeCenter = Graphics.GetCenterPoint(shape);
                var angle = (float) AngleBetweenTwoPoints(refShapeCenter, shapeCenter);

                float defaultUpAngle = 0;
                var hasDefaultDirection = shapeDefaultUpAngle.TryGetValue(shape.AutoShapeType, out defaultUpAngle);

                if (hasDefaultDirection)
                {
                    shape.Rotation = (defaultUpAngle + angle) + lastDir * 90;
                }
                else
                {
                    if (IsVertical(shape))
                    {
                        shape.Rotation = angle + lastDir * 90;
                    }
                    else
                    {
                        shape.Rotation = (angle - 90) + lastDir * 90;
                    }
                }
            }
        }

        public static void SnapShapeVertical(Shape shape)
        {
            if (IsVertical(shape))
            {
                SnapTo0Or180(shape);
            }
            else
            {
                SnapTo90Or270(shape);
            }
        }

        public static void SnapShapeHorizontal(Shape shape)
        {
            if (IsVertical(shape))
            {
                SnapTo90Or270(shape);
            }
            else
            {
                SnapTo0Or180(shape);
            }
        }

        private static void SnapTo0Or180 (Shape shape)
        {
            var rotation = shape.Rotation;

            if (rotation >= 90 && rotation < 270)
            {
                shape.Rotation = 180;
            }
            else
            {
                shape.Rotation = 0;
            }
        }

        private static void SnapTo90Or270(Shape shape)
        {
            var rotation = shape.Rotation;

            if (rotation >= 0 && rotation < 180)
            {
                shape.Rotation = 90;
            }
            else
            {
                shape.Rotation = 270;
            }
        }

        private static bool IsVertical(Shape shape)
        {
            return shape.Height > shape.Width;
        }
        #endregion

        #region Adjoin
        public static void AdjoinHorizontal(List<Shape> selectedShapes)
        {
            if (selectedShapes.Count < 2)
            {
                throw new Exception(ErrorMessageFewerThanTwoSelection);
            }

            var refShape = selectedShapes[0];
            var sortedShapes = Graphics.SortShapesByLeft(selectedShapes);
            var refShapeIndex = sortedShapes.IndexOf(refShape);

            var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
            var centerOfRef = Graphics.GetCenterPoint(refShape);

            var mostLeft = Graphics.LeftMostPoint(allPointsOfRef).X;
            //For all shapes left of refShape, adjoin them from closest to refShape
            for (var i = refShapeIndex - 1; i >= 0; i--)
            {
                var neighbour = sortedShapes[i];
                var allPointsOfNeighbour = Graphics.GetRealCoordinates(neighbour);
                var rightOfShape = Graphics.RightMostPoint(allPointsOfNeighbour).X;
                neighbour.IncrementLeft(mostLeft - rightOfShape);
                neighbour.IncrementTop(centerOfRef.Y - Graphics.GetCenterPoint(neighbour).Y);

                mostLeft = Graphics.LeftMostPoint(allPointsOfNeighbour).X + mostLeft - rightOfShape;
            }

            var mostRight = Graphics.RightMostPoint(allPointsOfRef).X;
            //For all shapes right of refShape, adjoin them from closest to refShape
            for (var i = refShapeIndex + 1; i < sortedShapes.Count; i++)
            {
                var neighbour = sortedShapes[i];
                var allPointsOfNeighbour = Graphics.GetRealCoordinates(neighbour);
                var leftOfShape = Graphics.LeftMostPoint(allPointsOfNeighbour).X;
                neighbour.IncrementLeft(mostRight - leftOfShape);
                neighbour.IncrementTop(centerOfRef.Y - Graphics.GetCenterPoint(neighbour).Y);

                mostRight = Graphics.RightMostPoint(allPointsOfNeighbour).X + mostRight - leftOfShape;
            }
        }

        public static void AdjoinVertical(List<Shape> selectedShapes)
        {
            if (selectedShapes.Count < 2)
            {
                throw new Exception(ErrorMessageFewerThanTwoSelection);
            }

            var refShape = selectedShapes[0];
            var sortedShapes = Graphics.SortShapesByTop(selectedShapes);
            var refShapeIndex = sortedShapes.IndexOf(refShape);

            var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
            var centerOfRef = Graphics.GetCenterPoint(refShape);

            var mostTop = Graphics.TopMostPoint(allPointsOfRef).Y;
            //For all shapes above refShape, adjoin them from closest to refShape
            for (var i = refShapeIndex - 1; i >= 0; i--)
            {
                var neighbour = sortedShapes[i];
                var allPointsOfNeighbour = Graphics.GetRealCoordinates(neighbour);
                var bottomOfShape = Graphics.BottomMostPoint(allPointsOfNeighbour).Y;
                neighbour.IncrementLeft(centerOfRef.X - Graphics.GetCenterPoint(neighbour).X);
                neighbour.IncrementTop(mostTop - bottomOfShape);

                mostTop = Graphics.TopMostPoint(allPointsOfNeighbour).Y + mostTop - bottomOfShape;
            }

            var lowest = Graphics.BottomMostPoint(allPointsOfRef).Y;
            //For all shapes right of refShape, adjoin them from closest to refShape
            for (var i = refShapeIndex + 1; i < sortedShapes.Count; i++)
            {
                var neighbour = sortedShapes[i];
                var allPointsOfNeighbour = Graphics.GetRealCoordinates(neighbour);
                var topOfShape = Graphics.TopMostPoint(allPointsOfNeighbour).Y;
                neighbour.IncrementLeft(centerOfRef.X - Graphics.GetCenterPoint(neighbour).X);
                neighbour.IncrementTop(lowest - topOfShape);

                lowest = Graphics.BottomMostPoint(allPointsOfNeighbour).Y + lowest - topOfShape;
            }
        }
        #endregion

        #region Swap
        public static void Swap(List<Shape> selectedShapes)
        {
            if (selectedShapes.Count < 2)
            {
                throw new Exception(ErrorMessageFewerThanTwoSelection);
            }

            var sortedShapes = Graphics.SortShapesByLeft(selectedShapes);
            var firstPos = Graphics.GetCenterPoint(sortedShapes[0]);

            for (var i = 0; i < sortedShapes.Count; i++)
            {
                var currentShape = sortedShapes[i];
                if (i < sortedShapes.Count - 1)
                {
                    var currentPos = Graphics.GetCenterPoint(currentShape);
                    var nextPos = Graphics.GetCenterPoint(sortedShapes[i + 1]);

                    currentShape.IncrementLeft(nextPos.X - currentPos.X);
                    currentShape.IncrementTop(nextPos.Y - currentPos.Y);
                }
                else
                {
                    var currentPos = Graphics.GetCenterPoint(currentShape);
                    currentShape.IncrementLeft(firstPos.X - currentPos.X);
                    currentShape.IncrementTop(firstPos.Y - currentPos.Y);
                }
            }
        }
        #endregion

        #region Distribute
        public static void DistributeHorizontal(List<Shape> selectedShapes, float slideWidth)
        {
            var shapeCount = selectedShapes.Count;

            var refShape = selectedShapes[0];
            var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
            Drawing.PointF rightMostRef;

            if (DistributeUseSlideAsReference)
            {
                var horizontalDistanceInRef = slideWidth;
                var spaceBetweenShapes = horizontalDistanceInRef;

                foreach (var s in selectedShapes)
                {
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var shapeWidth = Graphics.RealWidth(allPoints);
                    spaceBetweenShapes -= shapeWidth;
                }

                // TODO: guard against spaceBetweenShapes < 0

                spaceBetweenShapes /= shapeCount + 1;

                for (var i = 0; i < shapeCount; i++)
                {
                    var currShape = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(currShape);
                    var leftMost = Graphics.LeftMostPoint(allPoints);
                    if (i == 0)
                    {
                        currShape.IncrementLeft(spaceBetweenShapes - leftMost.X);
                    }
                    else
                    {
                        refShape = selectedShapes[i - 1];
                        allPointsOfRef = Graphics.GetRealCoordinates(refShape);
                        rightMostRef = Graphics.RightMostPoint(allPointsOfRef);
                        currShape.IncrementLeft(rightMostRef.X - leftMost.X + spaceBetweenShapes);
                    }
                }
            }
            else
            {
                if (shapeCount < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var horizontalDistanceInRef = Graphics.RealWidth(allPointsOfRef);
                var spaceBetweenShapes = horizontalDistanceInRef;

                for (var i = 1; i < shapeCount; i++)
                {
                    var s = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var shapeWidth = Graphics.RealWidth(allPoints);
                    spaceBetweenShapes -= shapeWidth;
                }

                // TODO: guard against spaceBetweenShapes < 0

                spaceBetweenShapes /= shapeCount;

                for (var i = 1; i < shapeCount; i++)
                {
                    var currShape = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(currShape);
                    var leftMost = Graphics.LeftMostPoint(allPoints);
                    refShape = selectedShapes[i - 1];
                    allPointsOfRef = Graphics.GetRealCoordinates(refShape);

                    if (i == 1)
                    {
                        var leftMostRef = Graphics.LeftMostPoint(allPointsOfRef);
                        currShape.IncrementLeft(leftMostRef.X - leftMost.X + spaceBetweenShapes);
                    }
                    else
                    {
                        rightMostRef = Graphics.RightMostPoint(allPointsOfRef);
                        currShape.IncrementLeft(rightMostRef.X - leftMost.X + spaceBetweenShapes);
                    }
                }
            }
        } 

        public static void DistributeVertical(List<Shape> selectedShapes, float slideHeight)
        {
            var shapeCount = selectedShapes.Count;

            var refShape = selectedShapes[0];
            var allPointsOfRef = Graphics.GetRealCoordinates(refShape);
            Drawing.PointF lowestRef;

            if (DistributeUseSlideAsReference)
            {
                var verticalDistanceInRef = slideHeight;
                var spaceBetweenShapes = verticalDistanceInRef;

                foreach (var s in selectedShapes)
                {
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var shapeHeight = Graphics.RealHeight(allPoints);
                    spaceBetweenShapes -= shapeHeight;
                }

                // TODO: guard against spaceBetweenShapes < 0

                spaceBetweenShapes /= shapeCount + 1;

                for (var i = 0; i < shapeCount; i++)
                {
                    var currShape = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(currShape);
                    var topMost = Graphics.TopMostPoint(allPoints);
                    if (i == 0)
                    {
                        currShape.IncrementTop(spaceBetweenShapes - topMost.Y);
                    }
                    else
                    {
                        refShape = selectedShapes[i - 1];
                        allPointsOfRef = Graphics.GetRealCoordinates(refShape);
                        lowestRef = Graphics.BottomMostPoint(allPointsOfRef);
                        currShape.IncrementTop(lowestRef.Y - topMost.Y + spaceBetweenShapes);
                    }
                }
            }
            else
            {
                if (shapeCount < 2)
                {
                    throw new Exception(ErrorMessageFewerThanTwoSelection);
                }

                var verticalDistanceInRef = Graphics.RealHeight(allPointsOfRef);
                var spaceBetweenShapes = verticalDistanceInRef;

                for (var i = 1; i < shapeCount; i++)
                {
                    var s = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(s);
                    var shapeHeight = Graphics.RealHeight(allPoints);
                    spaceBetweenShapes -= shapeHeight;
                }

                // TODO: guard against spaceBetweenShapes < 0

                spaceBetweenShapes /= shapeCount;

                for (var i = 1; i < shapeCount; i++)
                {
                    var currShape = selectedShapes[i];
                    var allPoints = Graphics.GetRealCoordinates(currShape);
                    var topMost = Graphics.TopMostPoint(allPoints);
                    refShape = selectedShapes[i - 1];
                    allPointsOfRef = Graphics.GetRealCoordinates(refShape);

                    if (i == 1)
                    {
                        var topMostRef = Graphics.TopMostPoint(allPointsOfRef);
                        currShape.IncrementTop(topMostRef.Y - topMost.Y + spaceBetweenShapes);
                    }
                    else
                    {
                        lowestRef = Graphics.BottomMostPoint(allPointsOfRef);
                        currShape.IncrementTop(lowestRef.Y - topMost.Y + spaceBetweenShapes);
                    }
                }
            }
        }

        public static void DistributeCenter(List<Shape> selectedShapes, float slideWidth, float slideHeight)
        {
            DistributeHorizontal(selectedShapes, slideWidth);
            DistributeVertical(selectedShapes, slideHeight);
        }

        public static void DistributeShapes(List<Shape> selectedShapes)
        {
            var shapeCount = selectedShapes.Count;

            if (shapeCount < 2)
            {
                throw new Exception(ErrorMessageFewerThanTwoSelection);
            }

            if (shapeCount == 2)
            {
                return;
            }

            var firstRef = selectedShapes[0];
            var lastRef = selectedShapes[selectedShapes.Count - 1];
            Shape refShape;

            var allPointsOfFirstRef = Graphics.GetRealCoordinates(firstRef);
            var allPointsOfLastRef = Graphics.GetRealCoordinates(lastRef);
            Drawing.PointF[] allPointsOfRef;

            var horizontalDistance = Graphics.LeftMostPoint(allPointsOfLastRef).X - Graphics.RightMostPoint(allPointsOfFirstRef).X;
            var verticalDistance = Graphics.TopMostPoint(allPointsOfLastRef).Y - Graphics.BottomMostPoint(allPointsOfFirstRef).Y;

            var spaceBetweenShapes = horizontalDistance;

            for (var i = 1; i < shapeCount - 1; i++)
            {
                var s = selectedShapes[i];
                var allPoints = Graphics.GetRealCoordinates(s);
                var shapeWidth = Graphics.RealWidth(allPoints);
                spaceBetweenShapes -= shapeWidth;
            }

            // TODO: guard against spaceBetweenShapes < 0

            spaceBetweenShapes /= (shapeCount-1);
            
            for (var i = 1; i < shapeCount - 1; i++)
            {
                var currShape = selectedShapes[i];
                var allPoints = Graphics.GetRealCoordinates(currShape);
                var leftMost = Graphics.LeftMostPoint(allPoints);
                refShape = selectedShapes[i - 1];
                allPointsOfRef = Graphics.GetRealCoordinates(refShape);

                var rightMostRef = Graphics.RightMostPoint(allPointsOfRef);
                currShape.IncrementLeft(rightMostRef.X - leftMost.X + spaceBetweenShapes);
            }

            spaceBetweenShapes = verticalDistance;
            for (var i = 1; i < shapeCount - 1; i++)
            {
                var s = selectedShapes[i];
                var allPoints = Graphics.GetRealCoordinates(s);
                var shapeHeight = Graphics.RealHeight(allPoints);
                spaceBetweenShapes -= shapeHeight;
            }

            // TODO: guard against spaceBetweenShapes < 0

            spaceBetweenShapes /= shapeCount;

            for (var i = 1; i < shapeCount - 1; i++)
            {
                var currShape = selectedShapes[i];
                var allPoints = Graphics.GetRealCoordinates(currShape);
                var topMost = Graphics.TopMostPoint(allPoints);
                refShape = selectedShapes[i - 1];
                allPointsOfRef = Graphics.GetRealCoordinates(refShape);

                var lowestRef = Graphics.BottomMostPoint(allPointsOfRef);
                currShape.IncrementTop(lowestRef.Y - topMost.Y + spaceBetweenShapes);
            }
        }

        public static void DistributeGrid(List<Shape> selectedShapes, int rowLength, int colLength)
        {
            var colLengthGivenFullRows = (int)Math.Ceiling((double)selectedShapes.Count / rowLength);
            if (colLength <= colLengthGivenFullRows)
            {
                DistributeGridByRow(selectedShapes, rowLength, colLength);
            }
            else
            {
                DistributeGridByCol(selectedShapes, rowLength, colLength);
            }
        }

        public static void DistributeGridByRow(List<Shape> selectedShapes, int rowLength, int colLength)
        {
            var refPoint = Graphics.GetCenterPoint(selectedShapes[0]);

            var allShapes = new List<PPShape>();
            var numShapes = selectedShapes.Count;

            for (var i = 0; i < numShapes; i++)
            {
                allShapes.Add(new PPShape(selectedShapes[i]));
            }

            var numIndicesToSkip = IndicesToSkip(numShapes, rowLength, DistributeGridAlignment);

            var rowDifferences = GetLongestWidthsOfRowsByRow(allShapes, rowLength, numIndicesToSkip);
            var colDifferences = GetLongestHeightsOfColsByRow(allShapes, rowLength, colLength);

            var posX = refPoint.X;
            var posY = refPoint.Y;
            var remainder = numShapes % rowLength;
            var differenceIndex = 0;

            for (var i = 0; i < numShapes; i++)
            {
                //Start of new row
                if (i % rowLength == 0 && i != 0)
                {
                    posX = refPoint.X;
                    differenceIndex = 0;
                    posY += GetSpaceBetweenShapes(i / rowLength - 1, i / rowLength, colDifferences, MarginTop, MarginBottom);
                }

                //If last row, offset by num of indices to skip
                if (numShapes - i == remainder)
                {
                    differenceIndex = numIndicesToSkip;
                    posX += GetSpaceBetweenShapes(0, differenceIndex, rowDifferences, MarginLeft, MarginRight);
                }

                var currentShape = selectedShapes[i];
                var center = Graphics.GetCenterPoint(currentShape);
                currentShape.IncrementLeft(posX - center.X);
                currentShape.IncrementTop(posY - center.Y);

                posX += GetSpaceBetweenShapes(differenceIndex, differenceIndex + 1, rowDifferences, MarginLeft, MarginRight);
                differenceIndex++;
            }
        }

        public static void DistributeGridByCol(List<Shape> selectedShapes, int rowLength, int colLength)
        {
            var refPoint = Graphics.GetCenterPoint(selectedShapes[0]);

            var allShapes = new List<PPShape>();
            var numShapes = selectedShapes.Count;

            for (var i = 0; i < numShapes; i++)
            {
                allShapes.Add(new PPShape(selectedShapes[i]));
            }

            var numIndicesToSkip = IndicesToSkip(numShapes, colLength, DistributeGridAlignment);

            var rowDifferences = GetLongestWidthsOfRowsByCol(allShapes, rowLength, colLength, numIndicesToSkip);
            var colDifferences = GetLongestHeightsOfColsByCol(allShapes, rowLength, colLength, numIndicesToSkip);

            var posX = refPoint.X;
            var posY = refPoint.Y;
            var remainder = colLength - (rowLength * colLength - numShapes);
            var augmentedShapeIndex = 0;

            for (var i = 0; i < numShapes; i++)
            {
                //If last index and need to skip, skip index 
                if (numIndicesToSkip > 0 && IsLastIndexOfRow(augmentedShapeIndex, rowLength))
                {
                    numIndicesToSkip--;
                    augmentedShapeIndex++;
                }

                //If last index and no more remainder, skip the rest
                if (IsLastIndexOfRow(augmentedShapeIndex, rowLength))
                {
                    if (remainder <= 0)
                    {
                        augmentedShapeIndex++;
                    }
                    else
                    {
                        remainder--;
                    }
                }

                if (IsFirstIndexOfRow(augmentedShapeIndex, rowLength) && augmentedShapeIndex != 0)
                {
                    posX = refPoint.X;
                    posY += GetSpaceBetweenShapes(augmentedShapeIndex / rowLength - 1, augmentedShapeIndex / rowLength, colDifferences, MarginTop, MarginBottom);
                }

                var currentShape = selectedShapes[i];
                var center = Graphics.GetCenterPoint(currentShape);
                currentShape.IncrementLeft(posX - center.X);
                currentShape.IncrementTop(posY - center.Y);

                posX += GetSpaceBetweenShapes(augmentedShapeIndex % rowLength, augmentedShapeIndex % rowLength + 1, rowDifferences, MarginLeft, MarginRight);
                augmentedShapeIndex++;
            }
        }
        #endregion

        #endregion

        #region Util
        public static double AngleBetweenTwoPoints(Drawing.PointF refPoint, Drawing.PointF pt)
        {
            var angle = Math.Atan((pt.Y - refPoint.Y) / (pt.X - refPoint.X)) * 180 / Math.PI;

            if (pt.X - refPoint.X > 0)
            {
                angle = 90 + angle;
            }
            else
            {
                angle = 270 + angle;
            }

            return angle;
        }

        public static bool NearlyEqual(float a, float b, float epsilon)
        {
            var absA = Math.Abs(a);
            var absB = Math.Abs(b);
            var diff = Math.Abs(a - b);

            if (a == b)
            { // shortcut, handles infinities
                return true;
            }
            if (a == 0 || b == 0 || diff < float.Epsilon)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < epsilon;
            }
            // use relative error
            return diff / (absA + absB) < epsilon;
        }

        private static int GetDirectionWrtRefShape(Shape shape, float angleFromRefShape)
        {
            float defaultUpAngle;
            var hasDefaultDirection = shapeDefaultUpAngle.TryGetValue(shape.AutoShapeType, out defaultUpAngle);

            if (shape.AutoShapeType == AutoShape.msoShapeLightningBolt)
            {
                Debug.WriteLine("defaultDir: " + hasDefaultDirection);
                Debug.WriteLine("defaultAngle: " + defaultUpAngle);
            }

            if (!hasDefaultDirection)
            {
                if (IsVertical(shape))
                {
                    defaultUpAngle = 0;
                }
                else
                {
                    defaultUpAngle = 90;
                }
            }

            var angle = AddAngles(angleFromRefShape, defaultUpAngle);
            var diff = SubtractAngles(shape.Rotation, angle);
            var phaseInFloat = diff / 90;

            if (shape.AutoShapeType == AutoShape.msoShapeLightningBolt)
            {
                Debug.WriteLine("angle: " + angle);
                Debug.WriteLine("diff: " + diff);
                Debug.WriteLine("phaseInFloat: " + defaultUpAngle);
                Debug.WriteLine("equal: " + NearlyEqual(phaseInFloat, (float)Math.Round(phaseInFloat), Epsilon));
            }

            if (!NearlyEqual(phaseInFloat, (float)Math.Round(phaseInFloat), Epsilon))
            {
                return None;
            }

            var phase = (int)Math.Round(phaseInFloat);

            if (!hasDefaultDirection)
            {
                if (phase == Left || phase == Right)
                {
                    return Leftorright;
                }

                return Upordown;
            }

            return phase;
        }

        private static bool IsSameDirection(int a, int b)
        {
            if (a == b) return true;
            if (a == Leftorright) return b == Left || b == Right;
            if (b == Leftorright) return a == Left || a == Right;
            if (a == Upordown) return b == Up || b == Down;
            if (b == Upordown) return a == Up || a == Down;

            return false;
       }

        public static float AddAngles(float a, float b)
        {
            return (a + b) % 360;
        }

        public static float SubtractAngles(float a, float b)
        {
            var diff = a - b;
            if (diff < 0)
            {
                return 360 + diff;
            }

            return diff;
        }

        public static float[] GetLongestWidthsOfRowsByRow(List<PPShape> shapes, int rowLength, int numIndicesToSkip)
        {
            var longestWidths = new float[rowLength];
            var numShapes = shapes.Count;
            var remainder = numShapes % rowLength;

            for (var i = 0; i < numShapes; i++)
            {
                var longestRowIndex = i % rowLength;
                if (numShapes - i == remainder - 1)
                {
                    longestRowIndex += numIndicesToSkip;
                }
                if (longestWidths[longestRowIndex] < shapes[i].AbsoluteWidth)
                {
                    longestWidths[longestRowIndex] = shapes[i].AbsoluteWidth;
                }
            }

            return longestWidths;
        }

        public static float[] GetLongestHeightsOfColsByRow(List<PPShape> shapes, int rowLength, int colLength)
        {
            var longestHeights = new float[colLength];

            for (var i = 0; i < shapes.Count; i++)
            {
                var longestHeightIndex = i / rowLength;
                if (longestHeights[longestHeightIndex] < shapes[i].AbsoluteHeight)
                {
                    longestHeights[longestHeightIndex] = shapes[i].AbsoluteHeight;
                }
            }

            return longestHeights;
        }

        public static float[] GetLongestWidthsOfRowsByCol(List<PPShape> shapes, int rowLength, int colLength, int numIndicesToSkip)
        {
            var longestWidths = new float[rowLength];
            var numShapes = shapes.Count;
            var augmentedShapeIndex = 0;
            var remainder = colLength - (rowLength * colLength - numShapes);

            for (var i = 0; i < numShapes; i++)
            {
                //If last index and need to skip, skip index 
                if (numIndicesToSkip > 0 && IsLastIndexOfRow(augmentedShapeIndex, rowLength))
                {
                    numIndicesToSkip--;
                    augmentedShapeIndex++;
                }

                //If last index and no more remainder, skip the rest
                if (IsLastIndexOfRow(augmentedShapeIndex, rowLength))
                {
                    if (remainder <= 0)
                    {
                        augmentedShapeIndex++;
                    }
                    else
                    {
                        remainder--;
                    }
                }

                var longestWidthsArrayIndex = augmentedShapeIndex % rowLength;

                if (longestWidths[longestWidthsArrayIndex] < shapes[i].AbsoluteWidth)
                {
                    longestWidths[longestWidthsArrayIndex] = shapes[i].AbsoluteWidth;
                }

                augmentedShapeIndex++;
            }

            return longestWidths;
        }

        public static float[] GetLongestHeightsOfColsByCol(List<PPShape> shapes, int rowLength, int colLength, int numIndicesToSkip)
        {
            var longestHeights = new float[colLength];
            var numShapes = shapes.Count;
            var augmentedShapeIndex = 0;
            var remainder = colLength - (rowLength * colLength - numShapes);

            for (var i = 0; i < numShapes; i++)
            {              
                //If last index and need to skip, skip index 
                if (numIndicesToSkip > 0 && IsLastIndexOfRow(augmentedShapeIndex, rowLength))
                {
                    numIndicesToSkip--;
                    augmentedShapeIndex++;
                }

                //If last index and no more remainder, skip the rest
                if (IsLastIndexOfRow(augmentedShapeIndex, rowLength))
                {
                    if (remainder <= 0)
                    {
                        augmentedShapeIndex++;
                    }
                    else
                    {
                        remainder--;
                    }
                }

                var longestHeightArrayIndex = augmentedShapeIndex / rowLength;

                if (longestHeights[longestHeightArrayIndex] < shapes[i].AbsoluteHeight)
                {
                    longestHeights[longestHeightArrayIndex] = shapes[i].AbsoluteHeight;
                }

                augmentedShapeIndex++;
            }

            return longestHeights;
        }

        private static bool IsFirstIndexOfRow(int index, int rowLength)
        {
            return index % rowLength == 0;
        }

        private static bool IsLastIndexOfRow(int index, int rowLength)
        {
            return index % rowLength == rowLength - 1;
        }

        public static int IndicesToSkip(int totalSelectedShapes, int rowLength, GridAlignment alignment)
        {
            var numOfShapesInLastRow = totalSelectedShapes % rowLength;

            if (alignment == GridAlignment.AlignLeft || numOfShapesInLastRow == 0)
            {
                return 0;
            }

            if (alignment == GridAlignment.AlignRight)
            {
                return rowLength - numOfShapesInLastRow;
            }

            if (alignment == GridAlignment.AlignCenter)
            {
                var difference = rowLength - numOfShapesInLastRow;
                return difference / 2;
            }

            return 0;
        }

        private static float GetSpaceBetweenShapes(int index1, int index2, float[] differences, float margin1, float margin2)
        {
            if (index1 >= differences.Length || index2 >= differences.Length)
            {
                return -1;
            }

            var start = 0;
            var end = 0;

            if (index1 < index2)
            {
                start = index1;
                end = index2;
            }
            else
            {
                start = index2;
                end = index1;
            }

            float difference = 0;

            for (var i = index1; i < index2; i++)
            {
                difference += (differences[i] / 2 + margin1 + margin2 + differences[i + 1] / 2);
            }

            return difference;
        }

        private static void InitDefaultShapesAngles()
        {
            shapeDefaultUpAngle = new Dictionary<MsoAutoShapeType, float>();

            shapeDefaultUpAngle.Add(AutoShape.msoShapeLeftArrow, RotateLeft);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeLeftRightArrow, RotateLeft);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeLeftArrowCallout, RotateLeft);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeLeftRightArrowCallout, RotateLeft);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeCurvedLeftArrow, RotateLeft);

            shapeDefaultUpAngle.Add(AutoShape.msoShapeRightArrow, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeBentArrow, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeStripedRightArrow, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeNotchedRightArrow, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapePentagon, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeChevron, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeRightArrowCallout, RotateRight);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeCurvedRightArrow, RotateRight);

            shapeDefaultUpAngle.Add(AutoShape.msoShapeUpArrow, RotateUp);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeBentUpArrow, RotateUp);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeUpDownArrow, RotateUp);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeLeftRightUpArrow, RotateUp);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeLeftUpArrow, RotateUp);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeUpArrowCallout, RotateUp);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeCurvedUpArrow, RotateUp);

            shapeDefaultUpAngle.Add(AutoShape.msoShapeDownArrow, RotateDown);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeUTurnArrow, RotateDown);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeDownArrowCallout, RotateDown);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeCurvedDownArrow, RotateDown);
            shapeDefaultUpAngle.Add(AutoShape.msoShapeCircularArrow, RotateDown);
        }

        private static void InitDefaultDistributeSettings()
        {
            MarginTop = 5;
            MarginBottom = 5;
            MarginLeft = 5;
            MarginRight = 5;
            DistributeGridAlignment = GridAlignment.AlignLeft;
            DistributeUseSlideAsReference = false;
        }

        private static void InitAlignSettings()
        {
            AlignUseSlideAsReference = false;
        }

        public static void InitPositionsLab()
        {
            InitDefaultShapesAngles();
            InitDefaultDistributeSettings();
            InitAlignSettings();
        }

        #endregion
    }
}