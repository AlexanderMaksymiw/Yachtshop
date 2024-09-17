using Microsoft.EntityFrameworkCore.Migrations;
using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class ConvertedDraftLengthBeamToMeters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "LengthMetres",
                table: "Specifications",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DraftMetres",
                table: "Specifications",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BeamMetres",
                table: "Specifications",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.Sql(@"
                UPDATE Specifications
                SET 
                    BeamMetres = 
                        CASE 
                            -- Handle feet and inches (e.g., 32'10)
                            WHEN CHARINDEX('''', Beam) > 0 AND CHARINDEX(' in', Beam) = 0
                            THEN 
                                (
                                    TRY_CAST(LEFT(Beam, CHARINDEX('''', Beam) - 1) AS DECIMAL(10, 2)) * 12
                                    + TRY_CAST(SUBSTRING(Beam, CHARINDEX('''', Beam) + 1, LEN(Beam) - CHARINDEX('''', Beam)) AS DECIMAL(10, 2))
                                ) * 0.0254
                            -- Handle decimal inches (e.g., 372.000000)
                            WHEN CHARINDEX(' in', Beam) > 0
                            THEN
                                TRY_CAST(REPLACE(Beam, ' in', '') AS DECIMAL(10, 2)) * 0.0254
                            -- Handle Metres (e.g., 7.43 m)
                            WHEN CHARINDEX(' m', Beam) > 0
                            THEN
                                TRY_CAST(REPLACE(Beam, ' m', '') AS DECIMAL(10, 2))
                            -- Handle plain decimal numbers (assumed to be in inches)
                            WHEN ISNUMERIC(Beam) = 1
                            THEN 
                                TRY_CAST(Beam AS DECIMAL(10, 2)) * 0.0254
                            ELSE NULL
                        END,
                    LengthMetres = 
                        CASE 
                            -- Handle feet and inches (e.g., 32'10)
                            WHEN CHARINDEX('''', Length) > 0 AND CHARINDEX(' in', Length) = 0
                            THEN 
                                (
                                    TRY_CAST(LEFT(Length, CHARINDEX('''', Length) - 1) AS DECIMAL(10, 2)) * 12
                                    + TRY_CAST(SUBSTRING(Length, CHARINDEX('''', Length) + 1, LEN(Length) - CHARINDEX('''', Length)) AS DECIMAL(10, 2))
                                ) * 0.0254
                            -- Handle decimal inches (e.g., 372.000000)
                            WHEN CHARINDEX(' in', Length) > 0
                            THEN
                                TRY_CAST(REPLACE(Length, ' in', '') AS DECIMAL(10, 2)) * 0.0254
                            -- Handle Metres (e.g., 7.43 m)
                            WHEN CHARINDEX(' m', Length) > 0
                            THEN
                                TRY_CAST(REPLACE(Length, ' m', '') AS DECIMAL(10, 2))
                            -- Handle plain decimal numbers (assumed to be in inches)
                            WHEN ISNUMERIC(Length) = 1
                            THEN 
                                TRY_CAST(Length AS DECIMAL(10, 2)) * 0.0254
                            ELSE NULL
                        END,
                    DraftMetres = 
                        CASE 
                            -- Handle feet and inches (e.g., 32'10)
                            WHEN CHARINDEX('''', Draft) > 0 AND CHARINDEX(' in', Draft) = 0
                            THEN 
                                (
                                    TRY_CAST(LEFT(Draft, CHARINDEX('''', Draft) - 1) AS DECIMAL(10, 2)) * 12
                                    + TRY_CAST(SUBSTRING(Draft, CHARINDEX('''', Draft) + 1, LEN(Draft) - CHARINDEX('''', Draft)) AS DECIMAL(10, 2))
                                ) * 0.0254
                            -- Handle decimal inches (e.g., 372.000000)
                            WHEN CHARINDEX(' in', Draft) > 0
                            THEN
                                TRY_CAST(REPLACE(Draft, ' in', '') AS DECIMAL(10, 2)) * 0.0254
                            -- Handle Metres (e.g., 7.43 m)
                            WHEN CHARINDEX(' m', Draft) > 0
                            THEN
                                TRY_CAST(REPLACE(Draft, ' m', '') AS DECIMAL(10, 2))
                            -- Handle plain decimal numbers (assumed to be in inches)
                            WHEN ISNUMERIC(Draft) = 1
                            THEN 
                                TRY_CAST(Draft AS DECIMAL(10, 2)) * 0.0254
                            ELSE NULL
                        END;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "LengthMetres",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DraftMetres",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BeamMetres",
                table: "Specifications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
