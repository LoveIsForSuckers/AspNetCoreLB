using AspNetCoreSolution.Models.Api.UserGame;
using AspNetCoreSolution.Models.IdentityModels;
using AspNetCoreSolution.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbGenericRepository;
using System;

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

            var mongoDBContext = CreateMongoContext();
            ConfigureIdentity(services, mongoDBContext);
            ConfigureRepositories(services);

            services.AddMvc();
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

        private void ConfigureRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserGameRepository, UserGameRepository>();
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
