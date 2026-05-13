import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Login } from './features/auth/Login';
import { ProductList } from './features/catalog/ProductList';
import { AdminProducts } from './features/admin/AdminProducts';
import { CartPage } from './features/cart/CartPage';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<ProductList />} />
          <Route path="cart" element={<CartPage />} />
          <Route path="admin" element={<AdminProducts />} />
        </Route>
        <Route path="/login" element={<Login />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
