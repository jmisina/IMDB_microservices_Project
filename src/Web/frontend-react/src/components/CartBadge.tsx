import { ShoppingCart } from 'lucide-react';
import { Link } from 'react-router-dom';
import { useCartStore } from '../store/useCartStore';

export const CartBadge = () => {
  const count = useCartStore((state) => 
    state.items.reduce((total, item) => total + item.quantity, 0)
  );

  return (
    <Link to="/cart" className="relative p-2 text-slate-300 hover:text-amber-500 transition-all hover:scale-110 active:scale-95 group">
      <ShoppingCart className="w-6 h-6" />
      {count > 0 && (
        <span className="absolute -top-0.5 -right-0.5 inline-flex items-center justify-center min-w-[20px] h-[20px] px-1 text-[10px] font-black leading-none text-slate-900 transform bg-amber-500 rounded-full border-2 border-slate-900 shadow-lg group-hover:bg-amber-400">
          {count}
        </span>
      )}
    </Link>
  );
};
