import apiClient from './apiClient';

export interface OrderItemRequest {
  productId: string;
  quantity: number;
}

export interface CreateOrderRequest {
  userId: number;
  orderItems: OrderItemRequest[];
}

export interface CreateOrderResponse {
  id: number;
  totalPrice: number;
}

export const createOrder = async (request: CreateOrderRequest): Promise<CreateOrderResponse> => {
  const response = await apiClient.post<CreateOrderResponse>('/orders-service/orders', request);
  return response.data;
};
