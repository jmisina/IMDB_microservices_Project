import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { getProducts, addProduct, updateProduct, deleteProduct } from "../catalog/catalogApi";
import { Search, Plus, Edit, Trash2, X, Package, ChevronLeft, ChevronRight } from "lucide-react";
import { useState } from "react";

export const AdminProducts = () => {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [searchTerm, setSearchTerm] = useState("");
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingProduct, setEditingProduct] = useState<any>(null);

  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: 0,
    stock: 0,
    category: "",
    weight: 0
  });

  const { data: products, isLoading } = useQuery({
    queryKey: ["products", page],
    queryFn: () => getProducts({ pageNumber: page, pageSize: 12 }),
  });


  const addMutation = useMutation({
    mutationFn: addProduct,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      closeModal();
    },
  });

  const updateMutation = useMutation({
    mutationFn: updateProduct,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["products"] });
      closeModal();
    },
  });

  const deleteMutation = useMutation({
    mutationFn: deleteProduct,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ["products"] }),
  });

  const openModal = (product: any = null) => {
    if (product) {
      setEditingProduct(product);
      setFormData({
        name: product.name,
        description: product.description,
        price: product.price,
        stock: product.stock,
        category: Array.isArray(product.category) ? product.category.join(", ") : product.category,
        weight: product.weight || 0
      });
    } else {
      setEditingProduct(null);
      setFormData({ name: "", description: "", price: 0, stock: 0, category: "", weight: 0 });
    }
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setEditingProduct(null);
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    const payload = {
      ...formData,
      category: formData.category.split(",").map(c => c.trim()),
      price: Number(formData.price),
      stock: Number(formData.stock),
      weight: Number(formData.weight)
    };

    if (editingProduct) {
      updateMutation.mutate({ ...payload, id: editingProduct.id });
    } else {
      addMutation.mutate(payload);
    }
  };

  // Local filtering for search (Backend doesn't support search yet based on previous analysis)
  const filteredProducts = products?.filter((p: any) => 
    p.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    p.description.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (isLoading) return <div className="text-black p-10 text-center">Ładowanie produktów...</div>;

  return (
    <div className="bg-white p-6 rounded-xl shadow-md text-black">
      <div className="flex flex-col md:flex-row justify-between items-center gap-4 mb-8">
        <h2 className="text-2xl font-bold flex items-center gap-2">
          <Package className="text-blue-600" /> Zarządzanie Produktami
        </h2>
        
        <div className="flex w-full md:w-auto gap-3">
          <div className="relative flex-grow md:w-64">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400" size={18} />
            <input
              type="text"
              placeholder="Szukaj produktu..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
              className="w-full pl-10 pr-4 py-2 border border-gray-200 rounded-lg focus:ring-2 focus:ring-blue-500 focus:outline-none transition-all"
            />
          </div>
          <button 
            onClick={() => openModal()}
            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2 font-semibold shadow-sm"
          >
            <Plus size={20} /> <span className="hidden sm:inline">Dodaj Produkt</span>
          </button>
        </div>
      </div>

      <div className="overflow-x-auto border border-gray-100 rounded-xl">
        <table className="w-full text-left">
          <thead className="bg-gray-50">
            <tr className="text-gray-600 text-sm uppercase">
              <th className="py-4 px-4">Produkt</th>
              <th className="px-4">Kategoria</th>
              <th className="px-4">Cena</th>
              <th className="px-4">Stan</th>
              <th className="px-4">Akcje</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-gray-100">
            {filteredProducts?.map((p: any) => (
              <tr key={p.id} className="hover:bg-gray-50 transition-colors group">
                <td className="py-4 px-4">
                  <div className="font-bold text-gray-800">{p.name}</div>
                  <div className="text-xs text-gray-500 line-clamp-1">{p.description}</div>
                </td>
                <td className="px-4">
                  <div className="flex flex-wrap gap-1">
                    {Array.isArray(p.category) ? p.category.map((cat: string) => (
                      <span key={cat} className="text-[10px] bg-gray-100 px-2 py-0.5 rounded-full text-gray-600">{cat}</span>
                    )) : <span className="text-[10px] bg-gray-100 px-2 py-0.5 rounded-full text-gray-600">{p.category}</span>}
                  </div>
                </td>
                <td className="px-4 font-semibold text-blue-600">{p.price}zł</td>
                <td className="px-4">
                  <span className={`text-sm font-medium ${p.stock < 10 ? 'text-red-500' : 'text-green-600'}`}>
                    {p.stock} szt.
                  </span>
                </td>
                <td className="px-4">
                  <div className="flex gap-2 opacity-0 group-hover:opacity-100 transition-opacity">
                    <button 
                      onClick={() => openModal(p)}
                      className="p-2 text-blue-600 hover:bg-blue-50 rounded-lg transition-colors"
                      title="Edytuj"
                    >
                      <Edit size={18} />
                    </button>
                    <button 
                      onClick={() => {
                        if (confirm(`Czy na pewno chcesz usunąć produkt ${p.name}?`)) {
                          deleteMutation.mutate(p.id);
                        }
                      }}
                      className="p-2 text-red-500 hover:bg-red-50 rounded-lg transition-colors"
                      title="Usuń"
                      disabled={deleteMutation.isPending}
                    >
                      <Trash2 size={18} />
                    </button>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        {(!filteredProducts || filteredProducts.length === 0) && (
          <div className="py-10 text-center text-gray-500 italic">Nie znaleziono produktów.</div>
        )}
      </div>

      {/* Pagination */}
      <div className="mt-8 flex justify-center items-center gap-4">
        <button
          onClick={() => setPage(p => Math.max(1, p - 1))}
          disabled={page === 1}
          className="p-2 rounded-lg border border-gray-200 hover:bg-gray-50 disabled:opacity-30 disabled:hover:bg-white transition-all"
        >
          <ChevronLeft size={20} />
        </button>
        <span className="font-bold text-gray-700">Strona {page}</span>
        <button
          onClick={() => setPage(p => p + 1)}
          disabled={!products || products.length < 10}
          className="p-2 rounded-lg border border-gray-200 hover:bg-gray-50 disabled:opacity-30 disabled:hover:bg-white transition-all"
        >
          <ChevronRight size={20} />
        </button>
      </div>

      {/* Add/Edit Modal */}
      {isModalOpen && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center p-4 z-50">
          <div className="bg-white rounded-2xl shadow-2xl max-w-lg w-full overflow-hidden animate-in zoom-in duration-200">
            <div className="p-6">
              <div className="flex justify-between items-center mb-6">
                <h3 className="text-xl font-bold">{editingProduct ? 'Edytuj Produkt' : 'Nowy Produkt'}</h3>
                <button onClick={closeModal} className="text-gray-400 hover:text-gray-600">
                  <X size={24} />
                </button>
              </div>

              <form id="productForm" onSubmit={handleSubmit} className="space-y-4">
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Nazwa produktu</label>
                  <input
                    type="text"
                    required
                    value={formData.name}
                    onChange={(e) => setFormData({...formData, name: e.target.value})}
                    className="w-full border border-gray-200 rounded-lg p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                  />
                </div>

                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-1">Opis</label>
                  <textarea
                    required
                    rows={3}
                    value={formData.description}
                    onChange={(e) => setFormData({...formData, description: e.target.value})}
                    className="w-full border border-gray-200 rounded-lg p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                  />
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Cena (zł)</label>
                    <input
                      type="number"
                      step="0.01"
                      required
                      value={formData.price}
                      onChange={(e) => setFormData({...formData, price: Number(e.target.value)})}
                      className="w-full border border-gray-200 rounded-lg p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Stan magazynowy</label>
                    <input
                      type="number"
                      required
                      value={formData.stock}
                      onChange={(e) => setFormData({...formData, stock: Number(e.target.value)})}
                      className="w-full border border-gray-200 rounded-lg p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                  </div>
                </div>

                <div className="grid grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Kategorie (po przecinku)</label>
                    <input
                      type="text"
                      placeholder="Materiały, Narzędzia"
                      required
                      value={formData.category}
                      onChange={(e) => setFormData({...formData, category: e.target.value})}
                      className="w-full border border-gray-200 rounded-lg p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                  </div>
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Waga (kg)</label>
                    <input
                      type="number"
                      step="0.1"
                      value={formData.weight}
                      onChange={(e) => setFormData({...formData, weight: Number(e.target.value)})}
                      className="w-full border border-gray-200 rounded-lg p-2 focus:ring-2 focus:ring-blue-500 focus:outline-none"
                    />
                  </div>
                </div>
              </form>
            </div>

            <div className="bg-gray-50 p-6 flex gap-3">
              <button
                type="button"
                onClick={closeModal}
                className="flex-1 px-4 py-2 bg-white border border-gray-300 rounded-lg font-bold text-gray-700 hover:bg-gray-100 transition-colors"
              >
                Anuluj
              </button>
              <button
                form="productForm"
                type="submit"
                disabled={addMutation.isPending || updateMutation.isPending}
                className="flex-1 px-4 py-2 bg-blue-600 text-white rounded-lg font-bold hover:bg-blue-700 disabled:opacity-50 transition-all shadow-md shadow-blue-200"
              >
                {addMutation.isPending || updateMutation.isPending ? 'Zapisywanie...' : (editingProduct ? 'Zapisz zmiany' : 'Dodaj produkt')}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};
