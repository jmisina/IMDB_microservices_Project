import { Link, Outlet, useNavigate } from 'react-router-dom';
import { useAuthStore } from '../store/useAuthStore';
import { User, Settings, Package, Users, ShoppingBag } from 'lucide-react';
import { CartBadge } from './CartBadge';

export const Layout = () => {
  const { token, user, logout } = useAuthStore();
  const navigate = useNavigate();
  const isAdmin = user?.role === 'ADMIN';
  const isManager = user?.role === 'MANAGER';
  const isAdminOrManager = isAdmin || isManager;

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <div className="min-h-screen bg-slate-50 text-slate-900 font-sans">
      <nav className="bg-slate-900 shadow-lg sticky top-0 z-50">
        <div className="mx-auto flex max-w-7xl items-center justify-between p-4">
          <Link to="/" className="text-2xl font-black tracking-tighter text-white flex items-center gap-2">
            <span className="bg-amber-500 text-slate-900 px-2 py-0.5 rounded italic">IMDB</span>
            <span className="text-amber-500">Shop</span>
          </Link>
          <div className="flex items-center space-x-1 md:space-x-4">
            <CartBadge />
            <div className="h-6 w-px bg-slate-700 mx-2 hidden md:block"></div>
            {token ? (
              <div className="flex items-center space-x-1 md:space-x-4">
                <Link to="/profile" className="flex items-center space-x-1 text-slate-300 hover:text-amber-500 transition-colors px-2 py-1 rounded" title="Mój Profil">
                  <User size={20} />
                  <span className="hidden sm:inline font-medium">Profil</span>
                </Link>

                {isAdminOrManager && (
                  <>
                    <Link to="/admin/products" className="flex items-center space-x-1 text-slate-300 hover:text-amber-500 transition-colors px-2 py-1 rounded" title="Zarządzaj Produktami">
                      <ShoppingBag size={20} />
                      <span className="hidden lg:inline font-medium">Produkty</span>
                    </Link>
                    <Link to="/admin/orders" className="flex items-center space-x-1 text-slate-300 hover:text-amber-500 transition-colors px-2 py-1 rounded" title="Zarządzaj Zamówieniami">
                      <Package size={20} />
                      <span className="hidden lg:inline font-medium">Zamówienia</span>
                    </Link>
                  </>
                )}

                {isAdmin && (
                  <Link to="/admin/users" className="flex items-center space-x-1 text-slate-300 hover:text-amber-500 transition-colors px-2 py-1 rounded" title="Zarządzaj Użytkownikami">
                    <Users size={20} />
                    <span className="hidden lg:inline font-medium">Użytkownicy</span>
                  </Link>
                )}

                <button 
                  onClick={handleLogout} 
                  className="text-sm font-bold text-slate-400 hover:text-red-400 cursor-pointer transition-colors px-3 py-1 border border-slate-700 rounded-lg hover:border-red-400"
                >
                  Wyloguj
                </button>
              </div>
            ) : (
              <Link to="/login" className="flex items-center space-x-2 bg-amber-500 text-slate-900 px-4 py-2 rounded-lg font-bold hover:bg-amber-400 transition-all active:scale-95 shadow-md shadow-amber-900/20">
                <User size={18} />
                <span>Zaloguj</span>
              </Link>
            )}
          </div>
        </div>
      </nav>

      <main className="mx-auto max-w-7xl p-4 md:p-8">
        <Outlet />
      </main>
    </div>
  );
};

