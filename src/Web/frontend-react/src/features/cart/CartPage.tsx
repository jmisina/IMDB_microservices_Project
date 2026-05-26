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
      setError("Zaloguj się, aby złożyć zamówienie.");
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
    } catch {
      setError("Błąd zamówienia. Spróbuj ponownie później.");
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
        <h2 className="text-2xl font-bold text-gray-800 mb-4">Twój koszyk jest pusty</h2>
        <p className="text-gray-600 mb-8">Wygląda na to, że nie dodałeś jeszcze żadnych produktów.</p>
        <Link 
          to="/" 
          className="inline-flex items-center gap-2 px-6 py-3 bg-blue-600 text-white font-semibold rounded-lg hover:bg-blue-700 transition-colors"
        >
          <ArrowLeft size={20} />
          Powrót do sklepu
        </Link>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto p-4 md:p-8">
      <div className="flex justify-between items-end mb-8">
        <h1 className="text-4xl font-black text-slate-900 tracking-tight">Koszyk</h1>
        <Link to="/" className="text-amber-600 font-bold hover:underline transition-all">
          Kontynuuj zakupy
        </Link>
      </div>

      <div className="bg-white rounded-3xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-6 md:p-8 divide-y divide-slate-100">
          {items.map(item => (
            <CartItemComponent key={item.id} item={item} />
          ))}
        </div>
        
        <div className="bg-slate-50 p-6 md:p-8 flex flex-col md:flex-row justify-between items-center gap-6">
          <button 
            onClick={clearCart}
            className="text-slate-500 hover:text-red-500 font-bold transition-colors text-sm"
          >
            Wyczyść koszyk
          </button>
          
          <div className="flex flex-col sm:flex-row items-center gap-6 w-full md:w-auto">
            <div className="text-lg text-slate-600 font-medium">
              Suma: <span className="text-3xl font-black text-slate-900 ml-2">{total.toFixed(2)} zł</span>
            </div>
            {error && <p className="text-red-500 text-sm font-bold">{error}</p>}
            <button 
              onClick={handleCheckout}
              disabled={isSubmitting}
              className="px-8 py-4 bg-amber-500 text-slate-900 font-black rounded-2xl hover:bg-amber-400 transition-all shadow-lg shadow-amber-500/20 disabled:bg-slate-400 active:scale-95 w-full sm:w-auto"
            >
              {isSubmitting ? 'Przetwarzanie...' : 'Zamów teraz'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

