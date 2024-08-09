using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Shipping.RoutePlanner.Domain.Data;

namespace Nop.Plugin.Shipping.RoutePlanner.Domain
{
    [NopMigration("2024/08/07 00:00:00", "RoutePlannerTablosu schema migration", MigrationProcessType.Installation)]
    public class RoutePlannerSchemaMigration : MigrationBase
    {
        public override void Down()
        {
            //nothing
        }

        public override void Up()
        {
            if (!Schema.Table(nameof(RoutePlannerTablosu)).Exists())
            {
                Create.TableFor<RoutePlannerTablosu>();
            }
        }
    }
}
