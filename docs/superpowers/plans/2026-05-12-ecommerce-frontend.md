# E-commerce Frontend Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build a React-based e-commerce frontend with a customer storefront and an administrative panel, integrated with existing microservices.

**Architecture:** Modular Monolith with feature-based organization. Uses React Query for server state and Zustand for local state (cart/auth).

**Tech Stack:** React (TypeScript), Vite, Tailwind CSS, TanStack Query, Zustand, Axios, React Router.

---

### Task 1: Project Scaffolding

**Files:**
- Create: `src/Web/frontend-react/*`

- [ ] **Step 1: Initialize Vite project with React and TypeScript**

Run: `npm create vite@latest src/Web/frontend-react -- --template react-ts`

- [ ] **Step 2: Install core dependencies**

Run: `cd src/Web/frontend-react && npm install axios lucide-react react-router-dom @tanstack/react-query zustand`

- [ ] **Step 3: Install Tailwind CSS and its dependencies**

Run: `cd src/Web/frontend-react && npm install -D tailwindcss postcss autoprefixer && npx tailwindcss init -p`

- [ ] **Step 4: Configure Tailwind CSS**

Update `src/Web/frontend-react/tailwind.config.js`:
```javascript
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

- [ ] **Step 5: Add Tailwind directives to CSS**

Replace content of `src/Web/frontend-react/src/index.css`:
```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

- [ ] **Step 6: Commit project scaffold**

Run: `git add src/Web/frontend-react && git commit -m "chore: scaffold react project with tailwind"`

---

### Task 2: API Client and Auth Store

**Files:**
- Create: `src/Web/frontend-react/src/api/apiClient.ts`
- Create: `src/Web/frontend-react/src/store/useAuthStore.ts`

- [ ] **Step 1: Create Auth Store with Zustand**

`src/Web/frontend-react/src/store/useAuthStore.ts`:
```typescript
import { create } from 'zustand';
import { persist } from 'zustand/middleware';

interface AuthState {
  token: string | null;
  user: any | null;
  setAuth: (token: string, user: any) => void;
  logout: () => void;
}

export const useAuthStore = create<AuthState>()(
  persist(
    (set) => ({
      token: null,
      user: null,
      setAuth: (token, user) => set({ token, user }),
      logout: () => set({ token: null, user: null }),
    }),
    { name: 'auth-storage' }
  )
);
```

- [ ] **Step 2: Create Axios Instance with Interceptors**

`src/Web/frontend-react/src/api/apiClient.ts`:
```typescript
import axios from 'axios';
import { useAuthStore } from '../store/useAuthStore';

const apiClient = axios.create({
  baseURL: 'http://localhost:8000', // YARP Gateway
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use((config) => {
  const token = useAuthStore.getState().token;
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default apiClient;
```

- [ ] **Step 3: Commit API and Auth setup**

Run: `git add src/Web/frontend-react/src/api src/Web/frontend-react/src/store && git commit -m "feat: setup api client and auth store"`

---

### Task 3: Auth Feature - Login Page

**Files:**
- Create: `src/Web/frontend-react/src/features/auth/Login.tsx`
- Create: `src/Web/frontend-react/src/features/auth/authApi.ts`

- [ ] **Step 1: Define Login API**

`src/Web/frontend-react/src/features/auth/authApi.ts`:
```typescript
import apiClient from '../../api/apiClient';

export const login = async (credentials: any) => {
  const response = await apiClient.post('/user-service/login', credentials);
  return response.data; // Expecting JWT string
};
```

- [ ] **Step 2: Create Login Page Component**

`src/Web/frontend-react/src/features/auth/Login.tsx`:
```tsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../store/useAuthStore';
import { login } from './authApi';

export const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const setAuth = useAuthStore((state) => state.setAuth);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const token = await login({ email, password });
      setAuth(token, { email }); // Simple user object for now
      navigate('/');
    } catch (error) {
      alert('Login failed');
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100">
      <form onSubmit={handleSubmit} className="w-full max-w-md rounded-lg bg-white p-8 shadow-md">
        <h2 className="mb-6 text-2xl font-bold">Login</h2>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          className="mb-4 w-full rounded border p-2"
          required
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          className="mb-6 w-full rounded border p-2"
          required
        />
        <button type="submit" className="w-full rounded bg-blue-600 py-2 text-white hover:bg-blue-700">
          Sign In
        </button>
      </form>
    </div>
  );
};
```

- [ ] **Step 3: Commit Login feature**

Run: `git add src/Web/frontend-react/src/features/auth && git commit -m "feat: add login page and auth api"`

---

### Task 4: Routing and Layout

**Files:**
- Create: `src/Web/frontend-react/src/components/Layout.tsx`
- Modify: `src/Web/frontend-react/src/App.tsx`
- Modify: `src/Web/frontend-react/src/main.tsx`

- [ ] **Step 1: Create Main Layout Component**

`src/Web/frontend-react/src/components/Layout.tsx`:
```tsx
import { Link, Outlet } from 'react-router-dom';
import { useAuthStore } from '../store/useAuthStore';
import { ShoppingCart, User, Settings } from 'lucide-react';

export const Layout = () => {
  const { token, logout } = useAuthStore();

  return (
    <div className="min-h-screen bg-gray-50">
      <nav className="bg-white shadow-sm">
        <div className="mx-auto flex max-w-7xl items-center justify-between p-4">
          <Link to="/" className="text-xl font-bold text-blue-600">IMDB Shop</Link>
          <div className="flex items-center space-x-6">
            <Link to="/cart" className="flex items-center space-x-1"><ShoppingCart size={20} /><span>Cart</span></Link>
            {token ? (
              <>
                <Link to="/admin" className="flex items-center space-x-1"><Settings size={20} /><span>Admin</span></Link>
                <button onClick={logout} className="text-sm font-medium text-gray-600">Logout</button>
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
```

