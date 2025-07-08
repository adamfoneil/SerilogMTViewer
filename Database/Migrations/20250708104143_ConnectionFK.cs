using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Service.Migrations
{
    /// <inheritdoc />
    public partial class ConnectionFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Connections",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                table: "Connections",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "HeaderSecret",
                table: "Connections",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "Connections",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationName",
                table: "Connections",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_OwnerUserId_ApplicationName",
                table: "Connections",
                columns: new[] { "OwnerUserId", "ApplicationName" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_AspNetUsers_OwnerUserId",
                table: "Connections",
                column: "OwnerUserId",
                principalTable: "AspNetUsers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_AspNetUsers_OwnerUserId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_OwnerUserId_ApplicationName",
                table: "Connections");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_UserId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Connections",
                newName: "DateModified");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Connections",
                newName: "DateCreated");

            migrationBuilder.AlterColumn<string>(
                name: "HeaderSecret",
                table: "Connections",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Endpoint",
                table: "Connections",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationName",
                table: "Connections",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
