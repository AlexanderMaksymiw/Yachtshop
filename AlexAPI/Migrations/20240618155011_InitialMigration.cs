using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlexAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    is2FA = table.Column<bool>(type: "bit", nullable: false),
                    secretKey2FA = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Autos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LengthMetres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LengthFeet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Beam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeamMetres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeamFeet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Draft = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftMetres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftFeet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearBuilt = table.Column<int>(type: "int", nullable: false),
                    YearRefit = table.Column<int>(type: "int", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalCrew = table.Column<int>(type: "int", nullable: false),
                    Builder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HullConstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HullConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Superstructure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestsSleeping = table.Column<int>(type: "int", nullable: false),
                    BedConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CabinConfig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cabins = table.Column<int>(type: "int", nullable: true),
                    Engines = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Toys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuelConsumption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FuelConsumptionUnits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CruisingSpeed = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brokers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CrewPhotosSwitch = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Details",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateWentPublic = table.Column<DateTime>(type: "datetime2", nullable: true),
                    YachtName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ListingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SailPower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearBuilt = table.Column<int>(type: "int", nullable: false),
                    Builder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherBuilder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YachtAdmin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YachtAdminEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YachtAdminPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cabins = table.Column<int>(type: "int", nullable: false),
                    SingleCabins = table.Column<int>(type: "int", nullable: false),
                    TwinCabins = table.Column<int>(type: "int", nullable: false),
                    DoubleCabins = table.Column<int>(type: "int", nullable: false),
                    TripleCabins = table.Column<int>(type: "int", nullable: true),
                    ConvertibleCabins = table.Column<int>(type: "int", nullable: true),
                    GuestsSleeping = table.Column<int>(type: "int", nullable: false),
                    GuestsCruising = table.Column<int>(type: "int", nullable: false),
                    FuelConsumption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CruisingSpeed = table.Column<int>(type: "int", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    TotalCrew = table.Column<int>(type: "int", nullable: false),
                    Captain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaptainsNationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Chef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefStewardess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChiefEngineer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MybaTippingPolicy = table.Column<int>(type: "int", nullable: false),
                    YearRefit = table.Column<int>(type: "int", nullable: true),
                    GrossTons = table.Column<int>(type: "int", nullable: true),
                    MasterCabinOnMainDeck = table.Column<int>(type: "int", nullable: false),
                    Range = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BulkBeds = table.Column<int>(type: "int", nullable: true),
                    Beds = table.Column<int>(type: "int", nullable: false),
                    KingBeds = table.Column<int>(type: "int", nullable: false),
                    QueenBeds = table.Column<int>(type: "int", nullable: false),
                    DoubleBeds = table.Column<int>(type: "int", nullable: false),
                    SingleBeds = table.Column<int>(type: "int", nullable: false),
                    PullmanBeds = table.Column<int>(type: "int", nullable: false),
                    Rig = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InteriorDesigner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NavalArchitect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HullConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HullConstruction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummerBasePort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WinterBasePort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistryPort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestsAccommodation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationDet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RateDet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SpecialConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contracts = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnginesGenerators = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefitDet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Toys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AvFacilities = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Communications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommercialStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrewProfiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrewModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Superstructure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndependentStakeholder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerCaOperated = table.Column<int>(type: "int", nullable: false),
                    OwnerCaptainOperated = table.Column<int>(type: "int", nullable: false),
                    LengthMetric = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeamMetric = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftMetric = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LengthImperial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeamImperial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftImperial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OperatingAreas = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonsUnavailable = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Details", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Galleries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Galleries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Generals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Available = table.Column<bool>(type: "bit", nullable: false),
                    DataSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GeneralDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperatingAreaItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Areas = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingAreaItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdFile = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    KingBeds = table.Column<int>(type: "int", nullable: false),
                    TwinCabins = table.Column<int>(type: "int", nullable: false),
                    YearBuilt = table.Column<int>(type: "int", nullable: false),
                    CruisingSpeed = table.Column<int>(type: "int", nullable: false),
                    GuestsSleeping = table.Column<int>(type: "int", nullable: false),
                    FuelConsumption = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearRefit = table.Column<int>(type: "int", nullable: false),
                    DoubleCabins = table.Column<int>(type: "int", nullable: false),
                    CaptainsNationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistryPort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoubleBeds = table.Column<int>(type: "int", nullable: false),
                    Beam = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeamMetres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    Builder = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SailPower = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SingleBeds = table.Column<int>(type: "int", nullable: false),
                    Length = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LengthMetres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QueenBeds = table.Column<int>(type: "int", nullable: false),
                    GuestsCruising = table.Column<int>(type: "int", nullable: false),
                    Cabins = table.Column<int>(type: "int", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PullmanBeds = table.Column<int>(type: "int", nullable: false),
                    GymEquipment = table.Column<bool>(type: "bit", nullable: false),
                    Draft = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftMetres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Toys = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrewProfiles = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VideoInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelTitle = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicenceRegistrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenceId = table.Column<int>(type: "int", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    YachtDetailGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenceRegistrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LicenceRegistrations_Details_YachtDetailGuid",
                        column: x => x.YachtDetailGuid,
                        principalTable: "Details",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "OperatingAreaNews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Areas = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YachtDetailGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingAreaNews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatingAreaNews_Details_YachtDetailGuid",
                        column: x => x.YachtDetailGuid,
                        principalTable: "Details",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "Rates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinRate = table.Column<int>(type: "int", nullable: true),
                    MaxRate = table.Column<int>(type: "int", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YachtDetailGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rates_Details_YachtDetailGuid",
                        column: x => x.YachtDetailGuid,
                        principalTable: "Details",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "SpecialRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    YachtDetailGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRequest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialRequest_Details_YachtDetailGuid",
                        column: x => x.YachtDetailGuid,
                        principalTable: "Details",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "GalleryItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false),
                    Filename = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GalleryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GalleryId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GalleryId2 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GalleryId3 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GalleryItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GalleryItems_Galleries_GalleryId",
                        column: x => x.GalleryId,
                        principalTable: "Galleries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GalleryItems_Galleries_GalleryId1",
                        column: x => x.GalleryId1,
                        principalTable: "Galleries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GalleryItems_Galleries_GalleryId2",
                        column: x => x.GalleryId2,
                        principalTable: "Galleries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GalleryItems_Galleries_GalleryId3",
                        column: x => x.GalleryId3,
                        principalTable: "Galleries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperatingAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _25Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatingAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OperatingAreas_OperatingAreaItems__25Id",
                        column: x => x._25Id,
                        principalTable: "OperatingAreaItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PriceTerms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinRate = table.Column<int>(type: "int", nullable: false),
                    MaxRate = table.Column<int>(type: "int", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Terms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceTerms_Prices_PricesId",
                        column: x => x.PricesId,
                        principalTable: "Prices",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoBrokerFriendly = table.Column<bool>(type: "bit", nullable: false),
                    VideoInfoId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_VideoInfos_VideoInfoId",
                        column: x => x.VideoInfoId,
                        principalTable: "VideoInfos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Brochures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AutoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BrokerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OperatingAreasId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CrewId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SpecificationsId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GalleriesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GeneralId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PricesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brochures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brochures_Autos_AutoId",
                        column: x => x.AutoId,
                        principalTable: "Autos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_Brokers_BrokerId",
                        column: x => x.BrokerId,
                        principalTable: "Brokers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_Crews_CrewId",
                        column: x => x.CrewId,
                        principalTable: "Crews",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_Galleries_GalleriesId",
                        column: x => x.GalleriesId,
                        principalTable: "Galleries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Brochures_Generals_GeneralId",
                        column: x => x.GeneralId,
                        principalTable: "Generals",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_OperatingAreas_OperatingAreasId",
                        column: x => x.OperatingAreasId,
                        principalTable: "OperatingAreas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_Prices_PricesId",
                        column: x => x.PricesId,
                        principalTable: "Prices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_Specifications_SpecificationsId",
                        column: x => x.SpecificationsId,
                        principalTable: "Specifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Brochures_Videos_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Videos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CrewMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tba = table.Column<bool>(type: "bit", nullable: false),
                    PhotoId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    YachtBrochureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CrewMembers_Brochures_YachtBrochureId",
                        column: x => x.YachtBrochureId,
                        principalTable: "Brochures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CrewMembers_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "KeyFeatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YachtBrochureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyFeatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyFeatures_Brochures_YachtBrochureId",
                        column: x => x.YachtBrochureId,
                        principalTable: "Brochures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Yachts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistryPort = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DetailGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BrochureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Yachts", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Yachts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Yachts_Brochures_BrochureId",
                        column: x => x.BrochureId,
                        principalTable: "Brochures",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Yachts_Details_DetailGuid",
                        column: x => x.DetailGuid,
                        principalTable: "Details",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_AutoId",
                table: "Brochures",
                column: "AutoId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_BrokerId",
                table: "Brochures",
                column: "BrokerId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_CrewId",
                table: "Brochures",
                column: "CrewId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_GalleriesId",
                table: "Brochures",
                column: "GalleriesId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_GeneralId",
                table: "Brochures",
                column: "GeneralId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_OperatingAreasId",
                table: "Brochures",
                column: "OperatingAreasId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_PricesId",
                table: "Brochures",
                column: "PricesId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_SpecificationsId",
                table: "Brochures",
                column: "SpecificationsId");

            migrationBuilder.CreateIndex(
                name: "IX_Brochures_VideoId",
                table: "Brochures",
                column: "VideoId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_PhotoId",
                table: "CrewMembers",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewMembers_YachtBrochureId",
                table: "CrewMembers",
                column: "YachtBrochureId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryItems_GalleryId",
                table: "GalleryItems",
                column: "GalleryId");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryItems_GalleryId1",
                table: "GalleryItems",
                column: "GalleryId1");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryItems_GalleryId2",
                table: "GalleryItems",
                column: "GalleryId2");

            migrationBuilder.CreateIndex(
                name: "IX_GalleryItems_GalleryId3",
                table: "GalleryItems",
                column: "GalleryId3");

            migrationBuilder.CreateIndex(
                name: "IX_KeyFeatures_YachtBrochureId",
                table: "KeyFeatures",
                column: "YachtBrochureId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenceRegistrations_YachtDetailGuid",
                table: "LicenceRegistrations",
                column: "YachtDetailGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OperatingAreaNews_YachtDetailGuid",
                table: "OperatingAreaNews",
                column: "YachtDetailGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OperatingAreas__25Id",
                table: "OperatingAreas",
                column: "_25Id");

            migrationBuilder.CreateIndex(
                name: "IX_PriceTerms_PricesId",
                table: "PriceTerms",
                column: "PricesId");

            migrationBuilder.CreateIndex(
                name: "IX_Rates_YachtDetailGuid",
                table: "Rates",
                column: "YachtDetailGuid");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRequest_YachtDetailGuid",
                table: "SpecialRequest",
                column: "YachtDetailGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_VideoInfoId",
                table: "Videos",
                column: "VideoInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_BrochureId",
                table: "Yachts",
                column: "BrochureId");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_DetailGuid",
                table: "Yachts",
                column: "DetailGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Yachts_UserId",
                table: "Yachts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CrewMembers");

            migrationBuilder.DropTable(
                name: "GalleryItems");

            migrationBuilder.DropTable(
                name: "KeyFeatures");

            migrationBuilder.DropTable(
                name: "LicenceRegistrations");

            migrationBuilder.DropTable(
                name: "OperatingAreaNews");

            migrationBuilder.DropTable(
                name: "PriceTerms");

            migrationBuilder.DropTable(
                name: "Rates");

            migrationBuilder.DropTable(
                name: "SpecialRequest");

            migrationBuilder.DropTable(
                name: "Yachts");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Brochures");

            migrationBuilder.DropTable(
                name: "Details");

            migrationBuilder.DropTable(
                name: "Autos");

            migrationBuilder.DropTable(
                name: "Brokers");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Galleries");

            migrationBuilder.DropTable(
                name: "Generals");

            migrationBuilder.DropTable(
                name: "OperatingAreas");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "OperatingAreaItems");

            migrationBuilder.DropTable(
                name: "VideoInfos");
        }
    }
}
