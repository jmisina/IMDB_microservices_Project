import { Link, Outlet } from 'react-router-dom';
import { useAuthStore } from '../store/useAuthStore';
import { User, Settings } from 'lucide-react';
import { CartBadge } from './CartBadge';

export const Layout = () => {
  const { token, logout } = useAuthStore();

  return (
    <div className="min-h-screen bg-gray-50 text-black">
      <nav className="bg-white shadow-sm">
        <div className="mx-auto flex max-w-7xl items-center justify-between p-4">
          <Link to="/" className="text-xl font-bold text-blue-600">IMDB Shop</Link>
          <div className="flex items-center space-x-6">
            <CartBadge />
            {token ? (
              <>
                <Link to="/admin" className="flex items-center space-x-1"><Settings size={20} /><span>Admin</span></Link>
                <button onClick={logout} className="text-sm font-medium text-gray-600 cursor-pointer">Logout</button>
              </>
            ) : (
              <Link to="/login" className="flex items-center space-x-1"><User size={20} /><span>Login</span></Link>
            )}
          </div>
        </div>
      </nav>
      <main className="mx-auto max-w-7xl p-6">
        <Outlet />
      </main>
    </div>
  );
};
