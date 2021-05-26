using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EasyCare.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EasyCare.Core.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        //public DatabaseContext()
        //{
        //}

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Device> Device { get; set; }

        public DbSet<Senior> Senior { get; set; }

        public DbSet<SensorMessage> SensorMessage { get; set; }

        public DbSet<SignedUpUsers> SignedUpUsers { get; set; }
        
        public DbSet<Supervisor> Supervisor { get; set; }

        public DbSet<TAN> TAN { get; set; }

        public DbSet<CalendarEvent> CalendarEvent { get; set; }

        public DbSet<Message> Message { get; set; }

        public DbSet<NotificationMessage> NotificationMessage { get; set; }

        public DbSet<Drugs> Drugs { get; set; }

        public DbSet<Group> Group { get; set; }

        public DbSet<Participant> Participant { get; set; }

        public DbSet<CalendarSupervisors> CalendarSupervisors { get; set; }

        public DbSet<CalendarEventSchedulers> CalendarEventSchedulers { get; set; }

        public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
        {
            var searchName = typeof(TEntity).Name;
            var searchSet = GetType()
                .GetProperties()
                .SingleOrDefault(x => x.Name == searchName);
            var searchValue = searchSet.GetValue(this) as DbSet<TEntity>;

            return searchValue;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<CustomDrugs>().HasNoKey().ToView(null);
            modelBuilder.Entity<CustomSupervisor>().HasNoKey().ToView(null);
            modelBuilder.Entity<CustomCalendarEventDate>().HasNoKey().ToView(null);
            modelBuilder.Entity<CustomEventScheduler>().HasNoKey().ToView(null);

            base.OnModelCreating(modelBuilder);
        }
    }
}