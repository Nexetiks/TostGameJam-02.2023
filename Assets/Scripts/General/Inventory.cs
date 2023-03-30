using System.Collections.Generic;

public class Inventory
{
    public class DuplicatesComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x <= y ? -1 : 1;
        }
    }

    public SortedList<int, Gatherable> Gatherable;

    public Inventory()
    {
        Gatherable = new SortedList<int, Gatherable>(new DuplicatesComparer());
    }
}