using System.Collections.Generic;
using TeamsToDoApp.DataModel;

namespace TeamsToDoApp.Repository
{
    /// <summary>
    /// Represents the subscription repository class which stores the temporary data.
    /// </summary>
    public class SubscriptionRepository
    {
        public static List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}