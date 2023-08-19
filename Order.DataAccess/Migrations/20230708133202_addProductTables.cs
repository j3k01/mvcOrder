using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Order.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addProductTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LongDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    MeasureUnit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetPrice = table.Column<double>(type: "float", nullable: false),
                    GrossPrice = table.Column<double>(type: "float", nullable: false),
                    ActiveState = table.Column<bool>(type: "bit", nullable: false),
                    VATRate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ActiveState", "GrossPrice", "LongDescription", "MeasureUnit", "Name", "NetPrice", "ShortDescription", "Symbol", "VATRate" },
                values: new object[,]
                {
                    { 1, true, 3.0, "Awesome, delicious, almost perfect fruit", "Kilograms", "Apple", 2.0, "Red fruit", "Symbol", 5 },
                    { 2, true, 4.0, "Yellow fruit, monkeys love them", "Kilograms", "Banana", 3.0, "Yellow fruit", "Symbol", 8 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
