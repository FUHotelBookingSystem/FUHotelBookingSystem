using System;
using System.Collections.Generic;

namespace PRN222.Assignment.FUHotelBookingSystem.Repository.Model;

public partial class Booking
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RoomId { get; set; }

    public DateOnly BookingDate { get; set; }

    public DateTime? CheckinAt { get; set; }

    public DateTime? CheckinOut { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Room Room { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
