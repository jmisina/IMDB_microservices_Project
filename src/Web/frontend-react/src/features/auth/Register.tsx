import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { register } from './authApi';

export const Register = () => {
  const [formData, setFormData] = useState({
    username: '',
    email: '',
    passwordRaw: '',
    firstName: '',
    lastName: '',
  });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError('');
    try {
      const response = await register(formData);
      if (response.result === 'success') {
        alert('Rejestracja pomyślna! Możesz się teraz zalogować.');
        navigate('/login');
      } else {
        setError('Rejestracja nieudana');
      }
    } catch (err: any) {
      console.error('Registration failed', err);
      setError(err.response?.data?.Message || 'Rejestracja nieudana. Spróbuj ponownie.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-100 p-4">
      <form onSubmit={handleSubmit} className="w-full max-w-md rounded-lg bg-white p-8 shadow-md">
        <h2 className="mb-6 text-2xl font-bold text-gray-800">Rejestracja</h2>
        
        {error && <div className="mb-4 text-sm text-red-600">{error}</div>}

        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="username">
            Nazwa użytkownika
          </label>
          <input
            id="username"
            name="username"
            type="text"
            placeholder="Nazwa użytkownika"
            value={formData.username}
            onChange={handleChange}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>

        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="email">
            Email
          </label>
          <input
            id="email"
            name="email"
            type="email"
            placeholder="Email"
            value={formData.email}
            onChange={handleChange}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>

        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="passwordRaw">
            Hasło
          </label>
          <input
            id="passwordRaw"
            name="passwordRaw"
            type="password"
            placeholder="Hasło"
            value={formData.passwordRaw}
            onChange={handleChange}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>

        <div className="mb-4">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="firstName">
            Imię
          </label>
          <input
            id="firstName"
            name="firstName"
            type="text"
            placeholder="Imię"
            value={formData.firstName}
            onChange={handleChange}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>

        <div className="mb-6">
          <label className="mb-2 block text-sm font-medium text-gray-700" htmlFor="lastName">
            Nazwisko
          </label>
          <input
            id="lastName"
            name="lastName"
            type="text"
            placeholder="Nazwisko"
            value={formData.lastName}
            onChange={handleChange}
            className="w-full rounded border border-gray-300 p-2 text-black focus:border-blue-500 focus:outline-none"
            required
          />
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full rounded bg-blue-600 py-2 font-semibold text-white transition duration-200 hover:bg-blue-700 disabled:opacity-50"
        >
          {loading ? 'Rejestrowanie...' : 'Zarejestruj się'}
        </button>

        <div className="mt-4 text-center">
          <span className="text-sm text-gray-600">Masz już konto? </span>
          <Link to="/login" className="text-sm font-medium text-blue-600 hover:underline">
            Zaloguj się
          </Link>
        </div>
      </form>
    </div>
  );
};

