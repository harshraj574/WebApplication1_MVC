using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class spCalculateTotalDisplayOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string procedure = @"CREATE PROCEDURE [dbo].[CalculateTotalDisplayOrders]
                                    AS
                                        BEGIN
                                            SELECT SUM(DisplayOrder) AS TotalDisplayOrder
                                            FROM categories;
                                        END";

			 migrationBuilder.Sql(procedure);
            
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string procedure = @"Drop PROCEDURE [dbo].[CalculateTotalDisplayOrders]";
            migrationBuilder.Sql(procedure);
		}
    }
}
