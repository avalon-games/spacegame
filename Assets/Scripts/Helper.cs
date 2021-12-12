using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * Helper for finding game objects in the scene through tags,
 * much faster than unity's default find methods
 * 
 * Source: https://answers.unity.com/questions/893966/how-to-find-child-with-tag.html
 * 
 */
public static class Helper {
    public static T[] FindComponentsInChildrenWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }
        List<T> list = new List<T>(parent.GetComponentsInChildren<T>(forceActive));
        if (list.Count == 0) { return null; }

        for (int i = list.Count - 1; i >= 0; i--) {
            if (list[i].CompareTag(tag) == false) {
                list.RemoveAt(i);
            }
        }
        return list.ToArray();
    }

    public static T FindComponentInChildWithTag<T>(this GameObject parent, string tag, bool forceActive = false) where T : Component {
        if (parent == null) { throw new System.ArgumentNullException(); }
        if (string.IsNullOrEmpty(tag) == true) { throw new System.ArgumentNullException(); }

        T[] list = parent.GetComponentsInChildren<T>(forceActive);
        foreach (T t in list) {
            if (t.CompareTag(tag) == true) {
                return t;
            }
        }
        return null;
    }
}