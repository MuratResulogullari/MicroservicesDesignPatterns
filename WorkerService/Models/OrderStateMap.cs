using MassTransit.EntityFrameworkCoreIntegration.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WorkerService.Models
{
    public class OrderStateMap : SagaClassMap<OrderStateInstance>
    {
        protected override void Configure(EntityTypeBuilder<OrderStateInstance> entity, ModelBuilder model)
        {
            entity.ToTable("OrderStateInstances");
            entity.Property(x => x.CorrelationId).HasMaxLength(36).IsRequired();
            entity.HasKey(x => x.CorrelationId);
            entity.Property(x => x.UserId).HasMaxLength(36).IsRequired();
            entity.Property(x => x.OderderId).HasMaxLength(36).IsRequired();
            entity.Property(x => x.CurrentState).HasMaxLength(100).IsRequired();
            entity.Property(x => x.TotalPrice).HasPrecision(18, 2).IsRequired();
            entity.Property(x => x.CreatedDate).IsRequired();
            base.Configure(entity, model);
        }
    }
}