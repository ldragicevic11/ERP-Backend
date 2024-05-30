using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.OrderDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using Stripe;
using Stripe.Checkout;
using System.Linq;

namespace SmartZonePhoneShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StripeController : ControllerBase
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly ApplicationDbContext _context;

    public StripeController(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        ApplicationDbContext context)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _context = context;
    }

    [Authorize(Policy = "Kupac")]
    [HttpPost("create-checkout-session")]
    public async Task<ActionResult> CreateCheckoutSession([FromBody] CheckoutDTO model)
    {
        var user = _context.Users.First(x => x.UserId == model.UserId);
        var cart = _cartRepository.GetCartByUserID(user.UserId);

        var deliveryFee = CalculateDeliveryFee(cart.CartItems.Sum(e => e.SumPrice));
        deliveryFee /= cart.CartItems.Count();

        var cartItems = cart.CartItems.Select(e => new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = ((long)e.SumPrice + deliveryFee) * 100,
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = e.Product.Name,
                },
            },
            Quantity = e.Quantity,
        }).ToList();

        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = new List<string>
                {
                    "card",
                },
            LineItems = cartItems,
            Mode = "payment",
            SuccessUrl = $"http://localhost:4200/success",
            CancelUrl = "http://localhost:4200/cart",
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return Ok(new { sessionId = session.Id });
    }

    private int CalculateDeliveryFee(double amount)
    {
        if (amount > 50_000) 
        {
            return 2000;
        }
        else if (amount > 20_000)
        {
            return 1000;
        }
        else if (amount > 20_000)
        {
            return 500;
        } 
        else
        {
            return 200;
        }
    }

    [Authorize(Policy = "Kupac")]
    [HttpPost("confirm-order")]
    public async Task<ActionResult> ConfirmOrder([FromBody] CheckoutDTO model)
    {
        var user = _context.Users.First(x => x.UserId == model.UserId);
        var cart = _cartRepository.GetCartByUserID(user.UserId);

        var itemsSum = cart.CartItems.Sum(e => e.SumPrice);
        var order = new Order
        {
            UserId = user.UserId,
            PaymentMethod = "card",
            Quantity = cart.CartItems.Count(),
            Date = DateTime.Now,
            City = user.City,
            PaymentSum = itemsSum + CalculateDeliveryFee(itemsSum),
            Street = user.Address,
            OrderStatusId = 1,
            OrderItems = new List<OrderItem>()
        };

        foreach (var cartItem in cart.CartItems)
        {
            var orderItem = new OrderItem
            {
                Quantity = cartItem.Quantity,
                ProductId = cartItem.ProductId,
                ProductPrice = cartItem.SumPrice,
            };

            order.OrderItems.Add(orderItem);
        }

        await _orderRepository.AddAsync(order);

        _context.Carts.Remove(cart);
        _context.SaveChanges();

        return Ok(new { OrderId = order.OrderId });
    }
}