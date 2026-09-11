//Code by shirubaurufu
namespace MachineGeometry;

public enum GridDirection
{
    Up,
    Right,
    Down,
    Left,
}

public static GridDirection GetFacing(GameItem item, GridDirection localFacing = GridDirection.Up)
{
    var shape = ShapeOf(item);
    if (shape == null)
        return localFacing;

    var (dx, dy) = ToVector(localFacing);

    if (TryDeriveTransform(item, out var flip, out var turns))
        Apply(flip, turns, ref dx, ref dy);
    else
        Apply(shape.flipped, shape.orientation & 3, ref dx, ref dy);

    return FromVector(dx, dy, localFacing);
}

private static void Apply(bool flip, int turns, ref int dx, ref int dy)
{
    if (flip)
        dx = -dx;

    for (var turn = turns & 3; turn > 0; turn--)
    {
        var x = dy;
        var y = -dx;
        dx = x;
        dy = y;
    }
}

private static GridDirection FromVector(int dx, int dy, GridDirection fallback)
{
    if (dx == 0 && dy < 0) return GridDirection.Up;
    if (dx > 0 && dy == 0) return GridDirection.Right;
    if (dx == 0 && dy > 0) return GridDirection.Down;
    if (dx < 0 && dy == 0) return GridDirection.Left;
    return fallback;
}

public static bool TryDeriveTransform(GameItem item, out bool flipped, out int turns)
{
    flipped = false;
    turns = 0;

    var local = LocalShapeOf(item);
    var placed = item?.modifiedShape;
    if (local == null || placed == null)
        return false;

    var target = PlacedCells(placed);
    if (target.Count == 0)
        return false;

    var matches = 0;
    for (var candidateFlip = 0; candidateFlip < 2; candidateFlip++)
        for (var candidateTurns = 0; candidateTurns < 4; candidateTurns++)
        {
            if (!SameCells(LocalCells(local, candidateFlip == 1, candidateTurns), target))
                continue;

            matches++;
            flipped = candidateFlip == 1;
            turns = candidateTurns;
        }

    // More than one match means the footprint is symmetric under some transform, so it
    // cannot say which one was applied.
    if (matches == 1)
        return true;

    flipped = false;
    turns = 0;
    return false;
}

private static HashSet<int> PlacedCells(GridShape placed)
{
    var cells = new HashSet<int>();

    for (var y = placed.minY; y <= placed.maxY; y++)
        for (var x = placed.minX; x <= placed.maxX; x++)
            if (placed.Get(x, y) != 0)
                cells.Add(Key(x - placed.minX, y - placed.minY));

    return cells;
}

private static HashSet<int> PlacedCells(GridShape placed)
{
    var cells = new HashSet<int>();

    for (var y = placed.minY; y <= placed.maxY; y++)
        for (var x = placed.minX; x <= placed.maxX; x++)
            if (placed.Get(x, y) != 0)
                cells.Add(Key(x - placed.minX, y - placed.minY));

    return cells;
}

private static HashSet<int> LocalCells(GridShape local, bool flip, int turns)
{
    var transformed = new List<(int X, int Y)>();
    int minX = int.MaxValue, minY = int.MaxValue;

    for (var y = 0; y < local.height; y++)
        for (var x = 0; x < local.width; x++)
        {
            if (local.GetLocal(x, y) == 0)
                continue;

            var cx = x;
            var cy = y;
            Apply(flip, turns, ref cx, ref cy);

            transformed.Add((cx, cy));
            if (cx < minX) minX = cx;
            if (cy < minY) minY = cy;
        }

    var cells = new HashSet<int>();
    foreach (var (x, y) in transformed)
        cells.Add(Key(x - minX, y - minY));

    return cells;
}

private static bool SameCells(HashSet<int> a, HashSet<int> b) => a.Count == b.Count && a.SetEquals(b);


public static GameItem GetFacingNeighbour(GameItem item, GridDirection localFacing = GridDirection.Up) => GetNeighbour(item, GetFacing(item, localFacing));

public static GameItem GetNeighbour(GameItem item, GridDirection direction)
{
    var found = GetNeighbours(item, direction);
    return found.Count == 0 ? null : found[0];
}

public static List<GameItem> GetNeighbours(GameItem item, GridDirection direction)
{
    var found = new List<GameItem>();

    var shape = ShapeOf(item);
    var inventory = item?.parentInventory;
    if (shape == null || inventory == null)
        return found;

    var (dx, dy) = ToVector(direction);

    for (var y = shape.minY; y <= shape.maxY; y++)
        for (var x = shape.minX; x <= shape.maxX; x++)
        {
            if (!Occupies(shape, x, y))
                continue;

            // Only the leading edge: a cell whose neighbour is also ours is interior.
            var probeX = x + dx;
            var probeY = y + dy;
            if (Occupies(shape, probeX, probeY))
                continue;

            var neighbour = ItemAt(inventory, probeX, probeY, item);
            if (neighbour != null && !Contains(found, neighbour))
                found.Add(neighbour);
        }

    return found;
}

public static GridShape ShapeOf(GameItem item) => item == null ? null : item.modifiedShape ?? item.shape;

public static GridShape LocalShapeOf(GameItem item) => item?.shape;

public static int GetOrientation(GameItem item)
{
    var shape = ShapeOf(item);
    return shape == null ? 0 : shape.orientation & 3;
}

public static bool IsFlipped(GameItem item)
{
    var shape = ShapeOf(item);
    return shape != null && shape.flipped;
}

public static bool TryGetBounds(GameItem item, out int minX, out int minY, out int maxX, out int maxY)
{
    minX = minY = maxX = maxY = 0;

    var shape = ShapeOf(item);
    if (shape == null)
        return false;

    minX = shape.minX;
    minY = shape.minY;
    maxX = shape.maxX;
    maxY = shape.maxY;
    return true;
}

private static bool Occupies(GridShape shape, int x, int y) =>
    x >= shape.minX && x <= shape.maxX &&
    y >= shape.minY && y <= shape.maxY &&
    shape.Get(x, y) != 0;

public static GameItem ItemAt(GameInventory inventory, int x, int y) => ItemAt(inventory, x, y, null);

private static GameItem ItemAt(GameInventory inventory, int x, int y, GameItem ignore)
{
    var children = inventory?.childItems;
    if (children == null)
        return null;

    for (var i = 0; i < children.Count; i++)
    {
        var candidate = children[i];
        if (candidate == null || (ignore != null && candidate.Pointer == ignore.Pointer))
            continue;

        var shape = ShapeOf(candidate);
        if (shape != null && Occupies(shape, x, y))
            return candidate;
    }

    return null;
}