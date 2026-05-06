using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using Example.API.Data;
using Example.API.Infrastructures;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

try
{
    IConfiguration configuration = builder.Configuration;

    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US", false);

    ConfigureServices.InstallService(builder.Services, configuration);

    var app = builder.Build();

    using (IServiceScope scope = app.Services.CreateScope())
    {
        AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }

    app.UseCors("AllowAnyOrigin");

    if (app.Environment.IsDevelopment())
        app.UseDeveloperExceptionPage();

    app.UsePathBase("/example-api");

    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.ToString());
}
