// src/features/cart/CheckoutSuccess.tsx
import { Link, useLocation, Navigate } from 'react-router-dom';
import { CheckCircle, ShoppingBag } from 'lucide-react';

export const CheckoutSuccess = () => {
  const location = useLocation();
  const orderData = location.state as { orderId: number; totalPrice: number } | null;

  if (!orderData) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="max-w-4xl mx-auto p-8 text-center mt-12">
      <div className="flex justify-center mb-6 text-green-500">
        <CheckCircle size={80} />
      </div>
      <h1 className="text-4xl font-extrabold text-gray-900 mb-4">Order Confirmed!</h1>
      <p className="text-xl text-gray-600 mb-8">
        Thank you for your purchase. Your order has been placed successfully.
      </p>

      <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-8 mb-10 max-w-md mx-auto">
        <div className="flex justify-between mb-4 pb-4 border-b">
          <span className="text-gray-600">Order ID:</span>
          <span className="font-bold text-gray-900">#{orderData.orderId}</span>
        </div>
        <div className="flex justify-between">
          <span className="text-gray-600">Total Amount:</span>
          <span className="font-bold text-gray-900 text-xl">{orderData.totalPrice.toFixed(2)}zł</span>
        </div>
      </div>

      <Link 
        to="/" 
        className="inline-flex items-center gap-2 px-8 py-3 bg-blue-600 text-white font-bold rounded-lg hover:bg-blue-700 transition-colors shadow-md"
      >
        <ShoppingBag size={20} />
        Continue Shopping
      </Link>
    </div>
  );
};
