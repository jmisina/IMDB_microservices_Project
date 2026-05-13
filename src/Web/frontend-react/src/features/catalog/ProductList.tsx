import { useQuery } from "@tanstack/react-query";
import { getProducts } from "./catalogApi";

export const ProductList = () => {
  const {
    data: products,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["products"],
    queryFn: getProducts,
  });

  if (isLoading) return <div className="text-black">Loading products...</div>;
  if (error) return <div className="text-red-500">Error loading products</div>;

  return (
    <div className="grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-4">
      {products.map((product: { id: string; name: string; description: string; price: number }) => (
        <div
          key={product.id}
          className="rounded-lg bg-white p-4 shadow text-black"
        >
          <div className="mb-4 h-48 w-full bg-gray-200 rounded flex items-center justify-center text-gray-400">
            No Image
          </div>
          <h3 className="text-lg font-bold">{product.name}</h3>
          <p className="text-gray-600 text-sm">{product.description}</p>
          <div className="mt-4 flex items-center justify-between">
            <span className="text-xl font-bold">{product.price}zł</span>
            <button className="rounded bg-blue-600 px-3 py-1 text-white hover:bg-blue-700">
              Add to Cart
            </button>
          </div>
        </div>
      ))}
    </div>
  );
};
