using Wsm.Utils;

namespace XlcToolBox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var apiUrl = AppSetting.Configuration["ApiService"];
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Remove the following line, as it may not be necessary and could be causing issues.
            // app.Urls.Add(apiUrl);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
