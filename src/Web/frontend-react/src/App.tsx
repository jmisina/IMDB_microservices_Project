import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import { Login } from './features/auth/Login';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<div>Product Catalog (Coming Soon)</div>} />
          <Route path="cart" element={<div>Shopping Cart (Coming Soon)</div>} />
          <Route path="admin" element={<div>Admin Dashboard (Coming Soon)</div>} />
        </Route>
        <Route path="/login" element={<Login />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
