using AuthPermissions.AspNetCore.GetDataKeyCode;
using AuthPermissions.BaseCode.CommonCode;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Core.Entities;
using Core.Entities.Users;
using Infrastructure.Data.Config;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<Persona, Role, string>, IBaseEntity, IDataKeyFilterReadOnly
    {
        private readonly DbContextOptions _options;
        private readonly TenantIdentifier _tenantIdentifier;

        public AppDbContext(DbContextOptions<AppDbContext> options, IGetDataKeyFromUser dataKeyFilter, TenantIdentifier tenantIdentifier) : base(options)
        {
            _options = options;
            _tenantIdentifier = tenantIdentifier;

            // The DataKey is null when: no one is logged in, its a background service, 
            // or user hasn't got an assigned tenant.
            // In these cases its best to set the data key that doesn't match any possible DataKey 
            DataKey = dataKeyFilter?.DataKey ?? "stop any user without a DataKey to access the data";
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<ParentStudent> ParentStudent { get; set; }
        public DbSet<Busdriver> Busdrivers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Campus> Campuses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<QrCode> QrCodes { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripStudent> TripStudents { get; set; }


        public DbSet<CompanyTenant> Companies { get; set; }
        public bool IsDeleted { get; set; }

        /// <summary>
        /// The DataKey to be used for multi-tenant applications
        /// </summary>
        public string DataKey { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("MyStar");

            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            var entityTypes = builder.Model.GetEntityTypes();
            entityTypes.ToList().ForEach(entityType =>
            {
                if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            });


            builder.Entity<Parent>()
                    .HasMany(e => e.Students)
                    .WithMany(e => e.Parents)
                    .UsingEntity<ParentStudent>();

            builder.Entity<TripStudent>()
                .HasKey(ts => new { ts.TripId, ts.StudentId }); // Composite key

            builder.Entity<TripStudent>()
                .HasOne(ts => ts.Trip)
                .WithMany(t => t.TripStudents)
                .HasForeignKey(ts => ts.TripId);

            builder.Entity<TripStudent>()
                .HasOne(ts => ts.Student)
                .WithMany(s => s.TripStudents)
                .HasForeignKey(ts => ts.StudentId);

            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    // Check if the entity has a 'DataKey' property
            //    var dataKeyProperty = entityType.ClrType.GetProperty("DataKey");
            //    if (dataKeyProperty != null && dataKeyProperty.PropertyType == typeof(string))
            //    {
            //        // Use reflection to create a lambda expression dynamically
            //        var parameter = Expression.Parameter(entityType.ClrType, "e");
            //        var property = Expression.Property(parameter, dataKeyProperty.Name);
            //        var tenantId = Expression.Constant(_tenantIdentifier.CurrentTenantId, typeof(string));
            //        var equalsExpression = Expression.Equal(property, tenantId);
            //        var lambda = Expression.Lambda(equalsExpression, parameter);

            //        builder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            //    }
            //}




            //Multitenant Datakey
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSingleTenantReadWriteQueryFilter(this);
                }
                else
                {
                    throw new Exception(
                        $"You haven't added the {nameof(IDataKeyFilterReadWrite)} to the entity {entityType.ClrType.Name}");
                }

                foreach (var mutableProperty in entityType.GetProperties())
                {
                    if (mutableProperty.ClrType == typeof(decimal))
                    {
                        mutableProperty.SetPrecision(9);
                        mutableProperty.SetScale(2);
                    }
                }
            }

            //builder.Entity<Campus>().HasQueryFilter(x => x.DataKey == _tenantIdentifier.CurrentTenantId);

            // Apply global query filters to tenant-specific entities
            //foreach (var entityType in builder.Model.GetEntityTypes())
            //{
            //    if (typeof(IBaseEntity).IsAssignableFrom(entityType.ClrType)) // Ensure this interface is implemented by tenant-specific entities
            //    {
            //        builder.Entity(entityType.ClrType)
            //                    .HasQueryFilter(ConvertFilterExpression(entityType.ClrType, _tenantIdentifier.CurrentTenantId));
            //    }
            //}


        }

        private LambdaExpression ConvertFilterExpression(Type entityType, string tenantId)
        {
            var parameter = Expression.Parameter(entityType, "e");
            var property = Expression.Property(parameter, "Datakey");
            var constant = Expression.Constant(tenantId);
            var equal = Expression.Equal(property, constant);
            var lambda = Expression.Lambda(equal, parameter);
            return lambda;
        }


        public async Task<bool> TrySaveChangesAsync()
        {
            try
            {
                this.MarkWithDataKeyIfNeeded(DataKey);
                await SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            this.MarkWithDataKeyIfNeeded(DataKey);
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        public object Clone()
        {
            throw new NotImplementedException();
        }
    }

    //public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>, IGetDataKeyFromUser
    //{
    //    public string DataKey { get; set; } = "dd";


    //    public AppDbContext CreateDbContext(string[] args)
    //    {
    //        string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION") ?? string.Empty;
    //        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
    //        optionsBuilder.UseNpgsql(connectionString);

    //        //var context = new AppDbContext(optionsBuilder.Options, this);

    //        return new AppDbContext(optionsBuilder.Options, this);
    //    }
    //}


}