- [ ] **Step 2: Setup React Query and Router in Main**

`src/Web/frontend-react/src/main.tsx`:
```tsx
import React from 'react'
import ReactDOM from 'react-dom/client'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import App from './App.tsx'
import './index.css'

const queryClient = new QueryClient()

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <QueryClientProvider client={queryClient}>
      <App />
    </QueryClientProvider>
  </React.StrictMode>,
)
```

- [ ] **Step 3: Define Routes in App**

`src/Web/frontend-react/src/App.tsx`:
```tsx
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
```

- [ ] **Step 4: Commit Layout and Routing**

Run: `git add src/Web/frontend-react/src/components src/Web/frontend-react/src/App.tsx src/Web/frontend-react/src/main.tsx && git commit -m "feat: setup main layout and routing"`

---

### Task 5: Catalog Feature - Product List

**Files:**
- Create: `src/Web/frontend-react/src/features/catalog/catalogApi.ts`
- Create: `src/Web/frontend-react/src/features/catalog/ProductList.tsx`
- Modify: `src/Web/frontend-react/src/App.tsx`

- [ ] **Step 1: Define Catalog API**

`src/Web/frontend-react/src/features/catalog/catalogApi.ts`:
```typescript
import apiClient from '../../api/apiClient';

export const getProducts = async () => {
  const response = await apiClient.get('/catalog-service/products');
  return response.data.products;
};
```

- [ ] **Step 2: Create Product List Component**

`src/Web/frontend-react/src/features/catalog/ProductList.tsx`:
```tsx
import { useQuery } from '@tanstack/react-query';
import { getProducts } from './catalogApi';

export const ProductList = () => {
  const { data: products, isLoading, error } = useQuery({
    queryKey: ['products'],
    queryFn: getProducts,
  });

  if (isLoading) return <div>Loading products...</div>;
  if (error) return <div>Error loading products</div>;

  return (
    <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
      {products.map((product: any) => (
        <div key={product.id} className="rounded-lg bg-white p-4 shadow">
          <div className="mb-4 h-48 w-full bg-gray-200 rounded"></div>
          <h3 className="text-lg font-bold">{product.name}</h3>
          <p className="text-gray-600">{product.description}</p>
          <div className="mt-4 flex items-center justify-between">
            <span className="text-xl font-bold">${product.price}</span>
            <button className="rounded bg-blue-600 px-3 py-1 text-white hover:bg-blue-700">
              Add to Cart
            </button>
          </div>
        </div>
      ))}
    </div>
  );
};
```

- [ ] **Step 3: Update App Routes**

`src/Web/frontend-react/src/App.tsx`:
```tsx
// ... imports
import { ProductList } from './features/catalog/ProductList';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<ProductList />} />
          {/* ... */}
        </Route>
        {/* ... */}
      </Routes>
    </BrowserRouter>
  );
}
```

- [ ] **Step 4: Commit Catalog feature**

Run: `git add src/Web/frontend-react/src/features/catalog src/Web/frontend-react/src/App.tsx && git commit -m "feat: add product list feature"`

---

### Task 6: Admin Feature - Product Management

**Files:**
- Create: `src/Web/frontend-react/src/features/admin/AdminProducts.tsx`
- Modify: `src/Web/frontend-react/src/App.tsx`

- [ ] **Step 1: Create Admin Products Component (Simplified CRUD view)**

`src/Web/frontend-react/src/features/admin/AdminProducts.tsx`:
```tsx
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getProducts } from '../catalog/catalogApi';
import apiClient from '../../api/apiClient';

export const AdminProducts = () => {
  const queryClient = useQueryClient();
  const { data: products } = useQuery({ queryKey: ['products'], queryFn: getProducts });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiClient.delete(`/catalog-service/products/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['products'] }),
  });

  return (
    <div className="bg-white p-6 rounded-lg shadow">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Manage Products</h2>
        <button className="bg-green-600 text-white px-4 py-2 rounded">+ Add Product</button>
      </div>
      <table className="w-full text-left">
        <thead>
          <tr className="border-b">
            <th className="py-2">Name</th>
            <th>Price</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products?.map((p: any) => (
            <tr key={p.id} className="border-b">
              <td className="py-2">{p.name}</td>
              <td>${p.price}</td>
              <td className="space-x-4">
                <button className="text-blue-600">Edit</button>
                <button 
                  onClick={() => deleteMutation.mutate(p.id)}
                  className="text-red-600"
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};
```

- [ ] **Step 2: Update App Routes**

`src/Web/frontend-react/src/App.tsx`:
```tsx
// ... imports
import { AdminProducts } from './features/admin/AdminProducts';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<ProductList />} />
          <Route path="admin" element={<AdminProducts />} />
          {/* ... */}
        </Route>
        {/* ... */}
      </Routes>
    </BrowserRouter>
  );
}
```

- [ ] **Step 3: Commit Admin feature**

Run: `git add src/Web/frontend-react/src/features/admin src/Web/frontend-react/src/App.tsx && git commit -m "feat: add admin product management"`
