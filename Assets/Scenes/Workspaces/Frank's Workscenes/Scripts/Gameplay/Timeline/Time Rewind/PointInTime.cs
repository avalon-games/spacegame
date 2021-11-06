using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointInTime
{
    private Vector2 position;
    private Quaternion rotation;
    private bool right;

    public PointInTime(Vector2 _position, Quaternion _rotation, bool _right)
    {
        this.position = _position;
        this.rotation = _rotation;
        this.right = _right;
    }

    public Vector2 GetPosition()
    {
        return this.position;
    }

    public Quaternion GetRotation()
    {
        return this.rotation;
    }

    public bool GetFaceRight()
    {
        return this.right;
    }
}
