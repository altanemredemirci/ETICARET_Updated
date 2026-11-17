using ETICARET.Business.Abstract;
using ETICARET.Business.Models;
using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Concrete
{
    public class CartManager(ICartDal cartDal, IProductDal productDal) : ICartService
    {
        public async Task<ServiceResult> AddToCartAsync(string userId, int productId, int quantity)
        {
            if (quantity < 1)
                return ServiceResult.Fail("Adet en az 1 olmalıdır");

            var product = await productDal.GetByIdAsync(productId);
            if (product is null)
                return ServiceResult.Fail("Ürün bulunamadı.");

            var cart = await cartDal.GetByUserIdAsync(userId);
            var item = cart?.Items.Find(i => i.ProductId == productId);
            var newQuantity = (item?.Quantity ?? 0) + quantity;

            if (newQuantity > product.Stock)
                return product.Stock == 0
                    ? ServiceResult.Fail($"\"{product.Name}\" şu an stokta yok.")
                    : ServiceResult.Fail($"\"{product.Name}\" için en fazla {product.Stock} adet alabilirsiniz.");
            
            if(cart is null)
            {
                await cartDal.AddAsync(new Cart
                {
                    UserId = userId,
                    Items = [new CartItem { ProductId = productId, Quantity = quantity }]
                });
            }
            else
            {
                if(item is null)
                {
                    cart.Items.Add(new CartItem { ProductId = productId, Quantity = quantity });
                }
                else
                {
                    item.Quantity = newQuantity;
                }

                await cartDal.UpdateAsync(cart);
            }

            return ServiceResult.Ok($"\"{product.Name}\" sepete eklendi.");
        }

        public Task<Cart> GetCartAsync(string userId)
        {
            return cartDal.GetByUserIdAsync(userId);
        }

        public Task<int> GetItemCountAsync(string userId)=> cartDal.GetItemCountAsync(userId);
        

        public async Task<ServiceResult> RemoveFromCartAsync(string userId, int productId)
        {
            var cart = await cartDal.GetByUserIdAsync(userId);
            var item = cart?.Items.Find(i => i.ProductId == productId);

            if(cart is null || item is null)
            {
                return ServiceResult.Fail("Ürün sepette bulunamadı.");
            }

            cart.Items.Remove(item);
            await cartDal.UpdateAsync(cart);
            return ServiceResult.Ok($"\"{item.Product?.Name}\" sepetten çıkarıldı.");
        }

        public async Task<ServiceResult> UpdateQuantityAsync(string userId, int productId, int quantity)
        {
            var cart = await cartDal.GetByUserIdAsync(userId);
            var item = cart?.Items.Find(i => i.ProductId == productId);

            if (cart is null || item?.Product is null)
                return ServiceResult.Fail("Ürün sepette bulunamadı.");

            if (quantity < 1)
                return await RemoveFromCartAsync(userId, productId);

            if (quantity > item.Product.Stock)
                return ServiceResult.Fail($"Stokta {item.Product.Stock} adet var.");

            item.Quantity = quantity;
            await cartDal.UpdateAsync(cart);
            return ServiceResult.Ok("Sepet güncellendi.");
        }
    }
}
