namespace WhiteLagoon.Application.Common.Utility;

/// <summary>
/// Static details that can be used in entire solution.
/// </summary>
public static class StaticDetails
{
    #region HelperFields
    /// <summary>
    /// If user is admin.
    /// </summary>
    public const string RoleAdmin = "Admin";
    /// <summary>
    /// If user is customer.
    /// </summary>
    public const string RoleCustomer = "Customer";
    /// <summary>
    /// If the payment is not done.
    /// </summary>
    public const string StatusPending = "Pending";
    /// <summary>
    /// If the payment is approved.
    /// </summary>
    public const string StatusApproved = "Approved";
    /// <summary>
    /// If the user checked into the villa.
    /// </summary>
    public const string StatusCheckIn = "CheckIn";
    /// <summary>
    /// User has completed his stay in the villa.
    /// </summary>
    public const string StatusCompleted = "Completed";
    /// <summary>
    /// Villa assign to the user has been cancelled.
    /// </summary>
    public const string StatusCancelled = "Cancelled";
    /// <summary>
    /// The payement is paid back to the user.
    /// </summary>
    public const string StatusRefunded = "Refunded";
    #endregion

    /// <summary>
    /// Helps to check how many villas are available based on the nights and check in date.
    /// </summary>
    /// <param name="villaId">Villa ID.</param>
    /// <param name="villaNumberList">Villa room numbers of that villa.</param>
    /// <param name="checkInDate">CheckIn date of the user.</param>
    /// <param name="nights">Number of nights of the user.</param>
    /// <param name="bookings">Available bookings from the database.</param>
    /// <returns>Count of available villa.</returns>
    public static int VillaRoomsAvailableCount(int villaId, List<VillaNumber> villaNumberList, DateTime checkInDate, int nights, List<Booking> bookings)
    {
        int roomsInVilla = villaNumberList.Where(v => v.VillaId == villaId).Count();
        int finalAvailableRoomsForAllNights = int.MaxValue;
        List<int> bookingInDate = new();

        for (int i = 0; i < nights; i++)
        {
            var villasBooked = bookings.Where(v => v.VillaId == villaId && v.CheckInDate <= checkInDate.AddDays(i) && v.CheckOutDate > checkInDate.AddDays(i));

            foreach (var booking in villasBooked)
            {
                if (!bookingInDate.Contains(booking.Id))
                {
                    bookingInDate.Add(booking.Id);
                }
            }

            var totalAvailableRooms = roomsInVilla - bookingInDate.Count();

            if (totalAvailableRooms == 0) return 0;
            else
            {
                if (finalAvailableRoomsForAllNights > totalAvailableRooms)
                {
                    finalAvailableRoomsForAllNights = totalAvailableRooms;
                }
            }
        }

        return finalAvailableRoomsForAllNights;
    }
}
