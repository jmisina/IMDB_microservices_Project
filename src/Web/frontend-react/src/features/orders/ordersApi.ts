import apiClient from '../../api/apiClient';

export const getOrders = async () => {
  const response = await apiClient.get('/orders-service/orders');
  return response.data.orders;
};

export const updateOrderStatus = async (orderId: number) => {
  await apiClient.put(`/orders-service/orders/${orderId}`);
};
