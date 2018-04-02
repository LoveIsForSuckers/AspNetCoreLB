using AspNetCoreSolution.Models.IdentityModels;
using AspNetCoreSolution.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDbGenericRepository;

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

            var mongoDBContext = new MongoDbContext(Configuration.GetConnectionString("DefaultConnection"), Configuration["DatabaseNames:Default"]);

            ConfigureIdentity(services, mongoDBContext);

            services.AddMvc();
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
