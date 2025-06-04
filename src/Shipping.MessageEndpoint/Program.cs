//using Shipping.MessageEndpoint;

using eShop.ServiceDefaults;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

builder.UseNServiceBusWithConventions("shipping-endpoint", "eventbus", "shippingdb");

var host = builder.Build();

host.Run();
