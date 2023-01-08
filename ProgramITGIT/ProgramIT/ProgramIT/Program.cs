using ProgramIT;
using Microsoft.AspNetCore.Hosting;
using System;


var builder = new WebHostBuilder();

builder.UseContentRoot(Environment.CurrentDirectory);

builder.UseKestrel((context, options) =>
{
    options.ListenAnyIP(8484);
});

builder.SuppressStatusMessages(false);
builder.UseStartup<Startup>();
builder.Build().Run();
