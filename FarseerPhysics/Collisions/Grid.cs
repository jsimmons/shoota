using System;
using System.Collections.Generic;
using System.Text;

#if (XNA)
using Microsoft.Xna.Framework; 
#endif

using FarseerGames.FarseerPhysics.Mathematics;

namespace FarseerGames.FarseerPhysics.Collisions {
    public sealed class Grid {
        float gridCellSize;
        float gridCellSizeInv;
        AABB aabb;
        float[,] nodes;
        Vector2[] points;

        public Vector2[] Points {
            get { return points; }
        }

        public Grid Clone() {
            Grid grid = new Grid();
            grid.gridCellSize = this.gridCellSize;
            grid.gridCellSizeInv = this.gridCellSizeInv;
            grid.aabb = this.aabb;
            grid.nodes = (float[,])this.nodes.Clone();
            grid.points = (Vector2[])this.points.Clone();
            return grid;
        }

        public void ComputeGrid(Geom geometry, float gridCellSize) {
            //prepare the geometry.
            Matrix old = geometry.Matrix;
            Matrix identity = Matrix.Identity;
            geometry.Matrix = identity;

            aabb = new AABB(geometry.AABB);
            this.gridCellSize = gridCellSize;
            this.gridCellSizeInv = 1 / gridCellSize;
            
            int xSize = (int)Math.Ceiling(Convert.ToDouble((aabb.Max.X - aabb.Min.X) * gridCellSizeInv)) + 1;
            int ySize = (int)Math.Ceiling(Convert.ToDouble((aabb.Max.Y - aabb.Min.Y) * gridCellSizeInv)) +1;

            this.nodes = new float[xSize, ySize];
            points = new Vector2[xSize * ySize];
            int i = 0;
            Vector2 vector = aabb.Min;
            for (int x = 0; x < xSize; ++x, vector.X += gridCellSize) {
                vector.Y = aabb.Min.Y;
                for (int y = 0; y < ySize; ++y, vector.Y += gridCellSize) {
                    nodes[x, y] = geometry.GetNearestDistance(vector);// shape.GetDistance(vector);
                    points[i] = vector;
                    i += 1;
                }
            }
            //restore the geometry
            geometry.Matrix = old;   
        }

        public bool Intersect(ref Vector2 vector, out Feature feature) {
            //TODO: Keep and eye out for floating point accuracy issues here. Possibly some
            //VERY intermittent errors exist?
            if (aabb.Contains(ref vector)) {
                int x = (int)Math.Floor((vector.X - aabb.Min.X) * gridCellSizeInv);
                int y = (int)Math.Floor((vector.Y - aabb.Min.Y) * gridCellSizeInv);
                

                float xPercent = (vector.X - (gridCellSize * x + aabb.Min.X)) * gridCellSizeInv;
                float yPercent = (vector.Y - (gridCellSize * y + aabb.Min.Y)) * gridCellSizeInv;

                float bottomLeft = nodes[x, y];
                float bottomRight = nodes[x + 1, y];
                float topLeft = nodes[x, y + 1];
                float topRight = nodes[x + 1, y + 1];

                if (bottomLeft <= 0 ||
                    bottomRight <= 0 ||
                    topLeft <= 0 ||
                    topRight <= 0) {
                    float top, bottom, distance;

                    top = MathHelper.Lerp(topLeft, topRight, xPercent);
                    bottom = MathHelper.Lerp(bottomLeft, bottomRight, xPercent);
                    distance = MathHelper.Lerp(bottom, top, yPercent);

                    if (distance <= 0) {
                        float right, left;

                        right = MathHelper.Lerp(bottomRight, topRight, yPercent);
                        left = MathHelper.Lerp(bottomLeft, topLeft, yPercent);

                        Vector2 normal = Vector2.Zero;
                        normal.X = right - left;
                        normal.Y = top - bottom;
                        Vector2.Normalize(ref normal, out normal);

                        feature = new Feature(vector,normal,distance);
                        return true;
                    }
                }
            }
            feature = new Feature();
            //feature = null;
            return false;
        }
    }

}
