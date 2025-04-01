using System;
using System.Collections.Generic;

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

public partial class Room
{
    public int Id { get; set; }

    public int HotelId { get; set; }

    public string RoomNumber { get; set; } = null!;

    public string Type { get; set; } = null!;

    public decimal Price { get; set; }

    public string Status { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Hotel Hotel { get; set; } = null!;
}
