using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utility
{
    #region Float Extension Methods

    public static float Round(float value, int decimals)
    {
        float powerOf10 = Mathf.Pow(10, decimals);
        return Mathf.Round(value * powerOf10) / powerOf10;
    }

    #endregion

    #region Vector3 Extension Methods
    //2022-07-10: copied from Stonicorn.Utility
    public static Vector3 setX(this Vector3 v, float x)
        => new Vector3(x, v.y, v.z);
    public static Vector3 setY(this Vector3 v, float y)
        => new Vector3(v.x, y, v.z);
    public static Vector3 setZ(this Vector3 v, float z)
        => new Vector3(v.x, v.y, z);
    #endregion

    #region Color Extension Methods
    public static Color setAlpha(this Color c, float a)
    {
        c.a = a;
        return c;
    }
    #endregion

    public static bool HitWall(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        return Mathf.Abs(normal.x) > Mathf.Abs(normal.y);
    }

    public static bool HitFloor(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        return normal.y > 0 && Mathf.Abs(normal.y) > Mathf.Abs(normal.x);
    }

    public static bool HitCeiling(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal;
        return normal.y < 0 && Mathf.Abs(normal.y) > Mathf.Abs(normal.x);
    }

    //2022-07-09: copied from Stonicorn.Utility
    public static Plane raycastPlane = new Plane(Vector3.forward, Vector3.zero);
    public static Vector2 ScreenToWorldPoint(Vector3 screenPoint)
    {
        //2019-01-28: copied from an answer by Tomer-Barkan: https://answers.unity.com/questions/566519/camerascreentoworldpoint-in-perspective.html
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        float distance;
        raycastPlane.Raycast(ray, out distance);
        return ray.GetPoint(distance);
    }

    public static T GetComponentInScene<T>(Scene scene) where T : MonoBehaviour
    {
        return GameObject.FindObjectsByType<T>(FindObjectsSortMode.InstanceID).ToList()
            .Find(comp => comp.gameObject.scene == scene)
            ?? default;
    }
    public static T GetComponentInScene<T>(string sceneName) where T : MonoBehaviour
    {
        return GameObject.FindObjectsByType<T>(FindObjectsSortMode.InstanceID).ToList()
            .Find(comp => comp.gameObject.scene.name == sceneName)
            ?? default;
    }
}
