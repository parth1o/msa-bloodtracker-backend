using Microsoft.EntityFrameworkCore.Migrations;

namespace msa_bloodtracker.Migrations
{
    public partial class changedemailtourl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Patients",
                newName: "GithubURL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GithubURL",
                table: "Patients",
                newName: "Email");
        }
    }
}
