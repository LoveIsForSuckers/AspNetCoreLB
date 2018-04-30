using AspNetCoreSolution.Models.Api;
using AspNetCoreSolution.Models.Api.Library;
using AspNetCoreSolution.Models.Api.UserGame;
using AspNetCoreSolution.Models.IdentityModels;
using AspNetCoreSolution.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDbGenericRepository;
using System;
using System.Security.Claims;
using System.Text;

namespace AspNetCoreSolution
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IMongoDbContext, MongoDbContext>(CreateMongoContext);

            ConfigureRazor(services);

            var mongoDBContext = CreateMongoContext();
            ConfigureIdentity(services, mongoDBContext);
            ConfigureJwt(services);
            ConfigureRepositories(services);

            services.AddMvc();
        }

        private void ConfigureRazor(IServiceCollection services)
        {
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationFormats.Add("/Views/Library/{1}/{0}" + RazorViewEngine.ViewExtension);
            });
        }

        private MongoDbContext CreateMongoContext(IServiceProvider arg = null)
        {
            return new MongoDbContext(Configuration.GetConnectionString("DefaultConnection"), Configuration["DatabaseNames:Default"]);
        }

        private void ConfigureIdentity(IServiceCollection services, IMongoDbContext mongoDBContext)
        {
            var identityBuilder = services.AddIdentity<ApplicationUser, ApplicationRole>();
            identityBuilder.AddMongoDbStores<ApplicationUser, ApplicationRole, int>(mongoDBContext);
            identityBuilder.AddDefaultTokenProviders();

            var identityOptions = Configuration.GetSection("IdentityOptions");
            var passwordOptions = identityOptions.GetSection("PasswordOptions");

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = passwordOptions.GetValue("RequireDigit", false);
                options.Password.RequiredLength = passwordOptions.GetValue("RequiredLength", 0);
                options.Password.RequiredUniqueChars = passwordOptions.GetValue("RequiredUniqueChars", 0);
                options.Password.RequireLowercase = passwordOptions.GetValue("RequireLowercase", false);
                options.Password.RequireNonAlphanumeric = passwordOptions.GetValue("RequireNonAlphanumeric", false);
                options.Password.RequireUppercase = passwordOptions.GetValue("RequireUppercase", false);
            });
        }

        private void ConfigureJwt(IServiceCollection services)
        {
            var jwtOptions = Configuration.GetSection("JwtOptions");
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions["SigningKey"]));

            services.Configure<JwtOptions>(options =>
            {
                options.Audience = jwtOptions["Audience"];
                options.Subject = jwtOptions["Subject"];
                options.Issuer = jwtOptions["Issuer"];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            var authBuilder = services.AddAuthentication();
            authBuilder.AddCookie(options => options.SlidingExpiration = true);
            authBuilder.AddJwtBearer(options =>
            {
                options.ClaimsIssuer = jwtOptions["Issuer"];
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions["Issuer"],

                    ValidateAudience = true,
                    ValidAudience = jwtOptions["Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,

                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(CustomClaimTypes.CanUseApi, policy => policy.RequireClaim(CustomClaimTypes.CanUseApi));
                options.AddPolicy(CustomClaimTypes.CanGetEveryonesData, policy => policy.RequireClaim(CustomClaimTypes.CanGetEveryonesData));
                options.AddPolicy(CustomClaimTypes.OwnsUserGame, policy => policy.RequireClaim(CustomClaimTypes.OwnsUserGame));
                options.AddPolicy(CustomClaimTypes.CanEditLibrary, policy => policy.RequireClaim(CustomClaimTypes.CanEditLibrary));
            });
        }

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserGameRepository, UserGameRepository>();
            services.AddScoped<ILibraryRepository, LibraryRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
