using AspNetCoreIdentityApp.Web.Extensions;
using AspNetCoreIdentityApp.Web.Models;
using AspNetCoreIdentityApp.Web.Services;
using AspNetCoreIdentityApp.Web.SettingsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"));
});

//Buradan appsettingsJsonda yazdigimiz datalari uygun olaraq sinife configure etdik
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));



/*Tokenin omru buradan teyin edurik*/
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
{
    opt.TokenLifespan = TimeSpan.FromMinutes(1);
});


/*SecurityStamp life time*/
builder.Services.Configure<SecurityStampValidatorOptions>(opt =>
{
    opt.ValidationInterval = TimeSpan.FromMinutes(35);
});

//File provider tanidiriq burda
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));



builder.Services.IdentityExtension();
builder.Services.AddScoped<IEmailService,EmailService>();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.Cookie.Name = "Murad_Net";
    //HttpOnly nədir?;
    //Yuxarıdakı nümunədə Server tərəfindən Set - Cookie - ni təyin edərkən HttpOnly bayrağını da görürük, bu nə edir?
    //Bu Bayraqdan istifadə edərək server brauzerə JavaScript vasitəsilə kukiyə girişə icazə verməməyi bildirir.Cookie JavaScript sayəsində oğurlana bilər, çünki JavaScript kodları XSS ​​hücumunda icra edilə bilər. HttpOnly sayəsində JavaScript kodlarının Cookie məlumatlarını oxumasına icazə vermir, XSS hücumundan qorunur.Başqasının kukisi ələ keçirilərsə, Təcavüzkar Sessiya zamanı kuki məlumatının ələ keçirildiyi şəxs kimi çıxış edə bilər(bax: Sessiyanın oğurlanması).
    option.Cookie.HttpOnly = true;
    //Eğer kritik bir cookie’yi (authentication token, session id gibi) Same Site olarak işaretlerseniz, tarayıcınız bunu sadece kendi websitesinden giden isteklerde POST ediyor. A sitesinden B sitesine giden isteklerde, bu cookie yokmuş gibi davranıyor.
    //Lax - The cookie will be sent with "same-site" requests, and with "cross-site" top level navigation.
    //None - The cookie will be sent with all requests (see remarks).
    //Strict - When the value is Strict the cookie will only be sent along with "same-site" requests.
    option.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    //Always - Secure is always marked true. Use this value when your login page and all subsequent pages requiring the authenticated identity are HTTPS. Local development will also need to be done with HTTPS urls.
    //None - Secure is not marked true. Use this value when your login page is HTTPS, but other pages on the site which are HTTP also require authentication information. This setting is not recommended because the authentication information provided with an HTTP request may be observed and used by other computers on your local network or wireless connection.
    //SameAsRequest	 - If the URI that provides the cookie is HTTPS, then the cookie will only be returned to the server on subsequent HTTPS requests. Otherwise if the URI that provides the cookie is HTTP, then the cookie will be returned to the server on all HTTP and HTTPS requests. This value ensures HTTPS for all authenticated requests on deployed servers, and also supports HTTP for localhost development and for servers that do not have HTTPS support.
    option.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
    //Coockie-nin həyatda qalma müddəti
    option.ExpireTimeSpan = TimeSpan.FromDays(60);
    option.SlidingExpiration = true;//60 gun erzinde istifadeci ozu eli ile cixib daxil olsa eger bu 60 gun muddeti avtomatik yenilenecek
    option.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Home/SignIn");
    option.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/Member/SignOut");
    option.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/AccessDenied");
    //İcazəsiz giriş zamanı yönləndirdiyi səhifə
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();//1
app.UseAuthorization();//2


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
