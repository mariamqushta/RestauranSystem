using Microsoft.AspNetCore.Identity;

namespace RestaurantReservationSystem.Models;

public class ApplicationUser : IdentityUser
{

    public string FullName { get; set; } = string.Empty;


    public ICollection<Reservation> Reservations { get; set; }
        = new List<Reservation>();


    public ICollection<Review> Reviews { get; set; }
        = new List<Review>();
}