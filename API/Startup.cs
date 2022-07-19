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

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
