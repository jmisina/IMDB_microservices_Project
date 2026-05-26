import apiClient from '../../api/apiClient';

export interface GetProductsParams {
  pageNumber?: number;
  pageSize?: number;
  category?: string;
  searchTerm?: string;
}

export const getProducts = async (params: GetProductsParams = {}) => {
  const { pageNumber = 1, pageSize = 12, category, searchTerm } = params;
  
  let url = '/catalog-service/products';
  if (category) {
    url = `/catalog-service/products/category/${category}`;
  }

  const response = await apiClient.get(url, {
    params: { pageNumber, pageSize, searchTerm }
  });
  
  return response.data.products;
};

export const addProduct = async (product: any) => {
  const response = await apiClient.post('/catalog-service/products', product);
  return response.data;
};

export const updateProduct = async (product: any) => {
  const response = await apiClient.put('/catalog-service/products', product);
  return response.data;
};

export const deleteProduct = async (id: string) => {
  await apiClient.delete(`/catalog-service/products/${id}`);
};
