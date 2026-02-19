namespace Application.DTO.Event
{
    public class EventSummaryDTO
    {
        public string ImageUrl { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Price { get; set; }
    }
}