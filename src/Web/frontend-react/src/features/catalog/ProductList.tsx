import { useQuery } from "@tanstack/react-query";
import { getProducts } from "./catalogApi";
import { useCartStore } from "../../store/useCartStore";

export const ProductList = () => {
  const addItem = useCartStore((state) => state.addItem);
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
    <div className="flex flex-col gap-4">
      {products.map((product: { id: string; name: string; description: string; price: number }) => (
        <div
          key={product.id}
          className="rounded-lg bg-white p-4 shadow text-black flex items-center justify-between"
        >
          <div className="flex-grow">
            <h3 className="text-lg font-bold">{product.name}</h3>
            <p className="text-gray-600 text-sm">{product.description}</p>
          </div>
          <div className="flex items-center gap-6 ml-4">
            <span className="text-xl font-bold whitespace-nowrap">{product.price}zł</span>
            <button
              onClick={() => addItem({ id: product.id, name: product.name, price: product.price })}
              className="rounded bg-blue-600 px-4 py-2 text-white hover:bg-blue-700 active:scale-95 transition-all"
            >
              Add to Cart
            </button>
          </div>
        </div>
      ))}
    </div>
  );
};
