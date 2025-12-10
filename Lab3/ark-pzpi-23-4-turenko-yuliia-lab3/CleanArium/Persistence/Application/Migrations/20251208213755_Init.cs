using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Application.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "CleanArium");

            migrationBuilder.CreateTable(
                name: "SystemSettings",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaxAquariumsPerUser = table.Column<int>(type: "int", nullable: false),
                    MaxDevicesPerAquarium = table.Column<int>(type: "int", nullable: false),
                    MaxAlarmRulesPerDevice = table.Column<int>(type: "int", nullable: false),
                    MaxScheduledCommandsPerDevice = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Aquariums",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aquariums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Aquariums_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CleanArium",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CleanArium",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CleanArium",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupportMessages",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ReplyToMessageId = table.Column<long>(type: "bigint", nullable: true),
                    Sender = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    MessageStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportMessages_SupportMessages_ReplyToMessageId",
                        column: x => x.ReplyToMessageId,
                        principalSchema: "CleanArium",
                        principalTable: "SupportMessages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupportMessages_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CleanArium",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AquariumId = table.Column<long>(type: "bigint", nullable: false),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    DeviceStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Aquariums_AquariumId",
                        column: x => x.AquariumId,
                        principalSchema: "CleanArium",
                        principalTable: "Aquariums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlarmRules",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Condition = table.Column<int>(type: "int", nullable: false),
                    Threshold = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlarmRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlarmRules_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "CleanArium",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutedCommands",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CommandType = table.Column<int>(type: "int", nullable: false),
                    CommandStatus = table.Column<int>(type: "int", nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutedCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutedCommands_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "CleanArium",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduledCommands",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    CommandType = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RepeatMode = table.Column<int>(type: "int", nullable: false),
                    IntervalMinutes = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledCommands", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScheduledCommands_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "CleanArium",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorData",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeviceId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorData_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "CleanArium",
                        principalTable: "Devices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "CleanArium",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    AlarmRuleId = table.Column<long>(type: "bigint", nullable: true),
                    ScheduledCommandId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AlarmRules_AlarmRuleId",
                        column: x => x.AlarmRuleId,
                        principalSchema: "CleanArium",
                        principalTable: "AlarmRules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_ScheduledCommands_ScheduledCommandId",
                        column: x => x.ScheduledCommandId,
                        principalSchema: "CleanArium",
                        principalTable: "ScheduledCommands",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "CleanArium",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlarmRules_DeviceId",
                schema: "CleanArium",
                table: "AlarmRules",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Aquariums_UserId",
                schema: "CleanArium",
                table: "Aquariums",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_AquariumId",
                schema: "CleanArium",
                table: "Devices",
                column: "AquariumId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutedCommands_DeviceId",
                schema: "CleanArium",
                table: "ExecutedCommands",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_AlarmRuleId",
                schema: "CleanArium",
                table: "Notifications",
                column: "AlarmRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ScheduledCommandId",
                schema: "CleanArium",
                table: "Notifications",
                column: "ScheduledCommandId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "CleanArium",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_UserId",
                schema: "CleanArium",
                table: "PasswordResetTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "CleanArium",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduledCommands_DeviceId",
                schema: "CleanArium",
                table: "ScheduledCommands",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorData_DeviceId",
                schema: "CleanArium",
                table: "SensorData",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_ReplyToMessageId",
                schema: "CleanArium",
                table: "SupportMessages",
                column: "ReplyToMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportMessages_UserId",
                schema: "CleanArium",
                table: "SupportMessages",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutedCommands",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "RefreshTokens",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "SensorData",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "SupportMessages",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "SystemSettings",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "AlarmRules",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "ScheduledCommands",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "Aquariums",
                schema: "CleanArium");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "CleanArium");
        }
    }
}
