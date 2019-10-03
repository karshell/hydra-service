using System;

namespace Services.Hydra.WebApi.Models
{
    public class ContainerFillState : Entity
    {
        public Container Container { get; set; }

        public bool FillState { get; set; }

        public DateTimeOffset? Timestamp { get; set; }
        
        public override string TypeName => nameof(ContainerFillState);
    }
}
