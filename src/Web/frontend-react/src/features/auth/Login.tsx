import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuthStore } from '../../store/useAuthStore';
import { login, googleLogin } from './authApi';
import { GoogleLogin } from '@react-oauth/google';

export const Login = () => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const setAuth = useAuthStore((state) => state.setAuth);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const { token, user } = await login({ email, password });
      setAuth(token, user);
      navigate('/');
    } catch (error) {
      console.error('Login failed', error);
      alert('Logowanie nieudane');
    }
  };

  const handleGoogleSuccess = async (credentialResponse: any) => {
    try {
      const { token, user } = await googleLogin(credentialResponse.credential);
      setAuth(token, user);
      navigate('/');
    } catch (error) {
      console.error('Google login failed', error);
      alert('Logowanie Google nieudane');
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-slate-100 p-4">
      <div className="w-full max-w-md rounded-3xl bg-white p-10 shadow-2xl shadow-slate-200 border border-slate-100">
        <h2 className="mb-8 text-3xl font-black text-slate-900 text-center tracking-tight">Logowanie</h2>
        
        <div className="flex justify-center mb-8">
          <GoogleLogin
            onSuccess={handleGoogleSuccess}
            onError={() => {
              console.error('Google Login Failed');
              alert('Logowanie Google nieudane');
            }}
            useOneTap
            theme="filled_blue"
            text="signin_with"
            shape="rectangular"
          />
        </div>

        <div className="relative mb-8">
          <div className="absolute inset-0 flex items-center">
            <div className="w-full border-t border-slate-200"></div>
          </div>
          <div className="relative flex justify-center text-sm">
            <span className="bg-white px-3 text-slate-400 font-medium">lub przez email</span>
          </div>
        </div>

        <form onSubmit={handleSubmit} className="space-y-5">
          <div>
            <label className="block text-sm font-bold text-slate-700 mb-2 ml-1" htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              placeholder="twoj@email.com"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full rounded-2xl border-2 border-slate-200 p-4 text-slate-900 focus:border-amber-500 focus:ring-4 focus:ring-amber-500/10 focus:outline-none transition-all"
              required
            />
          </div>
          <div>
            <label className="block text-sm font-bold text-slate-700 mb-2 ml-1" htmlFor="password">Hasło</label>
            <input
              id="password"
              type="password"
              placeholder="••••••••"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full rounded-2xl border-2 border-slate-200 p-4 text-slate-900 focus:border-amber-500 focus:ring-4 focus:ring-amber-500/10 focus:outline-none transition-all"
              required
            />
          </div>
          <button
            type="submit"
            className="w-full rounded-2xl bg-slate-900 py-4 font-black text-amber-500 transition-all hover:bg-slate-800 active:scale-95 text-lg mt-4 shadow-lg shadow-slate-900/20"
          >
            Zaloguj się
          </button>
          <div className="mt-6 text-center text-sm">
            <span className="text-slate-500">Nie masz konta? </span>
            <Link to="/register" className="font-bold text-amber-600 hover:text-amber-700 hover:underline">
              Zarejestruj się
            </Link>
          </div>
        </form>
      </div>
    </div>
  );
};


