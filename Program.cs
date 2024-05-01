using AspMVC.Data;
using AspMVC.Models;
using AspMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;

namespace AspMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddOptions();
            var mailsetting = builder.Configuration.GetSection("MailSettings");
            builder.Services.Configure<MailSettings>(mailsetting);
            builder.Services.AddSingleton<IEmailSender, SendMailService>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnectionStrings"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

            builder.Services.Configure<RazorViewEngineOptions>(options =>
            {
                // /View/Controller/Action.cshtml
                // /MyView/Controller/Action.cshtml

                // {0} -> ten Action
                // {1} -> ten Controller
                // {2} -> ten Area
                //options.ViewLocationFormats.Add("/MyView/{1}/{0}" + RazorViewEngine.ViewExtension);

                //options.AreaViewLocationFormats.Add("/MyAreas/{2}/Views/{1}/{0}.cshtml");

            });

            // Truy cập IdentityOptions
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Thiết lập về Password
                options.Password.RequireDigit = false; // Không bắt phải có số
                options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
                options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
                options.Password.RequireUppercase = false; // Không bắt buộc chữ in
                options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
                options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt

                // Cấu hình Lockout - khóa user
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Khóa 5 phút
                options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 3 lầ thì khóa
                options.Lockout.AllowedForNewUsers = true;

                // Cấu hình về User.
                options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;  // Email là duy nhất


                // Cấu hình đăng nhập.
                options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
                options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                options.SignIn.RequireConfirmedAccount = true;

            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login/";
                options.LogoutPath = "/logout/";
                options.AccessDeniedPath = "/khongduoctruycap.html";
            });

            builder.Services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        var gconfig = builder.Configuration.GetSection("Authentication:Google");
                        options.ClientId = gconfig["ClientId"];
                        options.ClientSecret = gconfig["ClientSecret"];
                        // https://localhost:5001/signin-google
                        options.CallbackPath = "/dang-nhap-tu-google";
                    })
                    .AddFacebook(options =>
                    {
                        var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
                        options.AppId = fconfig["AppId"];
                        options.AppSecret = fconfig["AppSecret"];
                        options.CallbackPath = "/dang-nhap-tu-facebook";
                    })
                    // .AddTwitter()
                    // .AddMicrosoftAccount()
                    ;

            //builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

            builder.Services.AddAuthorization(options =>
            {

                options.AddPolicy("AllowEditRole", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireClaim("canedit", "user");
                    policyBuilder.RequireRole(RoleName.Administrator);

                });

                //options.AddPolicy("InGenZ", policyBuilder => {
                //    policyBuilder.RequireAuthenticatedUser();
                //    // policyBuilder.RequireClaim("canedit", "user");
                //    policyBuilder.Requirements.Add(new GenZRequirement()); // GenZRequirement

                //    // new GenZRequirement() -> Authorization handler

                //});

                options.AddPolicy("ShowAdminMenu", pb =>
                {
                    pb.RequireRole("Admin");
                });

                //options.AddPolicy("CanUpdateArticle", builder => {
                //    builder.Requirements.Add(new ArticleUpdateRequirement());
                //});

            });
            builder.Services.AddTransient<IEmailSender, SendMailService>();
            //builder.Services.AddTransient<SignInManager<AppUser>>();
            //builder.Services.AddTransient<UserManager<AppUser>>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //them dinh dang apk cho upload file
            app.UseHttpsRedirection();
            // Set up custom content types - associating file extension to MIME type
            var provider = new FileExtensionContentTypeProvider();
            // Add new mappings
            provider.Mappings[".myapp"] = "application/x-msdownload";
            provider.Mappings[".htm3"] = "text/html";
            provider.Mappings[".image"] = "image/png";
            provider.Mappings[".image"] = "image/png";
            provider.Mappings[".apk"] = "application/vnd.android.package-archive";

            app.UseStaticFiles(new StaticFileOptions
            {
                ContentTypeProvider = provider
            });


            app.UseRouting();

            app.UseAuthentication(); //xac dinh danh tinh
            app.UseAuthorization(); // xac dinh quyen truy cap

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "MyAreas",
                  pattern: "{area:exists}/{controller=Contacts}/{action=Index}/{id?}"
                );
            });
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
