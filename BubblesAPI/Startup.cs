using System.Text;
using BubblesAPI.Authentication;
using BubblesAPI.Database;
using BubblesAPI.Database.Repository;
using BubblesAPI.Database.Repository.Implementation;
using BubblesAPI.DTOs;
using BubblesAPI.Services;
using BubblesAPI.Services.Implementation;
using BubblesAPI.Validators;
using BubblesEngine.Controllers;
using BubblesEngine.Controllers.Implementation;
using BubblesEngine.Engines;
using BubblesEngine.Engines.Implementations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace BubblesAPI
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
            services.AddMvcCore()
                .AddAuthorization();
            services.AddControllers(options => options.EnableEndpointRouting = false)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddDbContext<BubblesContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("db")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<IDbController, DbController>();
            services.AddTransient<IDomainFs, DomainFs>();
            services.AddTransient<IFileWrapper, FileWrapper>();

            services.AddTransient<IAuthentication, Authentication.Authentication>();
            services.AddTransient<IBubblesRepository, BubblesRepository>();

            services.AddTransient<IValidator<RegisterUserRequest>, RegisterUserRequestValidator>();
            services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IDbService, DbService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()){
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}