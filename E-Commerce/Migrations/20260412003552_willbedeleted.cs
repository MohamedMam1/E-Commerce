using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    /// <remarks>
    /// Originally this migration renamed Products.IsAvailable to IsDeleted and added IsDeleted to Categories.
    /// That is invalid: IsDeleted on Products already exists (see 20260409174448_softdeleteproduct) and
    /// IsDeleted on Categories already exists (see 20260409181323_softdeletecategory). Applying it caused:
    /// "Column name 'IsDeleted' in table 'Products' is specified more than once."
    /// The Up/Down methods are intentionally empty; schema is already correct from earlier migrations.
    /// </remarks>
    public partial class willbedeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
