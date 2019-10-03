using System;

namespace Services.Hydra.WebApi.Models
{
    public class NotificationHistory : Entity
    {
        public DateTimeOffset NotificationTime { get; set; }

        public override string TypeName => nameof(NotificationHistory);
    }
}
