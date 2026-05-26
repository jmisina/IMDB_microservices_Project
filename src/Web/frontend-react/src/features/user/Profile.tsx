import React, { useState, useEffect } from 'react';
import { useAuthStore } from '../../store/useAuthStore';
import { getUserDetails, updateProfile } from './userApi';
import { User as UserIcon, Phone, Mail } from 'lucide-react';

export const Profile = () => {
  const { user, setAuth, token } = useAuthStore();
  const [formData, setFormData] = useState({
    firstName: '',
    lastName: '',
    phone: '',
  });
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState({ type: '', text: '' });

  useEffect(() => {
    if (user) {
      setFormData({
        firstName: user.firstName || '',
        lastName: user.lastName || '',
        phone: user.phone || '',
      });
    }
  }, [user]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!user || !token) return;

    setLoading(true);
    setMessage({ type: '', text: '' });
    try {
      await updateProfile(user.id, formData);
      
      // Refresh user data in store
      const updatedUser = await getUserDetails(user.id);
      // We keep the same token, but update the user object
      setAuth(token, { ...updatedUser, role: user.role }); // Ensure role is preserved as it might not be in UserDetails
      
      setMessage({ type: 'success', text: 'Profil zaktualizowany pomyślnie!' });
    } catch (err: any) {
      console.error('Update failed', err);
      setMessage({ type: 'error', text: 'Nie udało się zaktualizować profilu.' });
    } finally {
      setLoading(false);
    }
  };

  if (!user) return <div className="text-center py-10">Zaloguj się, aby zobaczyć profil.</div>;

  return (
    <div className="max-w-2xl mx-auto bg-white rounded-lg shadow-md p-8">
      <div className="flex items-center gap-4 mb-8 border-b pb-4">
        <div className="bg-blue-100 p-3 rounded-full">
          <UserIcon className="text-blue-600" size={32} />
        </div>
        <div>
          <h1 className="text-2xl font-bold text-gray-800">Mój Profil</h1>
          <p className="text-gray-500">Witaj, {user.username}</p>
        </div>
      </div>

      {message.text && (
        <div className={`mb-6 p-4 rounded-lg text-sm ${
          message.type === 'success' ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'
        }`}>
          {message.text}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-6">
        <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Imię</label>
            <input
              type="text"
              name="firstName"
              value={formData.firstName}
              onChange={handleChange}
              className="w-full rounded border border-gray-300 p-2 text-black focus:ring-2 focus:ring-blue-500 focus:outline-none"
              required
            />
          </div>
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-1">Nazwisko</label>
            <input
              type="text"
              name="lastName"
              value={formData.lastName}
              onChange={handleChange}
              className="w-full rounded border border-gray-300 p-2 text-black focus:ring-2 focus:ring-blue-500 focus:outline-none"
              required
            />
          </div>
        </div>

        <div>
          <label className="block text-sm font-medium text-gray-700 mb-1">
            <div className="flex items-center gap-1">
              <Phone size={16} /> Numer telefonu
            </div>
          </label>
          <input
            type="tel"
            name="phone"
            value={formData.phone}
            onChange={handleChange}
            className="w-full rounded border border-gray-300 p-2 text-black focus:ring-2 focus:ring-blue-500 focus:outline-none"
          />
        </div>

        <div className="bg-gray-50 p-4 rounded-lg space-y-2">
          <div className="flex items-center gap-2 text-gray-600 text-sm">
            <Mail size={16} /> <span>{user.email}</span>
          </div>
          <p className="text-xs text-gray-400">Email nie może być zmieniony.</p>
        </div>

        <button
          type="submit"
          disabled={loading}
          className="w-full rounded bg-blue-600 py-2 font-semibold text-white transition duration-200 hover:bg-blue-700 disabled:opacity-50"
        >
          {loading ? 'Zapisywanie...' : 'Zapisz zmiany'}
        </button>
      </form>
    </div>
  );
};
