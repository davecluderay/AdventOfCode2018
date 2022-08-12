namespace Aoc2018_Day08;

internal class Solution
{
    public string Title => "Day 8: Memory Maneuver";

    public object PartOne()
    {
        var data = ReadNodeData();
        var node = Node.Parse(data);
        return CalculateSumOfMetadata(node);
    }
        
    public object PartTwo()
    {
        var data = ReadNodeData();
        var node = Node.Parse(data);
        return CalculateValue(node);
    }

    private int CalculateSumOfMetadata(Node node)
        => node.Metadata.Sum() + 
           node.Children.Sum(CalculateSumOfMetadata);

    private int CalculateValue(Node node)
    {
        if (!node.Children.Any())
        {
            return node.Metadata.Sum();
        }

        return node.Metadata.Where(m => m > 0 && m <= node.Children.Count)
                   .Sum(m => CalculateValue(node.Children[m - 1]));
    }

    private int[] ReadNodeData()
        => InputFile.ReadAllLines()
                    .SelectMany(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    .Select(s => Convert.ToInt32(s))
                    .ToArray();
}

public class Node
{
    public IReadOnlyList<Node> Children { get; }
    public IReadOnlyList<int> Metadata { get; }
    private int DataLength { get; }

    private Node(IReadOnlyList<Node> children, IReadOnlyList<int> metadata, int dataLength)
        => (Children, Metadata, DataLength) = (children, metadata, dataLength);

    public static Node Parse(IReadOnlyList<int> data, int index = 0)
    {
        var childCount = data[index];
        var metadataCount = data[index + 1];
        var dataLength = metadataCount + 2;
        index += 2;

        var children = new List<Node>(childCount);
        for (var i = 0; i < childCount; i++)
        {
            var child = Parse(data, index);
            dataLength += child.DataLength;
            index += child.DataLength;
            children.Add(child);
        }

        var metadata = Enumerable.Range(index, metadataCount)
                                 .Select(i => data[i])
                                 .ToList();

        return new Node(children, metadata, dataLength);
    }
}
