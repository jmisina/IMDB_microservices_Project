import { useState } from "react";
import { useQuery } from "@tanstack/react-query";
import { getProducts } from "./catalogApi";
import { useCartStore } from "../../store/useCartStore";
import { Plus, Search } from "lucide-react";

const CATEGORIES = [
  "Wszystkie",
  "Materiały",
  "Narzędzia",
  "Elektronarzędzia",
  "BHP",
  "Chemia",
  "Instalacje",
  "Maszyny",
  "Sprzęt"
];

export const ProductList = () => {
  const [page, setPage] = useState(1);
  const [selectedCategory, setSelectedCategory] = useState("Wszystkie");
  const [searchTerm, setSearchTerm] = useState("");
  const [appliedSearchTerm, setAppliedSearchTerm] = useState("");
  const addItem = useCartStore((state) => state.addItem);

  const handleSearch = () => {
    setAppliedSearchTerm(searchTerm);
    setPage(1);
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') handleSearch();
  };

  const {
    data: products,
    isLoading,
    isError,
  } = useQuery({
    queryKey: ["products", page, selectedCategory, appliedSearchTerm],
    queryFn: () =>
      getProducts({ 
        pageNumber: page, 
        pageSize: 12, 
        category: selectedCategory === "Wszystkie" ? undefined : selectedCategory,
        searchTerm: appliedSearchTerm
      }),
  });

  const handleCategoryChange = (category: string) => {
    setSelectedCategory(category);
    setSearchTerm("");
    setAppliedSearchTerm("");
    setPage(1);
  };

  if (isLoading) return <div className="text-black p-10 text-center">Ładowanie produktów...</div>;
  if (isError) return <div className="text-red-500 p-10 text-center">Błąd podczas ładowania produktów.</div>;

  return (
    <div className="flex flex-col gap-8">
      {/* Search Bar */}
      <div className="w-full max-w-2xl mx-auto">
        <div className="relative group flex gap-2">
          <div className="relative flex-grow">
            <Search className="absolute left-4 top-1/2 -translate-y-1/2 text-slate-400 group-focus-within:text-amber-500 transition-colors" size={20} />
            <input
              type="text"
              placeholder="Szukaj produktów po nazwie..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              onKeyDown={handleKeyDown}
              className="w-full pl-12 pr-4 py-4 border-2 border-slate-200 rounded-2xl focus:ring-4 focus:ring-amber-500/20 focus:border-amber-500 focus:outline-none transition-all shadow-sm text-lg"
            />
          </div>
          <button 
            onClick={handleSearch}
            className="px-6 bg-slate-900 text-amber-500 font-bold rounded-2xl hover:bg-slate-800 transition-all active:scale-95 whitespace-nowrap"
          >
            Wyszukaj
          </button>
        </div>
      </div>

      <div className="flex flex-col md:flex-row gap-8 items-start">
        {/* Sidebar - Categories */}
        <div className="w-full md:w-64 flex-shrink-0 sticky top-24">
          <h2 className="text-sm font-black text-slate-400 uppercase tracking-widest mb-4">Kategorie</h2>
          <div className="flex flex-wrap md:flex-col gap-2">
            {CATEGORIES.map((cat) => (
              <button
                key={cat}
                onClick={() => handleCategoryChange(cat)}
                className={`px-4 py-2.5 rounded-xl text-left font-bold transition-all ${
                  selectedCategory === cat
                    ? "bg-amber-500 text-slate-900 shadow-lg shadow-amber-500/20"
                    : "bg-white text-slate-600 hover:bg-slate-100 border border-slate-200"
                }`}
              >
                {cat}
              </button>
            ))}
          </div>
        </div>

        {/* Main Content - Grid */}
        <div className="flex-grow w-full">
          {products && products.length > 0 ? (
            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
              {products.map((product: { id: string; name: string; description: string; price: number }) => (
                <div
                  key={product.id}
                  className="group bg-white rounded-2xl p-6 shadow-sm border border-slate-200 hover:shadow-xl hover:shadow-slate-200/50 hover:-translate-y-1 transition-all flex flex-col h-full"
                >
                  <div className="flex-grow">
                    <h3 className="text-xl font-bold text-slate-900 mb-2 line-clamp-1">{product.name}</h3>
                    <p className="text-slate-500 text-sm leading-relaxed mb-6 line-clamp-3 min-h-[60px]">{product.description}</p>
                  </div>
                  <div className="flex items-center justify-between mt-auto pt-4 border-t border-slate-100">
                    <span className="text-2xl font-black text-slate-900">{product.price.toFixed(2)} zł</span>
                    <button
                      onClick={() => addItem({ id: product.id, name: product.name, price: product.price })}
                      className="rounded-xl bg-slate-900 p-3 text-amber-500 hover:bg-amber-500 hover:text-slate-900 active:scale-95 transition-all flex items-center justify-center shadow-md"
                      title="Dodaj do koszyka"
                    >
                      <Plus size={24} />
                    </button>
                  </div>
                </div>
              ))}
            </div>
          ) : (
            <div className="text-center py-20 bg-white rounded-2xl border-2 border-dashed border-slate-200 text-slate-400">
              {appliedSearchTerm ? `Nie znaleziono produktów dla "${appliedSearchTerm}"` : "Brak produktów w tej kategorii."}
            </div>
          )}

          {/* Pagination */}
          {!appliedSearchTerm && (
            <div className="flex justify-center items-center gap-4 mt-12">
              <button
                onClick={() => setPage((p) => Math.max(1, p - 1))}
                disabled={page === 1}
                className="px-6 py-2.5 rounded-xl bg-white border-2 border-slate-200 text-slate-900 font-bold disabled:opacity-30 hover:border-amber-500 hover:text-amber-600 transition-all"
              >
                Poprzednia
              </button>
              <span className="text-slate-900 font-black text-lg">Strona {page}</span>
              <button
                onClick={() => setPage((p) => p + 1)}
                disabled={!products || products.length < 12}
                className="px-6 py-2.5 rounded-xl bg-white border-2 border-slate-200 text-slate-900 font-bold disabled:opacity-30 hover:border-amber-500 hover:text-amber-600 transition-all"
              >
                Następna
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};


