namespace Aoc2018_Day23;

internal readonly record struct Box(int MinX, int MinY, int MinZ, int MaxX, int MaxY, int MaxZ)
{
    public int SizeX { get; } = MaxX - MinX;
    public int SizeY { get; } = MaxY - MinY;
    public int SizeZ { get; } = MaxZ - MinZ;

    public Box WithCentre(Position centre)
        => new(centre.X - SizeX / 2,
               centre.Y - SizeY / 2,
               centre.Z - SizeZ / 2,
               centre.X + SizeX - SizeX / 2,
               centre.Y + SizeY - SizeY / 2,
               centre.Z + SizeZ - SizeZ / 2);

    public Box ScaledDownBy(int factor)
        => new(MinX + SizeX / 2 / factor,
               MinY + SizeY / 2 / factor,
               MinZ + SizeZ / 2 / factor,
               MinX + SizeX / 2 / factor + SizeX / factor,
               MinY + SizeY / 2 / factor + SizeY / factor,
               MinZ + SizeZ / 2 / factor + SizeZ / factor);

    public static implicit operator Box((int MinX, int MinY, int MinZ, int MaxX, int MaxY, int MaxZ) box)
        => new(box.MinX, box.MinY, box.MinZ, box.MaxX, box.MaxY, box.MaxZ);
}
