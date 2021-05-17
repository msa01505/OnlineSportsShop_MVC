using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineSportShop.Areas.User.Services;
using Proj.Areas.Admin.Services;
using Proj.Areas.User.Services;
using Proj.Data;
using Proj.Models;

namespace Proj
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
            //for customer
            services.AddScoped<IProduct, ProductService>();
            services.AddScoped<ICartList, CartList_Service>();
            //for admin
            services.AddScoped<CRUDRepo<Product>, Product_Service>();


            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            //Admin Rules

            services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true
                   ).AddEntityFrameworkStores<ApplicationDbContext>()
                   .AddDefaultUI()
                   .AddDefaultTokenProviders()
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "734093631539-udnmdeo3s4cbf1fd8911fal0fk91v11k.apps.googleusercontent.com";
                options.ClientSecret = "3BsvvbO-PHoPFw09lb9obIje";
            });
            
            services.AddAuthentication().AddFacebook(options =>
            {
                options.ClientId = "763073141268805";
                options.ClientSecret = "7fcbc3a6081fdaea8ef828642a28509c";
            });

           
            services.AddAuthentication().AddGitHub(options =>
            {
                options.ClientId = "ef1de577033bb1807c5d";
                options.ClientSecret = "7a436c7291d2b41598d71dbc854b291fe7e31c70";
            });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                        name: "ProductsAdmin",
                        areaName: "Admin",
                        pattern: "Admin/{controller=Home}/{action=Index}/{id?}"

                        );
                endpoints.MapAreaControllerRoute(
                        name: "Products",
                        areaName: "User",
                        pattern: "User/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapDefaultControllerRoute();


                endpoints.MapRazorPages();
            });
        }
    }
}
