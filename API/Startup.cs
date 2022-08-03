using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using Infrastructure.Repositries;
using System.Text.Json.Serialization;
using API.Mapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using API.Helpers;
using System.Security.Claims;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IInventoryRepository, InventoryRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services
                .AddDbContext<EcommerceContext>(
                x => x.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")
                    )
                );

            services.AddIdentity<User, IdentityRole>(
                         options =>
                         {
                             options.Password.RequiredLength = 8;
                             options.Password.RequireNonAlphanumeric = false;
                             options.Password.RequireUppercase = false;
                             options.Password.RequireLowercase = false;
                         }

                     ).AddEntityFrameworkStores<EcommerceContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "User";
                options.DefaultChallengeScheme = "User";
            })
                .AddJwtBearer("User", options =>
                 {


                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         IssuerSigningKey = TokenHelper.GenerateSecretKey(Configuration),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Supplier", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Supplier"));

                options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
            });
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            }
            );

            services.AddAutoMapper(typeof(MappingProfiles));
                              
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Assets")),
                RequestPath = "/Assets"
            });

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
