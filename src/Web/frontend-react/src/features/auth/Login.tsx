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
      // Destructure token and user from the updated authApi login function
      const { token, user } = await login({ email, password });
      
      // Correctly pass the fully-typed User object to the store
      setAuth(token, user);
      
      navigate('/');
    } catch (error) {
      console.error('Login failed', error);
      alert('Login failed');
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <form onSubmit={handleSubmit} className="w-full max-w-md rounded-lg bg-white p-8 shadow-md">
        <h2 className="mb-6 text-2xl font-bold text-gray-800">Login</h2>
        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="email">
            Email
          </label>
          <input
            id="email"
            type="email"
            placeholder="Email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>
        <div className="mb-6">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="password">
            Password
          </label>
          <input
            id="password"
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>
        <button
          type="submit"
          className="w-full rounded bg-blue-600 py-2 font-semibold text-white transition duration-200 hover:bg-blue-700"
        >
          Sign In
        </button>
      </form>
    </div>
  );
};
