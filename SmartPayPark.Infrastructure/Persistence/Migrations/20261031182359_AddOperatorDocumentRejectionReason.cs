using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPayPark.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOperatorDocumentRejectionReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "OperatorDocuments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "OperatorDocuments");
        }
    }
}
