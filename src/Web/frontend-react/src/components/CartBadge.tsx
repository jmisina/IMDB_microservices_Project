import { ShoppingCart } from 'lucide-react';
import { Link } from 'react-router-dom';
import { useCartStore } from '../store/useCartStore';

export const CartBadge = () => {
  const count = useCartStore((state) => 
    state.items.reduce((total, item) => total + item.quantity, 0)
  );

  return (
    <Link to="/cart" className="relative p-2 text-gray-600 hover:text-blue-600 transition-colors">
      <ShoppingCart className="w-6 h-6" />
      {count > 0 && (
        <span className="absolute top-0 right-0 inline-flex items-center justify-center px-2 py-1 text-xs font-bold leading-none text-white transform translate-x-1/4 -translate-y-1/4 bg-red-600 rounded-full">
          {count}
        </span>
      )}
    </Link>
  );
};
