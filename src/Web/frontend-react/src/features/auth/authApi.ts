import apiClient from '../../api/apiClient';
import type { User } from '../../types/user';

export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterCredentials {
  username: string;
  email: string;
  passwordRaw: string;
  firstName: string;
  lastName: string;
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
  const role = payload.role || payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  // Fetch user details explicitly passing the new token
  const userResponse = await apiClient.get<any>(`/user-service/users/${userId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });

  // Ensure role is mapped correctly even if API returns it with different casing
  const userData = userResponse.data;
  const normalizedUser: User = {
    id: userData.id || userData.Id,
    username: userData.username || userData.Username,
    firstName: userData.firstName || userData.FirstName,
    lastName: userData.lastName || userData.LastName,
    email: userData.email || userData.Email,
    phone: userData.phone || userData.Phone,
    role: role || userData.role || userData.Role
  };

  return { token, user: normalizedUser };
};

export const googleLogin = async (idToken: string): Promise<{ token: string; user: User }> => {
  const response = await apiClient.post('/user-service/login/google', { idToken });
  const token = response.data;

  // Decode JWT to get user ID
  const base64Url = token.split('.')[1];
  const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
  const jsonPayload = decodeURIComponent(atob(base64).split('').map(function(c) {
      return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
  }).join(''));
  const payload = JSON.parse(jsonPayload);
  const userId = payload.sub;
  const role = payload.role || payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  // Fetch user details
  const userResponse = await apiClient.get<any>(`/user-service/users/${userId}`, {
    headers: { Authorization: `Bearer ${token}` }
  });

  const userData = userResponse.data;
  const normalizedUser: User = {
    id: userData.id || userData.Id,
    username: userData.username || userData.Username,
    firstName: userData.firstName || userData.FirstName,
    lastName: userData.lastName || userData.LastName,
    email: userData.email || userData.Email,
    phone: userData.phone || userData.Phone,
    role: role || userData.role || userData.Role
  };

  return { token, user: normalizedUser };
};

export const register = async (credentials: RegisterCredentials): Promise<{ result: string }> => {
  const response = await apiClient.post('/user-service/users', credentials);
  return response.data;
};
