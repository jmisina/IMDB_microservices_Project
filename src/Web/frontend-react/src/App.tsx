import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Login } from './features/auth/Login';
import { Register } from './features/auth/Register';
import { ProductList } from './features/catalog/ProductList';
import { AdminProducts } from './features/admin/AdminProducts';
import { ManageUsers } from './features/admin/ManageUsers';
import { ManageOrders } from './features/orders/ManageOrders';
import { Profile } from './features/user/Profile';
import { CartPage } from './features/cart/CartPage';
import { CheckoutSuccess } from './features/cart/CheckoutSuccess';
import { ProtectedRoute } from './components/ProtectedRoute';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<ProductList />} />
          <Route path="cart" element={<CartPage />} />
          <Route path="profile" element={<Profile />} />
          <Route path="checkout/success" element={<CheckoutSuccess />} />
          
          {/* Admin & Manager shared routes */}
          <Route element={<ProtectedRoute roles={["ADMIN", "MANAGER"]} />}>
            <Route path="admin/products" element={<AdminProducts />} />
            <Route path="admin/orders" element={<ManageOrders />} />
          </Route>

          {/* Admin only routes */}
          <Route element={<ProtectedRoute roles={["ADMIN"]} />}>
            <Route path="admin/users" element={<ManageUsers />} />
          </Route>
        </Route>
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
