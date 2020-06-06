using Microsoft.EntityFrameworkCore.Migrations;

namespace ParkyAPI.Migrations
{
    public partial class SeedDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT INTO NationalParks 
            VALUES 
            ('Test Park 1','NY','20200101 10:30 AM', GETDATE()),
            ('Test Park 2', 'LA', '20001210 10:57:57 PM', GETDATE()),
            ('Test Park 3','DC', '20100705', GETDATE())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
