using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Coach.Migrations
{
    public partial class InitialContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    BannerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerIsActive = table.Column<bool>(type: "bit", nullable: true),
                    BannerOrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.BannerId);
                });

            migrationBuilder.CreateTable(
                name: "CampTargets",
                columns: table => new
                {
                    CampTargetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampTargetTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampTargetTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampTargets", x => x.CampTargetId);
                });

            migrationBuilder.CreateTable(
                name: "CampTypes",
                columns: table => new
                {
                    CampTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampTypeTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampTypeTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampTypes", x => x.CampTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Configurations",
                columns: table => new
                {
                    ConfigurationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WhatsApp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Instgram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configurations", x => x.ConfigurationId);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Msg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryIsActive = table.Column<bool>(type: "bit", nullable: true),
                    CountryOrderIndex = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "CourseTargets",
                columns: table => new
                {
                    CourseTargetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseTargetTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseTargetTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTargets", x => x.CourseTargetId);
                });

            migrationBuilder.CreateTable(
                name: "EntityTypes",
                columns: table => new
                {
                    EntityTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityTypeTlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeTlen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntityTypes", x => x.EntityTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                columns: table => new
                {
                    Faqid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuestionEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnswerEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Faqid);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    GenderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GenderTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.GenderId);
                });

            migrationBuilder.CreateTable(
                name: "Newsletters",
                columns: table => new
                {
                    NewsletterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newsletters", x => x.NewsletterId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                });

            migrationBuilder.CreateTable(
                name: "PageContents",
                columns: table => new
                {
                    PageContentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageContents", x => x.PageContentId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethodTlar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethodTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.PaymentMethodId);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionOrderIndex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    SectionTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTargets",
                columns: table => new
                {
                    TournamentTargetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentTargetTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentTargetTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTargets", x => x.TournamentTargetId);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTypes",
                columns: table => new
                {
                    TournamentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentTypeTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentTypeTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTypes", x => x.TournamentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "CampPlans",
                columns: table => new
                {
                    CampPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DurationInMonth = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampPlans", x => x.CampPlanId);
                    table.ForeignKey(
                        name: "FK_CampPlans_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicDevices",
                columns: table => new
                {
                    PublicDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAndroiodDevice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicDevices", x => x.PublicDeviceId);
                    table.ForeignKey(
                        name: "FK_PublicDevices_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentPlans",
                columns: table => new
                {
                    TournamentPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DurationInMonth = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentPlans", x => x.TournamentPlanId);
                    table.ForeignKey(
                        name: "FK_TournamentPlans_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrainerPlans",
                columns: table => new
                {
                    TrainerPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    DurationInMonth = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerPlans", x => x.TrainerPlanId);
                    table.ForeignKey(
                        name: "FK_TrainerPlans_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Adzs",
                columns: table => new
                {
                    AdzId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdzPic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdzIsActive = table.Column<bool>(type: "bit", nullable: true),
                    AdzOrderIndex = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adzs", x => x.AdzId);
                    table.ForeignKey(
                        name: "FK_Adzs_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Adzs_EntityTypes_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "EntityTypes",
                        principalColumn: "EntityTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotifications",
                columns: table => new
                {
                    PublicNotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EntityTypeId = table.Column<int>(type: "int", nullable: true),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotifications", x => x.PublicNotificationId);
                    table.ForeignKey(
                        name: "FK_PublicNotifications_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicNotifications_EntityTypes_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "EntityTypes",
                        principalColumn: "EntityTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntityTypeId = table.Column<int>(type: "int", nullable: false),
                    EntityId = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.SubscriptionId);
                    table.ForeignKey(
                        name: "FK_Subscriptions_EntityTypes_EntityTypeId",
                        column: x => x.EntityTypeId,
                        principalTable: "EntityTypes",
                        principalColumn: "EntityTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trainers",
                columns: table => new
                {
                    TrainerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tele = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainers", x => x.TrainerId);
                    table.ForeignKey(
                        name: "FK_Trainers_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trainers_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "GenderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trainers_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Camps",
                columns: table => new
                {
                    CampId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampDescAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampDescEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CampTypeId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CampTargetId = table.Column<int>(type: "int", nullable: false),
                    CampPlanId = table.Column<int>(type: "int", nullable: false),
                    SubStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubPrice = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Camps", x => x.CampId);
                    table.ForeignKey(
                        name: "FK_Camps_CampPlans_CampPlanId",
                        column: x => x.CampPlanId,
                        principalTable: "CampPlans",
                        principalColumn: "CampPlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Camps_CampTargets_CampTargetId",
                        column: x => x.CampTargetId,
                        principalTable: "CampTargets",
                        principalColumn: "CampTargetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Camps_CampTypes_CampTypeId",
                        column: x => x.CampTypeId,
                        principalTable: "CampTypes",
                        principalColumn: "CampTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Camps_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentDescAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentDescEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TournamentTypeId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    TournamentTargetId = table.Column<int>(type: "int", nullable: false),
                    TournamentPlanId = table.Column<int>(type: "int", nullable: false),
                    SubStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SubPrice = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.TournamentId);
                    table.ForeignKey(
                        name: "FK_Tournaments_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournaments_TournamentPlans_TournamentPlanId",
                        column: x => x.TournamentPlanId,
                        principalTable: "TournamentPlans",
                        principalColumn: "TournamentPlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournaments_TournamentTargets_TournamentTargetId",
                        column: x => x.TournamentTargetId,
                        principalTable: "TournamentTargets",
                        principalColumn: "TournamentTargetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tournaments_TournamentTypes_TournamentTypeId",
                        column: x => x.TournamentTypeId,
                        principalTable: "TournamentTypes",
                        principalColumn: "TournamentTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicNotificationDevices",
                columns: table => new
                {
                    PublicNotificationDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicNotificationId = table.Column<int>(type: "int", nullable: false),
                    PublicDeviceId = table.Column<int>(type: "int", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicNotificationDevices", x => x.PublicNotificationDeviceId);
                    table.ForeignKey(
                        name: "FK_PublicNotificationDevices_PublicDevices_PublicDeviceId",
                        column: x => x.PublicDeviceId,
                        principalTable: "PublicDevices",
                        principalColumn: "PublicDeviceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PublicNotificationDevices_PublicNotifications_PublicNotificationId",
                        column: x => x.PublicNotificationId,
                        principalTable: "PublicNotifications",
                        principalColumn: "PublicNotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseTlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseTlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    CourseTargetId = table.Column<int>(type: "int", nullable: false),
                    CourseDescAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseDescEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_CourseTargets_CourseTargetId",
                        column: x => x.CourseTargetId,
                        principalTable: "CourseTargets",
                        principalColumn: "CourseTargetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerDevices",
                columns: table => new
                {
                    TrainerDeviceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAndroiodDevice = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerDevices", x => x.TrainerDeviceId);
                    table.ForeignKey(
                        name: "FK_TrainerDevices_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerImages",
                columns: table => new
                {
                    TrainerImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerImages", x => x.TrainerImageId);
                    table.ForeignKey(
                        name: "FK_TrainerImages_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainerSubscriptions",
                columns: table => new
                {
                    TrainerSubscriptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    TrainerPlanId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainerSubscriptions", x => x.TrainerSubscriptionId);
                    table.ForeignKey(
                        name: "FK_TrainerSubscriptions_TrainerPlans_TrainerPlanId",
                        column: x => x.TrainerPlanId,
                        principalTable: "TrainerPlans",
                        principalColumn: "TrainerPlanId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainerSubscriptions_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampImages",
                columns: table => new
                {
                    CampImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CampId = table.Column<int>(type: "int", nullable: false),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampImages", x => x.CampImageId);
                    table.ForeignKey(
                        name: "FK_CampImages_Camps_CampId",
                        column: x => x.CampId,
                        principalTable: "Camps",
                        principalColumn: "CampId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentImages",
                columns: table => new
                {
                    TournamentImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentImages", x => x.TournamentImageId);
                    table.ForeignKey(
                        name: "FK_TournamentImages_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "TournamentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseImages",
                columns: table => new
                {
                    CourseImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    Pic = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseImages", x => x.CourseImageId);
                    table.ForeignKey(
                        name: "FK_CourseImages_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adzs_CountryId",
                table: "Adzs",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Adzs_EntityTypeId",
                table: "Adzs",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CampImages_CampId",
                table: "CampImages",
                column: "CampId");

            migrationBuilder.CreateIndex(
                name: "IX_CampPlans_CountryId",
                table: "CampPlans",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Camps_CampPlanId",
                table: "Camps",
                column: "CampPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Camps_CampTargetId",
                table: "Camps",
                column: "CampTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Camps_CampTypeId",
                table: "Camps",
                column: "CampTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Camps_CountryId",
                table: "Camps",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseImages_CourseId",
                table: "CourseImages",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseTargetId",
                table: "Courses",
                column: "CourseTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TrainerId",
                table: "Courses",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicDevices_CountryId",
                table: "PublicDevices",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotificationDevices_PublicDeviceId",
                table: "PublicNotificationDevices",
                column: "PublicDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotificationDevices_PublicNotificationId",
                table: "PublicNotificationDevices",
                column: "PublicNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotifications_CountryId",
                table: "PublicNotifications",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicNotifications_EntityTypeId",
                table: "PublicNotifications",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_EntityTypeId",
                table: "Subscriptions",
                column: "EntityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PaymentMethodId",
                table: "Subscriptions",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentImages_TournamentId",
                table: "TournamentImages",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentPlans_CountryId",
                table: "TournamentPlans",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_CountryId",
                table: "Tournaments",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentPlanId",
                table: "Tournaments",
                column: "TournamentPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentTargetId",
                table: "Tournaments",
                column: "TournamentTargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentTypeId",
                table: "Tournaments",
                column: "TournamentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerDevices_TrainerId",
                table: "TrainerDevices",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerImages_TrainerId",
                table: "TrainerImages",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerPlans_CountryId",
                table: "TrainerPlans",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_CountryId",
                table: "Trainers",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_GenderId",
                table: "Trainers",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainers_SectionId",
                table: "Trainers",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSubscriptions_TrainerId",
                table: "TrainerSubscriptions",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainerSubscriptions_TrainerPlanId",
                table: "TrainerSubscriptions",
                column: "TrainerPlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adzs");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "CampImages");

            migrationBuilder.DropTable(
                name: "Configurations");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "CourseImages");

            migrationBuilder.DropTable(
                name: "Faqs");

            migrationBuilder.DropTable(
                name: "Newsletters");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PageContents");

            migrationBuilder.DropTable(
                name: "PublicNotificationDevices");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "TournamentImages");

            migrationBuilder.DropTable(
                name: "TrainerDevices");

            migrationBuilder.DropTable(
                name: "TrainerImages");

            migrationBuilder.DropTable(
                name: "TrainerSubscriptions");

            migrationBuilder.DropTable(
                name: "Camps");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "PublicDevices");

            migrationBuilder.DropTable(
                name: "PublicNotifications");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "TrainerPlans");

            migrationBuilder.DropTable(
                name: "CampPlans");

            migrationBuilder.DropTable(
                name: "CampTargets");

            migrationBuilder.DropTable(
                name: "CampTypes");

            migrationBuilder.DropTable(
                name: "CourseTargets");

            migrationBuilder.DropTable(
                name: "Trainers");

            migrationBuilder.DropTable(
                name: "EntityTypes");

            migrationBuilder.DropTable(
                name: "TournamentPlans");

            migrationBuilder.DropTable(
                name: "TournamentTargets");

            migrationBuilder.DropTable(
                name: "TournamentTypes");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
