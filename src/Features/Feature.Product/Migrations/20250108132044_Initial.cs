using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feature.Product.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "product");

            migrationBuilder.CreateTable(
                name: "ProductPlans",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Step = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanApprovalLines",
                schema: "product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductPlanId = table.Column<long>(type: "bigint", nullable: false),
                    ProductPlanId1 = table.Column<long>(type: "bigint", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanApprovalLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanApprovalLines_ProductPlans_ProductPlanId",
                        column: x => x.ProductPlanId,
                        principalSchema: "product",
                        principalTable: "ProductPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlanApprovalLines_ProductPlans_ProductPlanId1",
                        column: x => x.ProductPlanId1,
                        principalSchema: "product",
                        principalTable: "ProductPlans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanApprovalLines_ProductPlanId",
                schema: "product",
                table: "PlanApprovalLines",
                column: "ProductPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanApprovalLines_ProductPlanId1",
                schema: "product",
                table: "PlanApprovalLines",
                column: "ProductPlanId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanApprovalLines",
                schema: "product");

            migrationBuilder.DropTable(
                name: "ProductPlans",
                schema: "product");
        }
    }
}
