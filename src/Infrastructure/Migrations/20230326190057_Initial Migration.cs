using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lattice.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_account",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    owner_id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team", x => x.id);
                    table.ForeignKey(
                        name: "FK_team_user_account_owner_id",
                        column: x => x.owner_id,
                        principalTable: "user_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "board",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    created_by = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    team_id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_board", x => x.id);
                    table.ForeignKey(
                        name: "FK_board_team_team_id",
                        column: x => x.team_id,
                        principalTable: "team",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_board_user_account_created_by",
                        column: x => x.created_by,
                        principalTable: "user_account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user_team",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    team_id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    user_id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_team", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_team_team_team_id",
                        column: x => x.team_id,
                        principalTable: "team",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_user_team_user_account_user_id",
                        column: x => x.user_id,
                        principalTable: "user_account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "section",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    board_id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_section", x => x.id);
                    table.ForeignKey(
                        name: "FK_section_board_board_id",
                        column: x => x.board_id,
                        principalTable: "board",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "card",
                columns: table => new
                {
                    id = table.Column<decimal>(type: "decimal(20,0)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    assigned_to = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    section_id = table.Column<decimal>(type: "decimal(20,0)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<decimal>(type: "decimal(20,0)", nullable: true),
                    completed_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card", x => x.id);
                    table.ForeignKey(
                        name: "FK_card_section_section_id",
                        column: x => x.section_id,
                        principalTable: "section",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_card_user_account_assigned_to",
                        column: x => x.assigned_to,
                        principalTable: "user_account",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_card_user_account_created_by",
                        column: x => x.created_by,
                        principalTable: "user_account",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_board_created_by",
                table: "board",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_board_team_id",
                table: "board",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_card_assigned_to",
                table: "card",
                column: "assigned_to");

            migrationBuilder.CreateIndex(
                name: "IX_card_created_by",
                table: "card",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_card_section_id",
                table: "card",
                column: "section_id");

            migrationBuilder.CreateIndex(
                name: "IX_section_board_id",
                table: "section",
                column: "board_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_owner_id",
                table: "team",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_account_email",
                table: "user_account",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_team_team_id",
                table: "user_team",
                column: "team_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_team_user_id",
                table: "user_team",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "card");

            migrationBuilder.DropTable(
                name: "user_team");

            migrationBuilder.DropTable(
                name: "section");

            migrationBuilder.DropTable(
                name: "board");

            migrationBuilder.DropTable(
                name: "team");

            migrationBuilder.DropTable(
                name: "user_account");
        }
    }
}
