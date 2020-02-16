using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class Utility
{

    /// <summary>
    /// Get closest tile that is in tilemap
    /// </summary>
    /// <param name="x">genesis x</param>
    /// <param name="y">genesis y</param>
    /// <param name="tilemap">map of valid tiles</param>
    /// <param name="unavailableTiles">tiles we want to avoid</param>
    /// <returns>Vector2 of nearest walkable ground</returns>
    public static Vector2 GetClosestWalkableTile(int x, int y, Tilemap tilemap, List<Vector2> unavailableTiles = null) {
        // If this tile is walkable, return it. (as long as it's not a nono!)
        if (tilemap.GetTile(new Vector3Int(x, y, 0)) && (unavailableTiles == null || !unavailableTiles.Contains(new Vector2(x, y))))
            return new Vector2(x, y);

        var level = 0;
        while (level < 100) { // arbirtaray stop condition to prevent it from spiraling out of control
            level++;        // no pun intended

            for (int i = x - level; i <= x + level; i++)
                for (int j = y - level; j <= y + level; j++)
                    // only check if this tile is on the edge of the ring
                    if ((i == x - level || i == x + level) || (j == y - level || j == y + level) && 
                        // and it isn't in our nono list
                        (unavailableTiles == null || !unavailableTiles.Contains(new Vector2(x, y)))) {
                        if (tilemap.GetTile(new Vector3Int(i, j, 0)))
                            return new Vector2(i, j);
                    }
        }
        // this shouldn't happen
        return new Vector2();
    }
    public static Vector2 GetClosestWalkableTile(Vector2 v, Tilemap t, List<Vector2> touchedTiles = null) {
        return Utility.GetClosestWalkableTile((int)v.x, (int)v.y, t, touchedTiles);
    }



    /// <summary>
    /// Set the angle and the distance from the center of (x,y)
    /// </summary>
    /// <param name="radius">distance from center</param>
    /// <param name="angle">angle relative to screen</param>
    /// <param name="x">x</param>
    /// <param name="y">y</param>
    /// <returns></returns>
    public static Vector2 GetSpecificPointInCircle(float radius, float angle, float x, float y) {
        return new Vector2(x + (radius * Mathf.Cos(angle)), y + (radius * Mathf.Sin(angle)));

    }


    public static Vector2 RandomPointInCircle(float radius, float x, float y) {

        //https://programming.guide/random-point-within-circle.html


        System.Random random = new System.Random();

        float angle = (float)random.NextDouble() * 2 * Mathf.PI;

        // 1 + (r-1) because I don't want the inner circle to be the target(impassable terrain)
        float r = 1 + (radius - 1 * Mathf.Sqrt((float)random.NextDouble()));

        return new Vector2(x + (r * Mathf.Cos(angle)), y + (r * Mathf.Sin(angle)));
    }
}
