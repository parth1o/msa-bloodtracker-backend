using Microsoft.EntityFrameworkCore.Migrations;

namespace msa_bloodtracker.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    PatientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.PatientId);
                });

            migrationBuilder.CreateTable(
                name: "Bloodtests",
                columns: table => new
                {
                    BloodtestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hb = table.Column<int>(type: "int", nullable: false),
                    Platelets = table.Column<int>(type: "int", nullable: false),
                    WBC = table.Column<float>(type: "real", nullable: false),
                    Neuts = table.Column<float>(type: "real", nullable: false),
                    Creatinine = table.Column<int>(type: "int", nullable: false),
                    Mg = table.Column<float>(type: "real", nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloodtests", x => x.BloodtestId);
                    table.ForeignKey(
                        name: "FK_Bloodtests_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "PatientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bloodtests_PatientId",
                table: "Bloodtests",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bloodtests");

            migrationBuilder.DropTable(
                name: "Patients");
        }
    }
}
