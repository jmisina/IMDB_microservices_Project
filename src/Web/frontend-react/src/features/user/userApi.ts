import apiClient from '../../api/apiClient';
import type { User } from '../../types/user';

export interface UpdateProfileData {
  firstName: string;
  lastName: string;
  phone?: string;
}

export const updateProfile = async (userId: number, data: UpdateProfileData) => {
  await apiClient.put(`/user-service/profiles/${userId}`, data);
};

export const getUserDetails = async (userId: number): Promise<User> => {
  const response = await apiClient.get<User>(`/user-service/users/${userId}`);
  return response.data;
};

export const updateUserRole = async (userId: number, role: string) => {
  await apiClient.patch(`/user-service/users/${userId}/role`, { role });
};
