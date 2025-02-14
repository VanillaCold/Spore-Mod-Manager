﻿using System;
using System.Windows;
using Graphics = System.Drawing.Graphics;
using Point = System.Windows.Point;

namespace SporeMods.CommonUI
{
    /// <summary>
    /// Provides methods and constants for system scaling and converting between DIPs and physical pixels.
    /// </summary>
    public static class SystemScaling
    {
        /// <summary>
        /// Converts DIPs to physical pixels.
        /// </summary>
        /// <param name="wpfUnits">The amount of DIPs to convert to physical pixels.</param>
        /// <returns>A floating point number representing the amount of DIPs represented by <paramref name="realPixels"/> pixels.</returns>
        public static int WpfUnitsToRealPixels(Double wpfUnits) => (int)(wpfUnits * ScalingFactor);

        /// <summary>
        /// Converts physical pixels to DIPs.
        /// </summary>
        /// <param name="realPixels">The amount of real pixels to convert to DIPs.</param>
        /// <returns>A floating point number representing the amount of DIPs represented by <paramref name="realPixels"/> pixels.</returns>
        public static Double RealPixelsToWpfUnits(Double realPixels) => (realPixels / ScalingFactor);

        /// <summary>
        /// Converts DIPs to physical pixels.
        /// </summary>
        /// <param name="wpfUnits">The amount of DIPs to convert to physical pixels.</param>
        /// <returns>A floating point number representing the amount of DIPs represented by <paramref name="realPixels"/> pixels.</returns>
        public static int WpfUnitsToRealPixels(Single wpfUnits) => (int)(wpfUnits * ScalingFactor);

        /// <summary>
        /// Converts physical pixels to DIPs.
        /// </summary>
        /// <param name="realPixels">The amount of real pixels to convert to DIPs.</param>
        /// <returns>A floating point number representing the amount of DIPs represented by <paramref name="realPixels"/> pixels.</returns>
        public static Single RealPixelsToWpfUnits(Single realPixels) => (Single)(realPixels / ScalingFactor);

        /// <summary>
        /// Converts DIPs to physical pixels.
        /// </summary>
        /// <param name="wpfUnits">The amount of DIPs to convert to physical pixels.</param>
        /// <returns>An integer representing the amount of DIPs represented by <paramref name="realPixels"/> pixels.</returns>
        public static Int32 WpfUnitsToRealPixels(Int32 wpfUnits) => (Int32)(wpfUnits * ScalingFactor);

        /// <summary>
        /// Converts physical pixels to DIPs.
        /// </summary>
        /// <param name="realPixels">The amount of real pixels to convert to DIPs.</param>
        /// <returns>An integer representing the amount of DIPs represented by <paramref name="realPixels"/> pixels.</returns>
        public static Int32 RealPixelsToWpfUnits(Int32 realPixels) => (Int32)(realPixels / ScalingFactor);

        /// <summary>
        /// Gets the current scaling factor applied to the computer.
        /// </summary>
        /// <value>
        /// A floating-point number representing the applies scaling factor.
        /// </value>
        public static Double ScalingFactor
        {
            get
            {
                var g = Graphics.FromHwnd(IntPtr.Zero);
                return g.DpiY / 96;
            }
        }

        /// <summary>
        /// Gets the cursor position in DIPs.
        /// </summary>
        /// <value>
        /// A point representing the cursor's position.
        /// </value>
        public static Point CursorPosition
        {
            get
            {
                var p = System.Windows.Forms.Cursor.Position;
                return new Point(RealPixelsToWpfUnits(p.X), RealPixelsToWpfUnits(p.Y));
            }
            set
            {
                System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)WpfUnitsToRealPixels(value.X), (int)WpfUnitsToRealPixels(value.Y));
            }
        }

        /// <summary>
        /// Determines whether the cursor is within the bounds of a FrameworkElement, independent of WPF mouse events.
        /// </summary>
        /// <value>
        /// A point representing whether the cursor is within the bounds of a FrameworkElement, independent of WPF mouse events.
        /// </value>
        public static bool IsMouseWithin(FrameworkElement target)
        {
            return IsPointWithin(target, CursorPosition);
        }

        /// <summary>
        /// Determines whether a given point is within the bounds of a FrameworkElement.
        /// </summary>
        /// <value>
        /// A point representing whether the given point is within the bounds of a FrameworkElement.
        /// </value>
        public static bool IsPointWithin(FrameworkElement target, Point point)
        {
            var targetPoint = target.PointToScreen(new Point(0, 0));

            double width = target.ActualWidth;
            if (width == 0)
                try
                {
                    width = target.Width;
                }
                catch
                {
                }

            double height = target.ActualHeight;
            if (height == 0)
                try
                {
                    height = target.Height;
                }
                catch
                {
                }

            return (point.X >= targetPoint.X)
                && (point.Y >= targetPoint.Y)
                && (point.X < targetPoint.X + width)
                && (point.Y < targetPoint.Y + height);
        }
    }
}
