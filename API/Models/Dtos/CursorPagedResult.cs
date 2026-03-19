namespace API.Models.Dtos
{
    public class CursorPagedResult<T>
    {
        public List<T> Data { get; set; } = new List<T>();

        public DateTime? NextCursor { get; set; }
    }
}