using System;
using Coach.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace Coach.Data
{
    public partial class CoachContext : DbContext
    {
        public CoachContext()
        {
        }

        public CoachContext(DbContextOptions<CoachContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Adz> Adzs { get; set; }
        public virtual DbSet<Banner> Banners { get; set; }
        public virtual DbSet<Camp> Camps { get; set; }
        public virtual DbSet<CampImage> CampImages { get; set; }
        public virtual DbSet<CampPlan> CampPlans { get; set; }
        public virtual DbSet<CampTarget> CampTargets { get; set; }
        public virtual DbSet<CampType> CampTypes { get; set; }
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseImage> CourseImages { get; set; }
        public virtual DbSet<CourseTarget> CourseTargets { get; set; }
        public virtual DbSet<EntityType> EntityTypes { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Newsletter> Newsletters { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<PageContent> PageContents { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<PublicDevice> PublicDevices { get; set; }
        public virtual DbSet<PublicNotification> PublicNotifications { get; set; }
        public virtual DbSet<PublicNotificationDevice> PublicNotificationDevices { get; set; }
        public virtual DbSet<Section> Sections { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public virtual DbSet<Tournament> Tournaments { get; set; }
        public virtual DbSet<TournamentImage> TournamentImages { get; set; }
        public virtual DbSet<TournamentPlan> TournamentPlans { get; set; }
        public virtual DbSet<TournamentTarget> TournamentTargets { get; set; }
        public virtual DbSet<TournamentType> TournamentTypes { get; set; }
        public virtual DbSet<Trainer> Trainers { get; set; }
        public virtual DbSet<TrainerDevice> TrainerDevices { get; set; }
        public virtual DbSet<TrainerImage> TrainerImages { get; set; }
        public virtual DbSet<TrainerPlan> TrainerPlans { get; set; }
        public virtual DbSet<TrainerSubscription> TrainerSubscriptions { get; set; }
        //public virtual DbSet<paymenturl> Paymenturls { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PageContent>().HasData(new PageContent { PageContentId = 1, PageTitleAr = "من نحن", PageTitleEn = "About", ContentAr = "من نحن", ContentEn = "About Page" });
            modelBuilder.Entity<PageContent>().HasData(new PageContent { PageContentId = 2, PageTitleAr = "الشروط والاحكام", PageTitleEn = "Condition and Terms", ContentAr = "الشروط والاحكام", ContentEn = "Condition and Terms Page" });
            modelBuilder.Entity<PageContent>().HasData(new PageContent { PageContentId = 3, PageTitleAr = "سياسة الخصوصية", PageTitleEn = "Privacy Policy", ContentAr = "سياسة الخصوصية", ContentEn = "Privacy Policy Page" });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod { PaymentMethodId = 1, PaymentMethodTlar = "CASH", PaymentMethodTlEn = "CASH" });
            modelBuilder.Entity<PaymentMethod>().HasData(new PaymentMethod { PaymentMethodId = 2, PaymentMethodTlar = "KNET", PaymentMethodTlEn = "KNET" });
            modelBuilder.Entity<EntityType>().HasData(new EntityType { EntityTypeId = 1, EntityTypeTlar = "Trainer", EntityTypeTlen = "Trainer" });
            modelBuilder.Entity<EntityType>().HasData(new EntityType { EntityTypeId = 2, EntityTypeTlar = "Camp", EntityTypeTlen = "Camp" });
            modelBuilder.Entity<EntityType>().HasData(new EntityType { EntityTypeId = 3, EntityTypeTlar = "Tournment", EntityTypeTlen = "Tournment" });
            modelBuilder.Entity<EntityType>().HasData(new EntityType { EntityTypeId = 4, EntityTypeTlar = "Course", EntityTypeTlen = "Course" });
            modelBuilder.Entity<EntityType>().HasData(new EntityType { EntityTypeId = 5, EntityTypeTlar = "URL", EntityTypeTlen = "URL" });
            modelBuilder.Entity<Configuration>().HasData(new Configuration { ConfigurationId = 1, Facebook = "https://www.facebook.com/", Instgram = "https://www.insgram.com/", LinkedIn= "https://www.linkedin.com/", Twitter= "https://www.twitter.com/", WhatsApp= "+965241" });
        }
    }
}
