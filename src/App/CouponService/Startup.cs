using CouponService.EF;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CouponService
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
            var connString = Configuration.GetConnectionString("Mssql");
            //var ConnName = Configuration.GetSection("ConnectionStringName").Value;
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connString));

            services.AddScoped<ICouponRepository, CouponRepository>();

            //CORS
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("Development",
            //        builder => builder.WithOrigins("https://localhost:44360", "http://localhost:3356", "http://localhost:3357", "http://localhost:44302",
            //                                       "https://portal.ceptetamir.com", "https://ceptetamir.com", "https://www.ceptetamir.com",
            //                                       "https://panel.ceptekoruma.com", "https://ceptekoruma.com", "https://www.ceptekoruma.com")
            //                            .AllowAnyHeader()
            //                            .AllowAnyMethod());

            //    options.AddPolicy("Production",
            //        builder => builder.WithOrigins("https://portal.ceptetamir.com","https://ceptetamir.com", "https://www.ceptetamir.com",
            //                                       "https://panel.ceptekoruma.com", "https://ceptekoruma.com", "https://www.ceptekoruma.com",
            //                                       "https://portalceptetamir-stageportalceptetamir.azurewebsites.net",
            //                                       "http://portalceptetamir-stageportalceptetamir.azurewebsites.net")
            //                            .AllowAnyHeader()
            //                            .AllowAnyMethod());
            //});


            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            if (env.IsDevelopment())
            {
                app.UseCors("Development");
            }
            else
            {
                app.UseCors("Production");
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
