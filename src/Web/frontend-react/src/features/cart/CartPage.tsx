import { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { ArrowLeft, ShoppingBag } from 'lucide-react';
import { useCartStore } from '../../store/useCartStore';
import { useAuthStore } from '../../store/useAuthStore';
import { createOrder } from '../../api/ordersApi';
import { CartItemComponent } from './CartItemComponent';

export const CartPage = () => {
  const { items, getTotalPrice, clearCart } = useCartStore();
  const total = getTotalPrice();
  
  const navigate = useNavigate();
  const user = useAuthStore((state) => state.user);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleCheckout = async () => {
    if (!user) {
      setError("Please login to proceed with checkout.");
      return;
    }

    setIsSubmitting(true);
    setError(null);

    try {
      const orderItems = items.map(item => ({
        productId: item.id,
        quantity: item.quantity
      }));

      const result = await createOrder({
        userId: user.id,
        orderItems
      });

      clearCart();
      navigate('/checkout/success', { 
        state: { orderId: result.id, totalPrice: result.totalPrice } 
      });
    } catch (err: any) {
      setError("Checkout failed. Please try again later.");
    } finally {
      setIsSubmitting(false);
    }
  };

  if (items.length === 0) {
    return (
      <div className="max-w-4xl mx-auto p-8 text-center mt-12">
        <div className="flex justify-center mb-6 text-gray-300">
          <ShoppingBag size={64} />
        </div>
        <h2 className="text-2xl font-bold text-gray-800 mb-4">Your cart is empty</h2>
        <p className="text-gray-600 mb-8">Looks like you haven't added any movies yet.</p>
        <Link 
          to="/" 
          className="inline-flex items-center gap-2 px-6 py-3 bg-blue-600 text-white font-semibold rounded-lg hover:bg-blue-700 transition-colors"
        >
          <ArrowLeft size={20} />
          Back to Shop
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-8">
      <div className="flex justify-between items-end mb-8">
        <h1 className="text-3xl font-bold text-gray-900">Shopping Cart</h1>
        <Link to="/" className="text-blue-600 hover:underline text-sm font-medium">
          Continue Shopping
        </Link>
      </div>

      <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
        <div className="p-6">
          {items.map(item => (
            <CartItemComponent key={item.id} item={item} />
          ))}
        </div>
        
        <div className="bg-gray-50 p-6 flex flex-col md:flex-row justify-between items-center gap-4">
          <button 
            onClick={clearCart}
            className="text-gray-500 hover:text-red-600 text-sm font-medium transition-colors"
          >
            Clear Cart
          </button>
          
          <div className="flex items-center gap-6">
            <div className="text-lg text-gray-600">
              Total: <span className="text-2xl font-bold text-gray-900 ml-2">{total.toFixed(2)}zł</span>
            </div>
            {error && <p className="text-red-500 text-sm mr-4 font-normal">{error}</p>}
            <button 
              onClick={handleCheckout}
              disabled={isSubmitting}
              className="px-8 py-3 bg-green-600 text-white font-bold rounded-lg hover:bg-green-700 transition-colors shadow-sm disabled:bg-gray-400"
            >
              {isSubmitting ? 'Processing...' : 'Checkout'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
