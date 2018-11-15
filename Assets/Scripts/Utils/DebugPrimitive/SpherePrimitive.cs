﻿using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class SpherePrimitive: MonoBehaviour , INetworkSerializable
{
    public Vector3 center;
    public float radius;
    public Color color;
    
    public void Serialize(ref NetworkWriter writer, IEntityReferenceSerializer refSerializer)
    {
        writer.WriteVector3Q("center",center,3);
        writer.WriteFloatQ("radius", radius,3);
        writer.WriteVector3Q("color",new Vector3(color.r, color.g, color.b),2);
    }

    public void Deserialize(ref NetworkReader reader, IEntityReferenceSerializer refSerializer, int tick)
    {
        center = reader.ReadVector3Q();
        radius = reader.ReadFloatQ();
        var v = reader.ReadVector3Q();
        color = new Color(v.x, v.y, v.z);
    }
}

[DisableAutoCreation]
public class DrawSpherePrimitives : ComponentSystem
{
    struct SphereGroup
    {
        [ReadOnly]
        public ComponentArray<SpherePrimitive> spheres;
    }

    [Inject]
    SphereGroup Group;

    protected override void OnUpdate()
    {
        for (int i = 0, c = Group.spheres.Length; i < c; i++)
        {
            var sphere = Group.spheres[i];
            DebugDraw.Sphere(sphere.center, sphere.radius, sphere.color);
        }
    }
}