using Microsoft.EntityFrameworkCore;
using TaskManagmentAPI.Database;
using TaskManagmentAPI.Controllers;
using System.Configuration;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//database connection
var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContext");
builder.Services.AddDbContext<SqlConnectionDB>(options => options.UseSqlServer(connectionString));

// Configure rate limiting
builder.Services.AddRateLimiter(options => options.AddFixedWindowLimiter("FixedWindowPolicy", opt =>
{
    //time
    opt.Window = TimeSpan.FromSeconds(5);
    //number of limits
    opt.PermitLimit = 5;
    //allowed number of limits in queue if exceeded block any call
    opt.QueueLimit = 15;
    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
}).RejectionStatusCode = 429 //too many requests
);

builder.Services.AddRateLimiter(options => options.AddSlidingWindowLimiter("SlidingWindowPolicy", opt =>
{
    //time
    opt.Window = TimeSpan.FromSeconds(5);
    //number of limits
    opt.PermitLimit = 5;
    //allowed number of limits in queue if exceeded block any call
    opt.QueueLimit = 10;
    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    opt.SegmentsPerWindow = 4;
}).RejectionStatusCode = 429 //too many requests
);

builder.Services.AddRateLimiter(options => options.AddConcurrencyLimiter("ConcurrentWindowPolicy", opt =>
{

    //number of limits
    opt.PermitLimit = 5;
    //allowed number of limits in queue if exceeded block any call
    opt.QueueLimit = 10;
    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

}).RejectionStatusCode = 429 //too many requests
);

builder.Services.AddRateLimiter(options => options.AddTokenBucketLimiter("TokenBucketPolicy", opt =>
{

    opt.TokenLimit = 5;
    //allowed number of limits in queue if exceeded block any call
    opt.QueueLimit = 10;
    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
    opt.TokensPerPeriod = 4;
    opt.AutoReplenishment = true;
}).RejectionStatusCode = 429 //too many requests
);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use rate limiting middleware
app.UseRateLimiter();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


//app.MapTaskModelEndpoints();

app.Run();
