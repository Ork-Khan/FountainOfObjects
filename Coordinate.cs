namespace FountainOfObjects
{
    public struct Coordinate
    {
        public int Row { get; }
        public int Column { get; }

        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public override string ToString()
        {
            return $"(Row {Row}, Column {Column})";
        }

        public static bool operator ==(Coordinate obj1, Coordinate obj2) =>
         obj1.Row == obj2.Row && obj1.Column == obj2.Column;

        public static bool operator !=(Coordinate obj1, Coordinate obj2) =>
            !(obj1 == obj2);

        public override bool Equals(object? obj) =>
            obj is Coordinate other && Row == other.Row && Column == other.Column;

        public override int GetHashCode() =>
            HashCode.Combine(Row, Column);
    }
}
