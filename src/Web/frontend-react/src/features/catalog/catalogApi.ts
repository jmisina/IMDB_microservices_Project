import apiClient from '../../api/apiClient';

export const getProducts = async () => {
  const response = await apiClient.get('/catalog-service/products');
  return response.data.products;
};
