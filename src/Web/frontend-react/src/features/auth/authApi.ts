import apiClient from '../../api/apiClient';
import type { User } from '../../types/user';

export interface LoginCredentials {
  email: string;
  password: string;
}

export const login = async (credentials: LoginCredentials): Promise<{ token: string; user: User }> => {
  const response = await apiClient.post('/user-service/login', credentials);
  const token = response.data; // Expecting JWT string

  // Decode JWT to get user ID
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));
  const payload = JSON.parse(jsonPayload);
  const userId = payload.sub;

  // Fetch user details explicitly passing the new token
  const userResponse = await apiClient.get<User>(`/user-service/users/${userId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });

  return { token, user: userResponse.data };
};
