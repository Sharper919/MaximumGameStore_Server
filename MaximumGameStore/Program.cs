using MaximumGameStore.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace MaximumGameStore
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<MaximumGameStoreContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("MGStoreConnection")
                )
            );

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}