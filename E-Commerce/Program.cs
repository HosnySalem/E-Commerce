using E_Commerce.Configurations;
using E_Commerce.Data;
using E_Commerce.Helpers;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebApplication1.Repository;

namespace E_Commerce
{
    public class Program
    {
        public static async System.Threading.Tasks.Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(corsOptions =>
            {
                corsOptions.AddPolicy("Policy1", corsOptPolicy =>
                {
                    corsOptPolicy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });


            builder.Services.AddControllers();


            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("MyConn"),
            b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
            builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(op =>
           op.DefaultAuthenticateScheme = "myschema")
           .AddJwtBearer("myschema", options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("Welcome to the accoumt of hosny mohamed salem")),
                   ValidateIssuer = false,
                   ValidateAudience = false,
               };
           });

            //builder.Services.AddAuthentication(op =>
            //{
            //    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = "Issuer",
            //        ValidAudience = "Audience",
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey"))
            //    };
            //});

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<ICartRepository, CartRepository>();


            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDbContext>();
                var userManager = services.GetRequiredService<UserManager<AppUser>>();

                try
                {
                    await dbContext.Database.MigrateAsync();
                    await IdentitySeed.SeedUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred while seeding the database: " + ex.Message);
                }
            }
            //builder.Services.Configure<FormOptions>(o =>
            //{
            //    o.ValueLengthLimit = int.MaxValue;
            //    o.MultipartBodyLengthLimit = int.MaxValue;
            //    o.MemoryBufferThreshold = int.MaxValue;
            //});

            app.UseHttpsRedirection();
            app.UseCors("Policy1");
            app.UseStaticFiles(); 

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
            //    RequestPath = new PathString("/Resources")
            //});

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
