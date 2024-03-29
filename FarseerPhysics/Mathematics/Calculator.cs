using System;
using System.Collections.Generic;
using System.Text;

#if (XNA)
using Microsoft.Xna.Framework;
#endif

using FarseerGames.FarseerPhysics.Collisions;

namespace FarseerGames.FarseerPhysics.Mathematics {
    public static class Calculator {
        public const float TwoPi = 6.28318531f;
        public const float DegreesToRadiansRatio = 57.29577957855f;
        public const float RadiansToDegreesRatio = 1f / 57.29577957855f;
        private static Random random = new Random();

        public static float Sin(float angle) {
            return (float)Math.Sin((double)angle);
        }

        public static float Cos(float angle) {
            return (float)Math.Cos((double)angle);
        }

        public static float ACos(float value) {
            return (float)Math.Acos((double)value);
        }

        public static float ATan2(float y, float x) {
            return (float)Math.Atan2((double)y, (double)x);
        }

        //performs bilinear interpolation of a point
        public static float BiLerp(Vector2 point, Vector2 min, Vector2 max, float value1, float value2, float value3, float value4, float minValue, float maxValue) {
            float x = point.X;
            float y = point.Y;
            float value;

            x = MathHelper.Clamp(x, min.X, max.X);
            y = MathHelper.Clamp(y, min.Y, max.Y);

            float xRatio = (x - min.X) / (max.X - min.X);
            float yRatio = (y - min.Y) / (max.Y - min.Y);

            float top = MathHelper.Lerp(value1, value4, xRatio);
            float bottom = MathHelper.Lerp(value2, value3, xRatio);

            value = MathHelper.Lerp(top, bottom, yRatio);
            value = MathHelper.Clamp(value, minValue, maxValue);
            return value;
        }

        public static float Clamp(float value, float low, float high) {
            return Math.Max(low, Math.Min(value, high));
        }

        public static float DistanceBetweenPointAndPoint(Vector2 point1, Vector2 point2) {
            Vector2 v = Vector2.Subtract(point1, point2);
            return v.Length();
        }

        public static float DistanceBetweenPointAndLineSegment(Vector2 point, Vector2 lineEndPoint1, Vector2 lineEndPoint2) {
            Vector2 v = Vector2.Subtract(lineEndPoint2, lineEndPoint1);
            Vector2 w = Vector2.Subtract(point, lineEndPoint1);

            float c1 = Vector2.Dot(w, v);
            if (c1 <= 0) return DistanceBetweenPointAndPoint(point, lineEndPoint1);

            float c2 = Vector2.Dot(v, v);
            if (c2 <= c1) return DistanceBetweenPointAndPoint(point, lineEndPoint2);

            float b = c1 / c2;
            Vector2 pointOnLine = Vector2.Add(lineEndPoint1, Vector2.Multiply(v, b));
            return DistanceBetweenPointAndPoint(point, pointOnLine);
        }

        public static float Cross(Vector2 value1, Vector2 value2) {
            return value1.X * value2.Y - value1.Y * value2.X;
        }

        public static Vector2 Cross(Vector2 value1, float value2) {
            return new Vector2(value2 * value1.Y, -value2 * value1.X);
        }

        public static Vector2 Cross(float value2, Vector2 value1) {
            return new Vector2(-value2 * value1.Y, value2 * value1.X);
        }

        public static void Cross(ref Vector2 value1, ref Vector2 value2, out float ret) {
            ret = value1.X * value2.Y - value1.Y * value2.X;
        }

        public static void Cross(ref Vector2 value1, ref float value2, out Vector2 ret) {
            ret = value1; //necassary to get past a compile error on 360
            ret.X = value2 * value1.Y;
            ret.Y = -value2 * value1.X;
        }

        public static void Cross(ref float value2, ref Vector2 value1, out Vector2 ret) {
            ret = value1;//necassary to get past a compile error on 360
            ret.X = -value2 * value1.Y;
            ret.Y = value2 * value1.X;
        }

        public static Vector2 Project(Vector2 projectVector, Vector2 onToVector) {
            float multiplier = 0;
            float numerator = (onToVector.X * projectVector.X + onToVector.Y * projectVector.Y);
            float denominator = (onToVector.X * onToVector.X + onToVector.Y * onToVector.Y);

            if (denominator != 0) {
                multiplier = numerator / denominator;
            }

            return Vector2.Multiply(onToVector, multiplier);
        }

        public static void Truncate(ref Vector2 vector, float maxLength, out Vector2 truncatedVector) {
            float length = vector.Length();
            length = Math.Min(length, maxLength);
            if (length > 0) {
                vector.Normalize();
            }
            Vector2.Multiply(ref vector, length, out truncatedVector);
        }

        public static float DegreesToRadians(float degrees) {
            return degrees * RadiansToDegreesRatio;
        }

        public static float RandomNumber(float min, float max)
        {
            return (float)((max - min) * random.NextDouble() + min);
        }

        public static bool IsBetweenNonInclusive(float number, float min, float max)
        {
            if (number > min && number < max)
            {
                return true;
            }
            else {
                return false;
            }

        }

        /// Temp variables to speed up the following code.
        private static float tPow2;
        private static float wayToGo;
        private static float wayToGoPow2;

        private static Vector2 startCurve;
        private static Vector2 curveEnd;
        private static Vector2 _temp;

        public static float VectorToRadians(Vector2 vector)
        {
            return (float)Math.Atan2((double)vector.X, -(double)vector.Y);
        }

