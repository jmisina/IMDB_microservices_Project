import { Minus, Plus, Trash2 } from 'lucide-react';
import { useCartStore } from '../../store/useCartStore';
import type { CartItem } from '../../store/useCartStore';

interface CartItemProps {
  item: CartItem;
}

export const CartItemComponent = ({ item }: CartItemProps) => {
  const { updateQuantity, removeItem } = useCartStore();

  return (
    <div className="flex items-center py-4 border-b border-gray-200 gap-4">
      <div className="w-16 h-16 bg-gray-100 rounded flex items-center justify-center text-gray-400 flex-shrink-0">
        Img
      </div>
      
      <div className="flex-grow">
        <h3 className="font-semibold text-gray-800">{item.name}</h3>
        <p className="text-gray-600">{item.price.toFixed(2)}zł</p>
      </div>

      <div className="flex items-center gap-2">
        <button 
          onClick={() => updateQuantity(item.id, item.quantity - 1)}
          className="p-1 rounded bg-gray-100 hover:bg-gray-200 text-gray-600"
          disabled={item.quantity <= 1}
          aria-label="Decrease quantity"
        >
          <Minus size={16} />
        </button>
        <span className="w-8 text-center font-medium">{item.quantity}</span>
        <button 
          onClick={() => updateQuantity(item.id, item.quantity + 1)}
          className="p-1 rounded bg-gray-100 hover:bg-gray-200 text-gray-600"
          aria-label="Increase quantity"
        >
          <Plus size={16} />
        </button>
      </div>

      <div className="text-right w-24 font-bold text-gray-800">
        {(item.price * item.quantity).toFixed(2)}zł
      </div>

      <button 
        onClick={() => removeItem(item.id)}
        className="p-2 text-red-400 hover:text-red-600 transition-colors"
        title="Remove item"
        aria-label="Remove item"
      >
        <Trash2 size={20} />
      </button>
    </div>
  );
};
