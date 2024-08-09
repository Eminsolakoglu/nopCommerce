using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Shipping.RoutePlanner.Domain.Data;

namespace Nop.Plugin.Shipping.RoutePlanner.Domain.Builder
{
    public partial class RoutePlannerTablosuBuilder :NopEntityBuilder<RoutePlannerTablosu>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(RoutePlannerTablosu.State)).AsString().NotNullable()
                .WithColumn(nameof(RoutePlannerTablosu.State)).AsString().Nullable()
                
                .WithColumn(nameof(RoutePlannerTablosu.OrderCount)).AsInt32().NotNullable();
        }
    }
}
