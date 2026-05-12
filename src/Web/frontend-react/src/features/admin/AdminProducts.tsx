import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getProducts } from '../catalog/catalogApi';
import apiClient from '../../api/apiClient';

export const AdminProducts = () => {
  const queryClient = useQueryClient();
  const { data: products, isLoading } = useQuery({ queryKey: ['products'], queryFn: getProducts });

  const deleteMutation = useMutation({
    mutationFn: (id: string) => apiClient.delete(`/catalog-service/products/${id}`),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['products'] }),
  });

  if (isLoading) return <div className="text-black">Loading...</div>;

  return (
    <div className="bg-white p-6 rounded-lg shadow text-black">
      <div className="flex justify-between items-center mb-6">
        <h2 className="text-2xl font-bold">Manage Products</h2>
        <button className="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
          + Add Product
        </button>
      </div>
      <div className="overflow-x-auto">
        <table className="w-full text-left">
          <thead>
            <tr className="border-b text-gray-600">
              <th className="py-2">Name</th>
              <th>Price</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {products?.map((p: any) => (
              <tr key={p.id} className="border-b hover:bg-gray-50">
                <td className="py-2">{p.name}</td>
                <td>${p.price}</td>
                <td className="space-x-4">
                  <button className="text-blue-600 hover:underline">Edit</button>
                  <button 
                    onClick={() => {
                      if (confirm('Are you sure?')) {
                        deleteMutation.mutate(p.id);
                      }
                    }}
                    className="text-red-600 hover:underline"
                    disabled={deleteMutation.isPending}
                  >
                    {deleteMutation.isPending ? 'Deleting...' : 'Delete'}
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};
