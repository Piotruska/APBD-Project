using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevenueRecodnition.Migrations
{
    /// <inheritdoc />
    public partial class ProjectMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "APBDProject");

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "APBDProject",
                columns: table => new
                {
                    IdClient = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.IdClient);
                });

            migrationBuilder.CreateTable(
                name: "Discount",
                schema: "APBDProject",
                columns: table => new
                {
                    IdDiscount = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.IdDiscount);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "APBDProject",
                columns: table => new
                {
                    IdProduct = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CurrentVersion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BasePrice = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.IdProduct);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "APBDProject",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "CompanyClient",
                schema: "APBDProject",
                columns: table => new
                {
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    ComapnyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    KRS = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClient", x => x.IdClient);
                    table.ForeignKey(
                        name: "FK_CompanyClient_Client_IdClient",
                        column: x => x.IdClient,
                        principalSchema: "APBDProject",
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IndividualClient",
                schema: "APBDProject",
                columns: table => new
                {
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PESEL = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualClient", x => x.IdClient);
                    table.ForeignKey(
                        name: "FK_IndividualClient_Client_IdClient",
                        column: x => x.IdClient,
                        principalSchema: "APBDProject",
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contract",
                schema: "APBDProject",
                columns: table => new
                {
                    IdContract = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDatePayement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDatePayement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateContract = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateContract = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDateSupport = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateSupport = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    IsSigned = table.Column<bool>(type: "bit", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdProduct = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.IdContract);
                    table.ForeignKey(
                        name: "FK_Contract_Client_IdClient",
                        column: x => x.IdClient,
                        principalSchema: "APBDProject",
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Contract_Product_IdProduct",
                        column: x => x.IdProduct,
                        principalSchema: "APBDProject",
                        principalTable: "Product",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                schema: "APBDProject",
                columns: table => new
                {
                    IdSubscription = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RenewalPeriod = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    StartDateRenewalPayement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateRenewalPayement = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    IdProduct = table.Column<int>(type: "int", nullable: false),
                    Canceled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.IdSubscription);
                    table.ForeignKey(
                        name: "FK_Subscription_Client_IdClient",
                        column: x => x.IdClient,
                        principalSchema: "APBDProject",
                        principalTable: "Client",
                        principalColumn: "IdClient",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscription_Product_IdProduct",
                        column: x => x.IdProduct,
                        principalSchema: "APBDProject",
                        principalTable: "Product",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                schema: "APBDProject",
                columns: table => new
                {
                    IdPayement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdContract = table.Column<int>(type: "int", nullable: true),
                    IdSubscription = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    DatePayed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.IdPayement);
                    table.ForeignKey(
                        name: "FK_Payment_Contract_IdContract",
                        column: x => x.IdContract,
                        principalSchema: "APBDProject",
                        principalTable: "Contract",
                        principalColumn: "IdContract",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payment_Subscription_IdSubscription",
                        column: x => x.IdSubscription,
                        principalSchema: "APBDProject",
                        principalTable: "Subscription",
                        principalColumn: "IdSubscription",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_IdClient",
                schema: "APBDProject",
                table: "Contract",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_IdProduct",
                schema: "APBDProject",
                table: "Contract",
                column: "IdProduct");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_IdContract",
                schema: "APBDProject",
                table: "Payment",
                column: "IdContract");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_IdSubscription",
                schema: "APBDProject",
                table: "Payment",
                column: "IdSubscription");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IdClient",
                schema: "APBDProject",
                table: "Subscription",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Subscription_IdProduct",
                schema: "APBDProject",
                table: "Subscription",
                column: "IdProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyClient",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "Discount",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "IndividualClient",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "Payment",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "User",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "Contract",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "Subscription",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "APBDProject");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "APBDProject");
        }
    }
}
