﻿using System;

public struct Double3
{
    public double x, y, z;

    public byte[] Bytes()
    {
        var temp = new byte[sizeof(double) * 3];
        BitConverter.GetBytes(x).CopyTo(temp, 0);
        BitConverter.GetBytes(y).CopyTo(temp, sizeof(double));
        BitConverter.GetBytes(z).CopyTo(temp, sizeof(double) * 2);
        return temp;
    }

    public Double3(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public static bool operator ==(Double3 a, Double3 b)
    {
        return a.x == b.x && a.y == b.y && a.z == b.z;
    }

    public static bool operator !=(Double3 a, Double3 b)
    {
        return a.x != b.x || a.y != b.y || a.z != b.z;
    }

    public static Double3 operator +(Double3 a, Double3 b)
    {
        return new Double3(a.x + b.x, a.y + b.y, a.z + b.z);
    }
}
