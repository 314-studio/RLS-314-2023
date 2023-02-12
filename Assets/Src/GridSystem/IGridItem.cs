namespace Src.GridSystem
{
    public interface IGridItem
    {
        public string Type => GetType().Name;
    }
}