﻿using System;

namespace MyFirstGameEngine.Models
{
    /// <summary>
    /// Represents a two-dimensional vector with float-based X and Y components.
    /// Provides utility methods for movement, normalization, distance calculation, angle detection, and vector arithmetic.
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// The horizontal (X) component of the vector.
        /// </summary>
        public float X { get; set; }

        /// <summary>
        /// The vertical (Y) component of the vector.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2"/> class with specified X and Y values.
        /// </summary>
        /// <param name="x">The horizontal component.</param>
        /// <param name="y">The vertical component.</param>
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Returns a new <see cref="Vector2"/> instance at position (0, 0).
        /// </summary>
        public static Vector2 Zero() => new Vector2(0, 0);

        /// <summary>
        /// Moves the vector upward by decreasing its Y value.
        /// </summary>
        /// <param name="value">The amount to move.</param>
        /// <returns>The updated vector instance.</returns>
        public Vector2 Up(float value)
        {
            Y -= value;
            return this;
        }

        /// <summary>
        /// Moves the vector downward by increasing its Y value.
        /// </summary>
        /// <param name="value">The amount to move.</param>
        /// <returns>The updated vector instance.</returns>
        public Vector2 Down(float value)
        {
            Y += value;
            return this;
        }

        /// <summary>
        /// Moves the vector left by decreasing its X value.
        /// </summary>
        /// <param name="value">The amount to move.</param>
        /// <returns>The updated vector instance.</returns>
        public Vector2 Left(float value)
        {
            X -= value;
            return this;
        }

        /// <summary>
        /// Moves the vector right by increasing its X value.
        /// </summary>
        /// <param name="value">The amount to move.</param>
        /// <returns>The updated vector instance.</returns>
        public Vector2 Right(float value)
        {
            X += value;
            return this;
        }

        /// <summary>
        /// Adds two vectors component-wise.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>A new vector that is the sum of the input vectors.</returns>
        public static Vector2 operator +(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X + vec2.X, vec1.Y + vec2.Y);
        }

        /// <summary>
        /// Subtracts the second vector from the first vector component-wise.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>A new vector that is the difference of the input vectors.</returns>
        public static Vector2 operator -(Vector2 vec1, Vector2 vec2)
        {
            return new Vector2(vec1.X - vec2.X, vec1.Y - vec2.Y);
        }

        /// <summary>
        /// Calculates the Euclidean distance between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        public float DistanceTo(Vector2 other)
        {
            float dx = this.X - other.X;
            float dy = this.Y - other.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Clamps the X and Y values of this vector within the specified bounds.
        /// </summary>
        /// <param name="minX">The minimum X value.</param>
        /// <param name="maxX">The maximum X value.</param>
        /// <param name="minY">The minimum Y value.</param>
        /// <param name="maxY">The maximum Y value.</param>
        /// <returns>A new clamped vector.</returns>
        public Vector2 Clamp(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2(
                Math.Clamp(X, minX, maxX),
                Math.Clamp(Y, minY, maxY)
            );
        }

        /// <summary>
        /// Returns a normalized version of this vector (same direction, length = 1).
        /// If the vector is zero-length, returns Vector2.Zero().
        /// </summary>
        /// <returns>A new normalized vector.</returns>
        public Vector2 Normalize()
        {
            float length = (float)Math.Sqrt(X * X + Y * Y);
            return length > 0 ? new Vector2(X / length, Y / length) : Vector2.Zero();
        }

        /// <summary>
        /// Calculates the dot product between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The scalar dot product value.</returns>
        public float DotProduct(Vector2 other)
        {
            return X * other.X + Y * other.Y;
        }

        /// <summary>
        /// Calculates the angle in degrees between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The angle between the vectors in degrees (0 to 180).</returns>
        public float AngleTo(Vector2 other)
        {
            float dot = this.Normalize().DotProduct(other.Normalize());
            return (float)(Math.Acos(Math.Clamp(dot, -1f, 1f)) * (180.0 / Math.PI));
        }
    }
}
