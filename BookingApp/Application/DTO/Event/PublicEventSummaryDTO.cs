using Application.DTO.Event;

namespace Application.DTO.Event
{
    public class PublicEventSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int MaxAttendees { get; set; }

        public AddressDTO Address { get; set; }
    }
}