        public static Vector2 RadiansToVector(float radians)
        {
            return new Vector2((float)Math.Sin((double)radians), -(float)Math.Cos((double)radians));
        }

        public static void RadiansToVector(float radians, ref Vector2 vector)
        {
            vector.X = (float)Math.Sin((double)radians);
            vector.Y = -(float)Math.Cos((double)radians);
        }

        public static void RotateVector(ref Vector2 vector, float radians)
        {
            float length = vector.Length();
            float newRadians = (float)Math.Atan2((double)vector.X, -(double)vector.Y) + radians;

            vector.X = (float)Math.Sin((double)newRadians) * length;
            vector.Y = -(float)Math.Cos((double)newRadians) * length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="t">Value between 0.0f and 1.0f.</param>
        /// <returns></returns>
        public static Vector2 LinearBezierCurve(Vector2 start, Vector2 end, float t)
        {
            return start + (end - start) * t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="curve"></param>
        /// <param name="end"></param>
        /// <param name="t">Value between 0.0f and 1.0f.</param>
        /// <returns></returns>
        public static Vector2 QuadraticBezierCurve(Vector2 start, Vector2 curve, Vector2 end, float t)
        {
            wayToGo = 1.0f - t;

            return wayToGo * wayToGo * start
                   + 2.0f * t * wayToGo * curve
                   + t * t * end;
        }

        public static Vector2 QuadraticBezierCurve(Vector2 start, Vector2 curve, Vector2 end, float t, ref float radians)
        {
            startCurve = start + (curve - start) * t;
            curveEnd = curve + (end - curve) * t;
            _temp = curveEnd - startCurve;

            radians = (float)Math.Atan2((double)_temp.X, -(double)_temp.Y);
            return startCurve + _temp * t;
        }

        public static Vector2 CubicBezierCurve2(Vector2 start, Vector2 startPointsTo, Vector2 end, Vector2 endPointsTo, float t)
        {
            return CubicBezierCurve(start, start + startPointsTo, end + endPointsTo, end, t);
        }

        public static Vector2 CubicBezierCurve2(Vector2 start, Vector2 startPointsTo, Vector2 end, Vector2 endPointsTo, float t, ref float radians)
        {
            return CubicBezierCurve(start, start + startPointsTo, end + endPointsTo, end, t, ref radians);
        }

        public static Vector2 CubicBezierCurve2(Vector2 start, float startPointDirection, float startPointLength,
                                                Vector2 end, float endPointDirection, float endPointLength,
                                                float t, ref float radians)
        {
            return CubicBezierCurve(start,
                                    Calculator.RadiansToVector(startPointDirection) * startPointLength,
                                    Calculator.RadiansToVector(endPointDirection) * endPointLength,
                                    end,
                                    t,
                                    ref radians);
        }

        public static Vector2 CubicBezierCurve(Vector2 start, Vector2 curve1, Vector2 curve2, Vector2 end, float t)
        {
            tPow2 = t * t;
            wayToGo = 1.0f - t;
            wayToGoPow2 = wayToGo * wayToGo;

            return wayToGo * wayToGoPow2 * start
                   + 3.0f * t * wayToGoPow2 * curve1
                   + 3.0f * tPow2 * wayToGo * curve2
                   + t * tPow2 * end;
        }

        public static Vector2 CubicBezierCurve(Vector2 start, Vector2 curve1, Vector2 curve2, Vector2 end, float t, ref float radians)
        {
            return QuadraticBezierCurve(start + (curve1 - start) * t,
                                        curve1 + (curve2 - curve1) * t,
                                        curve2 + (end - curve2) * t,
                                        t,
                                        ref radians);
        }

        //Interpolate normal vectors ...
        public static Vector2 InterpolateNormal(Vector2 vector1, Vector2 vector2, float t)
        {
            vector1 += (vector2 - vector1) * t;
            vector1.Normalize();

            return vector1;
        }

        public static void InterpolateNormal(Vector2 vector1, Vector2 vector2, float t, out Vector2 vector)
        {
            vector = vector1 + (vector2 - vector1) * t;
            vector.Normalize();
        }

        public static void InterpolateNormal(ref Vector2 vector1, Vector2 vector2, float t)
        {
            vector1 += (vector2 - vector1) * t;
            vector1.Normalize();
        }

        public static float InterpolateRotation(float radians1, float radians2, float t)
        {
            Vector2 vector1 = new Vector2((float)Math.Sin((double)radians1), -(float)Math.Cos((double)radians1));
            Vector2 vector2 = new Vector2((float)Math.Sin((double)radians2), -(float)Math.Cos((double)radians2));

            vector1 += (vector2 - vector1) * t;
            vector1.Normalize();

            return (float)Math.Atan2((double)vector1.X, -(double)vector1.Y);
        }

        public static void ProjectToAxis(ref Vector2[]points,  ref Vector2 axis, out float min, out float max)
        {
            // To project a point on an axis use the dot product
            float dotProduct = Vector2.Dot(axis, points[0]);
            min = dotProduct;
            max = dotProduct;

            for (int i = 0; i < points.Length; i++)
            {
                dotProduct = Vector2.Dot(points[i], axis);
                if (dotProduct < min)
                {
                    min = dotProduct;
                }
                else
                {
                    if (dotProduct > max)
                    {
                        max = dotProduct;
                    }
                }
            }
        }
    }
}
