import apiClient from '../../api/apiClient';

export const login = async (credentials: any) => {
  const response = await apiClient.post('/user-service/login', credentials);
  return response.data; // Expecting JWT string
};
