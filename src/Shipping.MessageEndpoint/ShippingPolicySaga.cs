using eShop.Catalog.Contracts;
using eShop.Ordering.Contracts;

namespace Shipping.MessageEndpoint;

public class ShippingPolicyData : ContainSagaData
{
    public int OrderId { get; set; }
    public bool OrderDetailsConfirmed { get; set; }
    public bool OrderStockConfirmed { get; set; }
    public bool OrderStockRejected { get; set; }
}

public class ShippingPolicySaga : Saga<ShippingPolicyData>,
    IAmStartedByMessages<OrderDetailsConfirmedEvent>,
    IAmStartedByMessages<OrderStockConfirmedEvent>,
    IAmStartedByMessages<OrderStockRejectedEvent>
{
    private readonly ILogger<ShippingPolicySaga> _logger;

    public ShippingPolicySaga(ILogger<ShippingPolicySaga> logger)
    {
        _logger = logger;
    }
    
    protected override void ConfigureHowToFindSaga(SagaPropertyMapper<ShippingPolicyData> mapper)
    {
        mapper.MapSaga(s => s.OrderId)
            .ToMessage<OrderDetailsConfirmedEvent>(msg => msg.OrderId)
            .ToMessage<OrderStockConfirmedEvent>(msg => msg.OrderId)
            .ToMessage<OrderStockRejectedEvent>(msg => msg.OrderId);
    }

    public Task Handle(OrderDetailsConfirmedEvent message, IMessageHandlerContext context)
    {
        Data.OrderDetailsConfirmed = true;
        
        CheckIfCompleted();
        
        return Task.CompletedTask;
    }

    private void CheckIfCompleted()
    {
        if (Data.OrderDetailsConfirmed && Data.OrderStockConfirmed)
        {
            _logger.LogInformation("Shipping order {orderId}", Data.OrderId);
            
            MarkAsComplete();
        }

        if (Data.OrderDetailsConfirmed && Data.OrderStockRejected)
        {
            _logger.LogInformation("No ship for you {orderId}", Data.OrderId);
            
            MarkAsComplete();
        }
    }

    public Task Handle(OrderStockConfirmedEvent message, IMessageHandlerContext context)
    {
        Data.OrderStockConfirmed = true;
        
        CheckIfCompleted();
        
        return Task.CompletedTask;
    }

    public Task Handle(OrderStockRejectedEvent message, IMessageHandlerContext context)
    {
        Data.OrderStockRejected = true;
        
        CheckIfCompleted();
        
        return Task.CompletedTask;
    }
}
