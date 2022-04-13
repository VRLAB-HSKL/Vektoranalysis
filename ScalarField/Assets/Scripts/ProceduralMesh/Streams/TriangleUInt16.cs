﻿using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMesh.Streams
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TriangleUInt16
    {
        public ushort a;
        public ushort b;
        public ushort c;

        public static implicit operator TriangleUInt16(int3 t) => new TriangleUInt16()
        {
            a = (ushort) t.x,
            b = (ushort) t.y,
            c = (ushort) t.z
        };
    }
}